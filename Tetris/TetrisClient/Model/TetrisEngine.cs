using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System;
using System.Collections.Generic;

namespace TetrisClient.Model
{
    class TetrisEngine
    {
        public const int TotaalX = 10;
        public const int TotaalY = 16;

        public Score Score { get; }

        public TetrisGrid TetrisGrid { get; }

        public Tetromino CurrentTetromino { get; private set; }

        public Tetromino NextTetromino { get; private set; }

        public bool GameEnded = false;

        private Random random;

        public TetrisEngine(Random random)
        {
            this.Score = new Score();
            this.random = random;
            this.TetrisGrid = new TetrisGrid(TotaalY, TotaalX);
            this.CurrentTetromino = Tetromino.GetRandomShape(random);
            this.NextTetromino = Tetromino.GetRandomShape(random);
        }

        /// <summary>
        /// zorgt er voor dat het tetromino object valt
        /// </summary>
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

                        var cell = TetrisGrid[y + CurrentTetromino.OffsetY, x + CurrentTetromino.OffsetX];
                        cell.Bezet = true;
                        cell.Brush = CurrentTetromino.Brush;

                    }
                }

                //verwijder eventueel hele rijen
                int aantalRijenBezet = 0;
                for (int rowIndex = 0; rowIndex < TetrisGrid.RowCount; rowIndex++)
                {
                    var rijBezet = TetrisGrid.GetRow(rowIndex).All(a => a.Bezet);
                    if(rijBezet)
                    {
                        aantalRijenBezet++;
                        TetrisGrid.ShiftRow(rowIndex);
                    }
                }
                Score.RowsRemoved(aantalRijenBezet);

                // klaar, bereid nu een nieuwe tetromino!
                // maar, als die collide, dan game over!
                
                if (DetectCollision(NextTetromino.Shape, NextTetromino.OffsetY, NextTetromino.OffsetX))
                {
                    // game over
                    GameEnded = true;
                }
                else
                {
                    this.CurrentTetromino = NextTetromino;
                }
                NextTetromino = Tetromino.GetRandomShape(random);
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
                    if (y < 0 || y >= TetrisGrid.RowCount)
                        return true;

                    // als x buiten range, dan collision
                    if (x < 0 || x >= TetrisGrid.ColCount)
                        return true;

                    var bezezetting = TetrisGrid[y, x];
                    if (bezezetting.Bezet)
                        return true;

                }
            }
            return false;
        }
        /// <summary>
        ///  zorgt er voor dat het tetromino object naar links wordt verplaatst door middel van de offset
        /// </summary>
        public void GoToTheleft()
        {
            if (DetectCollision(CurrentTetromino.Shape, CurrentTetromino.OffsetY, CurrentTetromino.OffsetX - 1) == false) {
                CurrentTetromino.OffsetX--;
            }
            
        }
        /// <summary>
        /// zorgt er voor dat het tetromino object naar rechts wordt verplaatst door middel van de offset
        /// </summary>
        public void GoToTheRight()
        {
            if (DetectCollision(CurrentTetromino.Shape, CurrentTetromino.OffsetY, CurrentTetromino.OffsetX + 1) == false)
            {
                CurrentTetromino.OffsetX++;
            }
                
        }
        /// <summary>
        /// zorgt er voor dat het tetromino object geroteerd wordt naar links door middel van het matrix object
        /// </summary>
        public void RotateToTheLeft()
        {
            var newShape = CurrentTetromino.Shape.Rotate90CounterClockwise();

            if (!DetectCollision(newShape, CurrentTetromino.OffsetY, CurrentTetromino.OffsetX))
            {
                CurrentTetromino.Shape = newShape;
            }
        }
        /// <summary>
        /// zorgt er voor dat het tetromino object geroteerd wordt naar rechts door middel van het matrix object
        /// </summary>
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
