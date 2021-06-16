using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.AspNetCore.SignalR.Client;
using TetrisClient.Model;
using System.Windows.Shapes;

namespace TetrisClient
{
    public partial class MultiplayerWindow : Window
    {
        private HubConnection _connection;
        private DispatcherTimer dropCurrentTetrominoDispatchtimer;
        private TetrisEngine tetrisEngine1;
        private TetrisEngine tetrisEngine2;

        public MultiplayerWindow()
        {
            InitializeComponent();

            // De url waar de meegeleverde TetrisHub op draait:
            string url = "http://127.0.0.1:5000/TetrisHub"; 
            
            // De Builder waarmee de connectie aangemaakt wordt:
            _connection = new HubConnectionBuilder()
                .WithUrl(url)
                .WithAutomaticReconnect()
                .Build();

            dropCurrentTetrominoDispatchtimer = new DispatcherTimer();
            dropCurrentTetrominoDispatchtimer.Interval = TimeSpan.FromMilliseconds(1000);
            dropCurrentTetrominoDispatchtimer.Tick += new EventHandler(OnDropCurrentTetromino);


            // De eerste paramater moet gelijk zijn met de methodenaam in TetrisHub.cs
            // Wat er tussen de <..> staat bepaald wat de type van de paramater `seed` is.
            // Op deze manier loopt het onderstaande gelijk met de methode in TetrisHub.cs.
            _connection.On<int>("ReadyUp", seed =>
            {
                // Seed van de andere client:
                //P2Random = new Random(seed);
                //MessageBox.Show(seed.ToString());
                Dispatcher.BeginInvoke(() =>
                {
                    tetrisEngine1 = new TetrisEngine(new Random(seed));
                    tetrisEngine2 = new TetrisEngine(new Random(seed));
                    dropCurrentTetrominoDispatchtimer.Start();
                });
            });

            _connection.On("DropShape", () =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    tetrisEngine2.DropCurrentTetromino();
                });
            });
            _connection.On<string>("RotateShape", direction =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (direction == "left")
                        tetrisEngine2.RotateToTheRight();
                    else
                        tetrisEngine2.RotateToTheRight();
                    //TODO: als niet "left" of "right", dan exception
                });
            });
            _connection.On<string>("MoveShape", direction =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    if (direction == "left")
                        tetrisEngine2.GoToTheleft();
                    else
                        tetrisEngine2.GoToTheRight();
                    //TODO: als niet "left" of "right", dan exception
                });

            });

            // Let op: het starten van de connectie moet *nadat* alle event listeners zijn gezet!
            // Als de methode waarin dit voorkomt al `async` (asynchroon) is, dan kan `Task.Run` weggehaald worden.
            // In het startersproject staat dit in de constructor, daarom is dit echter wel nodig:
            Task.Run(async () => await _connection.StartAsync());
        }

        // Events kunnen `async` zijn in WPF:
        private async void StartGame_OnClick(object sender, RoutedEventArgs e)
        {
            // Als de connectie nog niet is geïnitialiseerd, dan kan er nog niks verstuurd worden:
            if (_connection.State != HubConnectionState.Connected)
            {
                return;
            }
            
            int seed = Guid.NewGuid().GetHashCode();

            tetrisEngine1 = new TetrisEngine(new Random(seed));
            tetrisEngine2 = new TetrisEngine(new Random(seed));
            dropCurrentTetrominoDispatchtimer.Start();

            // Het aanroepen van de TetrisHub.cs methode `ReadyUp`.
            // Hier geven we de int mee die de methode `ReadyUp` verwacht.
            await _connection.InvokeAsync("ReadyUp", seed);
        }

        /// <summary>
        /// Beweeg de tetronimo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnDropCurrentTetromino(object sender, EventArgs e)
        {
            tetrisEngine1.DropCurrentTetromino();
            await _connection.InvokeAsync("DropShape");
            //tetrisEngine2.DropCurrentTetromino(); // tijdelijk
            if (tetrisEngine1.GameEnded || tetrisEngine2.GameEnded)
            {
                dropCurrentTetrominoDispatchtimer.Stop();
                MessageBox.Show("Game Ended ");
                
            }
            else
            {
                DrawGrid();
            }
        }

        /// <summary>
        /// creeert en verwijdert het gehele speelveld
        /// </summary>
        private void DrawGrid()
        {
            DrawGame(tetrisEngine1, TetrisGrid, Score, Lines);
            DrawGame(tetrisEngine2, TetrisGrid2, Score2, Lines2);
            DrawNextTetromino(tetrisEngine1, NextTetronimo);
            DrawNextTetromino(tetrisEngine2, NextTetronimo2);
        }

        private void DrawGame(TetrisEngine tetrisEngine, Grid tetrisGrid, Label score, Label lines)
        {
            // alles wissen
            tetrisGrid.Children.Clear();

            // bezettingen tekenen
            for (int y = 0; y < tetrisEngine.TetrisGrid.RowCount; y++)
            {
                for (int x = 0; x < tetrisEngine.TetrisGrid.ColCount; x++)
                {
                    var cell = tetrisEngine.TetrisGrid[y, x];
                    if (cell.Bezet)
                    {
                        Rectangle rectangle = new Rectangle()
                        {
                            Width = 25, // Breedte van een 'cell' in de Grid
                            Height = 25, // Hoogte van een 'cell' in de Grid
                            Stroke = Brushes.White, // De rand
                            StrokeThickness = 1, // Dikte van de rand
                            Fill = cell.Brush, // Achtergrondkleur
                        };

                        tetrisGrid.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                        Grid.SetRow(rectangle, y); // Zet de rij
                        Grid.SetColumn(rectangle, x); // Zet de kolom
                    }
                }
            }

            // current tetromino tekenen
            int[,] values = tetrisEngine.CurrentTetromino.Shape.Value;
            for (int y = 0; y < values.GetLength(0); y++)
            {
                for (int x = 0; x < values.GetLength(1); x++)
                {

                    // Als de waarde niet gelijk is aan 1,
                    // dan hoeft die niet getekent te worden:
                    if (values[y, x] != 1) continue;

                    Rectangle rectangle = new Rectangle()
                    {
                        Width = 25, // Breedte van een 'cell' in de Grid
                        Height = 25, // Hoogte van een 'cell' in de Grid
                        Stroke = Brushes.White, // De rand
                        StrokeThickness = 1, // Dikte van de rand
                        Fill = tetrisEngine.CurrentTetromino.Brush, // Achtergrondkleur
                    };

                    tetrisGrid.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                    Grid.SetRow(rectangle, y + tetrisEngine.CurrentTetromino.OffsetY); // Zet de rij
                    Grid.SetColumn(rectangle, x + tetrisEngine.CurrentTetromino.OffsetX); // Zet de kolom
                }
            }

            // draw score
            score.Content = "Score: " + tetrisEngine.Score.Punten;
            lines.Content = "Aantal regels: " + tetrisEngine.Score.Regels;
        }

        /// <summary>
        /// Tekent de volgende tetromino 
        /// </summary>
        private void DrawNextTetromino(TetrisEngine tetrisEngine, Grid NextTetronimo)
        {
            // alles wissen
            NextTetronimo.Children.Clear();
            int[,] values = tetrisEngine.NextTetromino.Shape.Value;
            for (int y = 0; y < values.GetLength(0); y++)
            {
                for (int x = 0; x < values.GetLength(1); x++)
                {

                    // Als de waarde niet gelijk is aan 1,
                    // dan hoeft die niet getekent te worden:
                    if (values[y, x] != 1) continue;

                    Rectangle rectangle = new Rectangle()
                    {
                        Width = 25, // Breedte van een 'cell' in de Grid
                        Height = 25, // Hoogte van een 'cell' in de Grid
                        Stroke = Brushes.White, // De rand
                        StrokeThickness = 1, // Dikte van de rand
                        Fill = tetrisEngine.NextTetromino.Brush, // Achtergrondkleur
                    };

                    NextTetronimo.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                    Grid.SetRow(rectangle, y + 0); // Zet de rij
                    Grid.SetColumn(rectangle, x + 0); // Zet de kolom
                }
            }
        }
        /// <summary>
        /// Op bassis van de input van de gebruiker worden er keuzes op de tetromino toegepast
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Windows_KeyEvent(Object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    tetrisEngine1.GoToTheleft();
                    await _connection.InvokeAsync("MoveShape", "left"); 
                    DrawGrid();
                    break;
                case Key.Right:
                    tetrisEngine1.GoToTheRight();
                    await _connection.InvokeAsync("MoveShape", "right"); 
                    DrawGrid();
                    break;
                case Key.D:
                    tetrisEngine1.RotateToTheLeft();
                    await _connection.InvokeAsync("RotateShape", "left"); 
                    DrawGrid();
                    break;
                case Key.A:
                    tetrisEngine1.RotateToTheRight();
                    await _connection.InvokeAsync("RotateShape", "right");
                    DrawGrid();
                    break;
                case Key.Down:
                    tetrisEngine1.DropCurrentTetromino();
                    await _connection.InvokeAsync("DropShape");
                    DrawGrid();
                    break;
                default:
                    break;
            }
        }


    }
}
