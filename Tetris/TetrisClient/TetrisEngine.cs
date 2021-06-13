using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System;
using System.Collections.Generic;

namespace TetrisClient
{
    class TetrisEngine
    {
        public const int TotaalX = 10;
        public const int TotaalY = 16;
        public int Punten { get; set; }
        public int Regels { get; set; }

        public Bezetting[,] Bezettingen { get; set; }
        public Tetromino CurrentTetromino { get; set; }

        //public Tetromino Nextetromino { get; set; }

        public bool GameEnded = false;

        public TetrisEngine()
        {
            this.Bezettingen = new Bezetting[TotaalY, TotaalX];
            for (int i = 0; i < this.Bezettingen.GetLength(0); i++)
            {
                for (int j = 0; j < this.Bezettingen.GetLength(1); j++)
                {
                    this.Bezettingen[i, j] = new Bezetting();
                }
            }

            this.CurrentTetromino = Tetromino.GetRandomShape();
        }

        public void DropCurrentTetromino()
        {
            var collisionDetected = DetectCollision(CurrentTetromino.Shape, CurrentTetromino.OffsetY + 1, CurrentTetromino.OffsetX);

            if (collisionDetected)
            {
                // geland
                int[,] values = CurrentTetromino.Shape.Value;
                for (int y = 0; y < values.GetLength(0); y++)
                {
                    for (int x = 0; x < values.GetLength(1); x++)
                    {
                        // Als de waarde niet gelijk is aan 1,
                        // dan hoeft die niet getekent te worden:
                        if (values[y, x] != 1) continue;

                        var bezezetting = Bezettingen[y + CurrentTetromino.OffsetY, x + CurrentTetromino.OffsetX];
                        bezezetting.Bezet = true;
                        bezezetting.Brush = CurrentTetromino.Brush;

                    }
                }

                //verwijder eventueel hele rijen
                //var query = from bezetting[] row in values
                //            where row.all(a => a.bezet)
                //            select row;
                //foreach (var row in query)
                //{
                //    if (row.count() != 0) {
                //        // shift
                //        for (int y = row.; y > 0; y--)
                //        {
                //            for (int x = 0; x < bezettingen.getlength(1); x++)
                //            {
                //                bezettingen[y, x] = bezettingen[y - 1, x];
                //            }
                //        }
                //        // bovenste rij, leeg maken
                //        for (int x = 0; x < bezettingen.getlength(1); x++)
                //        {
                //            bezettingen[0, x].bezet = false;
                //        }
                //    }
                //}
                //var bezettingenArray = Bezettingen.OfType<Bezetting[]>().ToArray();
                for (int rowIndex = 0; rowIndex < Bezettingen.GetLength(0); rowIndex++)
                {
                    var rijBezet = Enumerable.Range(0, Bezettingen.GetLength(1)).Select(x => Bezettingen[rowIndex, x]).All(a => a.Bezet);
                    //var rijBezet = bezettingenArray[rowIndex].All(a => a.Bezet);
                    if(rijBezet)
                    {
                        Punten = Punten + 25;
                        Regels++;
                        // shift
                        for (int y = rowIndex; y > 0; y--)
                        {
                            for (int x = 0; x < Bezettingen.GetLength(1); x++)
                            {
                                Bezettingen[y, x] = Bezettingen[y - 1, x];
                            }
                        }
                        // bovenste rij, leeg maken
                        for (int x = 0; x < Bezettingen.GetLength(1); x++)
                        {
                            Bezettingen[0, x].Bezet = false;
                        }
                    }
                }

                // klaar, bereid nu een nieuwe tetromino!
                // maar, als die collide, dan game over!
                var newTetromino = Tetromino.GetRandomShape();
                if (DetectCollision(newTetromino.Shape, newTetromino.OffsetY, newTetromino.OffsetX))
                {
                    // game over
                    GameEnded = true;
                }
                else
                {
                    this.CurrentTetromino = newTetromino;
                }
            }
            else
            {
                this.CurrentTetromino.OffsetY++;
            }
        }



        /// <summary>
        /// Detecteert of de tetromino + offset, een bezetting (of de randen) zou raken.
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="offsetY"></param>
        /// <param name="offsetX"></param>
        /// <returns><c>true</c> als er een collision zou plaatvinden.</returns>
        private bool DetectCollision(Matrix shape, int offsetY, int offsetX)
        {
            // geland
            //int[,] shape = CurrentTetromino.Shape.Value;
            for (int shapeY = 0; shapeY < shape.Value.GetLength(0); shapeY++)
            {
                for (int shapeX = 0; shapeX < shape.Value.GetLength(1); shapeX++)
                {
                    // Als de waarde niet gelijk is aan 1,
                    // dan hoeft die niet getekent te worden:
                    if (shape.Value[shapeY, shapeX] != 1) continue;

                    // bereken y en x op bezettingen space
                    var y = shapeY + offsetY;
                    var x = shapeX + offsetX;

                    // als y buiten range, dan collision
                    if (y < 0 || y >= Bezettingen.GetLength(0))
                        return true;

                    // als x buiten range, dan collision
                    if (x < 0 || x >= Bezettingen.GetLength(1))
                        return true;

                    var bezezetting = Bezettingen[y, x];
                    if (bezezetting.Bezet)
                        return true;

                }
            }
            return false;
        }

        public void GoToTheleft()
        {
            if (DetectCollision(CurrentTetromino.Shape, CurrentTetromino.OffsetY, CurrentTetromino.OffsetX - 1) == false) {
                CurrentTetromino.OffsetX--;
            }
            
        }
        public void GoToTheRight()
        {
            if (DetectCollision(CurrentTetromino.Shape, CurrentTetromino.OffsetY, CurrentTetromino.OffsetX + 1) == false)
            {
                CurrentTetromino.OffsetX++;
            }
                
        }

        public void RotateToTheLeft()
        {
            var newShape = CurrentTetromino.Shape.Rotate90CounterClockwise();

            if (!DetectCollision(newShape, CurrentTetromino.OffsetY, CurrentTetromino.OffsetX))
            {
                CurrentTetromino.Shape = newShape;
            }
        }

        public void RotateToTheRight()
        {
            var newShape = CurrentTetromino.Shape.Rotate90();

            if (!DetectCollision(newShape, CurrentTetromino.OffsetY, CurrentTetromino.OffsetX))
            {
                CurrentTetromino.Shape = newShape;
            }
        }


    }
}
