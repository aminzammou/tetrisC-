using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TetrisClient.Model;

namespace TetrisClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SingleplayerWindow : Window
    {
        //private static int offsetY = 0;
        //private static int offsetX = 0;
        private DispatcherTimer dropCurrentTetrominoDispatchtimer;
        //private Tetromino CurrentTetromino;
        private TetrisEngine tetrisEngine;

        public SingleplayerWindow()
        {

            InitializeComponent();

            tetrisEngine = new TetrisEngine(new Random());

            dropCurrentTetrominoDispatchtimer = new DispatcherTimer();
            dropCurrentTetrominoDispatchtimer.Interval = TimeSpan.FromMilliseconds(1000);
            dropCurrentTetrominoDispatchtimer.Tick += new EventHandler( OnDropCurrentTetromino);

            dropCurrentTetrominoDispatchtimer.Start();
        }
        /// <summary>
        /// Dropt de tetromino block zol lang de game nog niet afgelopen is
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDropCurrentTetromino(object sender, EventArgs e) {
            tetrisEngine.DropCurrentTetromino();
            if (tetrisEngine.GameEnded)
            {
                MessageBox.Show("Game Ended ");
                dropCurrentTetrominoDispatchtimer.Stop();
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
            // alles wissen
            TetrisGrid.Children.Clear();

            // bezettingen tekenen
            for (int y = 0; y < tetrisEngine.TetrisGrid.RowCount; y++)
            {
                for (int x = 0; x < tetrisEngine.TetrisGrid.ColCount; x++)
                {
                    var cell = tetrisEngine.TetrisGrid[y, x];
                    if(cell.Bezet)
                    {
                        Rectangle rectangle = new Rectangle()
                        {
                            Width = 25, // Breedte van een 'cell' in de Grid
                            Height = 25, // Hoogte van een 'cell' in de Grid
                            Stroke = Brushes.White, // De rand
                            StrokeThickness = 1, // Dikte van de rand
                            Fill = cell.Brush, // Achtergrondkleur
                        };

                        TetrisGrid.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
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

                    TetrisGrid.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                    Grid.SetRow(rectangle, y + tetrisEngine.CurrentTetromino.OffsetY); // Zet de rij
                    Grid.SetColumn(rectangle, x + tetrisEngine.CurrentTetromino.OffsetX); // Zet de kolom
                }
            }
            DrawNextTetromino();

            // draw score
            Score.Content = "Score: " + tetrisEngine.Score.Punten;
            Lines.Content = "Aantal regels: " + tetrisEngine.Score.Regels;
        }

        private void DrawNextTetromino() {
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
        private void Windows_KeyEvent(Object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Left:
                    tetrisEngine.GoToTheleft();
                    DrawGrid();
                    break;
                case Key.Right:
                    tetrisEngine.GoToTheRight();
                    DrawGrid();
                    break;
                case Key.D:
                    tetrisEngine.RotateToTheLeft();
                    DrawGrid();
                    break;
                case Key.A:
                case Key.Up:
                    tetrisEngine.RotateToTheRight();
                    DrawGrid();
                    break;
                case Key.Down:
                    tetrisEngine.DropCurrentTetromino();
                    DrawGrid();
                    break;
                default:
                    break;
            }
        }
        public void Quit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void PauzeGame(object sender, RoutedEventArgs e)
        {
            if (dropCurrentTetrominoDispatchtimer.IsEnabled)
            {
                dropCurrentTetrominoDispatchtimer.Stop();
            }
            else
            {
                dropCurrentTetrominoDispatchtimer.Start();
            }
        }
    }
}
