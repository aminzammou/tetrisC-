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

        // Een indexer
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
        /// <summary>
        /// Geeft een row aan cellen terug
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public IEnumerable<Cell> GetRow(int rowIndex)
        {

            // yield-return versie
            for (int colIndex = 0; colIndex < ColCount; colIndex++)
            {
                yield return this[rowIndex, colIndex];
            }
        }
        /// <summary>
        /// Zorgt er voor dat na dat er een rij verwijdert is dat deze worden verplaatst en een nieuwe lege boven aan wordt aangemaakt
        /// </summary>
        /// <param name="rowIndex"></param>
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
        /// <summary>
        /// creert een nieuwe grid
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="cols"></param>
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
