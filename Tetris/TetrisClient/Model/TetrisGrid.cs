using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisClient.Model
{
    class TetrisGrid
    {
        private Cell[,] cells;

        public TetrisGrid()
        {
        }

        // indexer
        public Cell this[int rowIndex, int colIndex]
        {
            get
            {
                return cells[rowIndex, colIndex];
            }
        }

        public int RowCount => cells.GetLength(0);

        public int ColCount => cells.GetLength(1);

        public IEnumerable<IEnumerable<Cell>> GetRows()
        {
            for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
            {
                yield return GetRow(rowIndex);
            }
        }

        public IEnumerable<Cell> GetRow(int rowIndex)
        {
            // normale versie
            //var row = new List<Cell>();
            //for (int colIndex = 0; colIndex < ColCount; colIndex++)
            //{
            //    row.Add(this[rowIndex, colIndex]);
            //}
            //return row;

            // yield-return versie
            for (int colIndex = 0; colIndex < ColCount; colIndex++)
            {
                yield return this[rowIndex, colIndex];
            }
        }

        public void ShiftRow(int rowIndex)
        {
            // shift
            for (int y = rowIndex; y > 0; y--)
            {
                for (int x = 0; x < ColCount; x++)
                {
                    cells[y, x] = cells[y - 1, x];
                }
            }
            // bovenste rij, leeg maken
            for (int x = 0; x < ColCount; x++)
            {
                cells[0, x].Bezet = false;
            }
        }

        public TetrisGrid(int rows, int cols)
        {
            cells = new Cell[rows, cols];
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    cells[i, j] = new Cell();
                }
            }
        }
    }
}
