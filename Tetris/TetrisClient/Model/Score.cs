using System;

namespace TetrisClient.Model
{
    public class Score
    {
        private int[] RijenVerwijderenScore = new int[] {0, 25, 100, 400 };
        public int Punten { get; set; }
        public int Regels { get; set; }

        public Score()
        {
        }

        public void RowsRemoved(int rowCount)
        { 
            this.Punten += RijenVerwijderenScore[rowCount];
            this.Regels += rowCount;

        }
    }
}