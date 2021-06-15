using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TetrisClient.Model
{
    class Tetromino
    {
        public Matrix Shape { get; set; }
        public Brush Brush { get; set; }

        public int OffsetX { get; set; }
        public int OffsetY { get; set; }

        public int Height => Shape.Value.GetLength(1);

        public int Width => Shape.Value.GetLength(0);

        private static Random random = new Random();

        private static Brush[] colors = new Brush[] { Brushes.Red, Brushes.Blue, Brushes.Yellow, Brushes.Green, Brushes.Purple, Brushes.Brown };

        private static Matrix[] shapes = new Matrix[] {new Matrix(new int[,]
                {
                    { 0, 0, 1 },
                    { 1, 1, 1 },
                    { 0, 0, 0 },
                }
            ), new Matrix(new int[,]
                {
                    { 0, 0, 0 },
                    { 1, 1, 1 },
                    { 0, 0, 1 },
                }
            ),
            new Matrix(new int[,]
                {
                    { 0, 1, 1 },
                    { 1, 1, 0 },
                    { 0, 0, 0 },
                }
            ),
             new Matrix(new int[,]
                {
                    { 1, 1, 0 },
                    { 0, 1, 1 },
                    { 0, 0, 0 },
                }
            ),
             new Matrix(new int[,]
                {
                    { 1, 1, 1 },
                    { 0, 1, 0 },
                    { 0, 0, 0 },
                }
            ),
             new Matrix(new int[,]
                {
                    { 1, 1, 0 },
                    { 1, 1, 0 },
                    { 0, 0, 0 },
                }
            ),
             new Matrix(new int[,]
                {
                    { 1, 1, 1 },
                    { 0, 0, 0 },
                    { 0, 0, 0 },
                }
            )
    };

        public Tetromino(Matrix matrix, Brush brush, int offsetx, int offsetY)
        {
            this.Shape = matrix;
            this.Brush = brush;
            this.OffsetX = offsetx;
            this.OffsetY = offsetY;
        }

        /// <summary>
        /// Geeft een random tetromino block terug
        /// </summary>
        /// <returns></returns>
        public static Tetromino GetRandomShape() {
            var shapeIndex = random.Next(shapes.Length);
            var colorIndex = random.Next(colors.Length);
            return new Tetromino(shapes[shapeIndex], colors[colorIndex], 0, 0);

        }


    }
}
