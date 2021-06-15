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
    public partial class MainWindow : Window
    {
        //private static int offsetY = 0;
        //private static int offsetX = 0;
        private DispatcherTimer dropCurrentTetrominoDispatchtimer;
        //private Tetromino CurrentTetromino;
        private TetrisEngine tetrisEngine;

        public MainWindow()
        {

            InitializeComponent();
            TetrisGrid.ShowGridLines = true;
            //Windows_KeyEvent();

            tetrisEngine = new TetrisEngine();

            dropCurrentTetrominoDispatchtimer = new DispatcherTimer();
            dropCurrentTetrominoDispatchtimer.Interval = TimeSpan.FromMilliseconds(1000);
            dropCurrentTetrominoDispatchtimer.Tick += new EventHandler( OnDropCurrentTetromino);

            dropCurrentTetrominoDispatchtimer.Start();
        }
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
            /*
            //TetrisGrid.Children.RemoveAt(TetrisGrid.Children.OfType<Shape>().Count() - 1);
            TetrisGrid.Children.Clear();

             CurrentTetromino = Tetromino.GetShapeL();

            int[,] values = CurrentTetromino.Shape.Value;
            for (int i = 0; i < values.GetLength(0); i++)
            {
                for (int j = 0; j < values.GetLength(1); j++)
                {

                    // Als de waarde niet gelijk is aan 1,
                    // dan hoeft die niet getekent te worden:
                    if (values[i, j] != 1) continue;

                    Rectangle rectangle = new Rectangle()
                    {
                        Width = 25, // Breedte van een 'cell' in de Grid
                        Height = 25, // Hoogte van een 'cell' in de Grid
                        Stroke = Brushes.White, // De rand
                        StrokeThickness = 1, // Dikte van de rand
                        Fill = CurrentTetromino.Brush, // Achtergrondkleur
                    };

                    TetrisGrid.Children.Add(rectangle); // Voeg de rectangle toe aan de Grid
                    Grid.SetRow(rectangle, i + offsetY); // Zet de rij
                    Grid.SetColumn(rectangle, j + offsetX); // Zet de kolom
                }
            }
            offsetY++;
            */
        }

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

            // draw score
            Score.Content = "Score: " + tetrisEngine.Score.Punten;
            Lines.Content = "Aantal regels: " + tetrisEngine.Score.Regels;
        }

        private void RemoveUiObject()
        {
            //TetrisGrid.Children.RemoveAt(TetrisGrid.Children.OfType<Shape>().Count() - 1);
            //TetrisGrid.Children.Cast<UIElement>().Where(e => e);

        }

        private void Windows_KeyEvent(Object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Left:
                    //MessageBox.Show("links");
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
                    tetrisEngine.RotateToTheRight();
                    DrawGrid();
                    break;
                case Key.Down:
                    tetrisEngine.DropCurrentTetromino();
                    DrawGrid();
                    break;
                default:
                    MessageBox.Show("neee");
                    break;
            }
        }
    }
}
