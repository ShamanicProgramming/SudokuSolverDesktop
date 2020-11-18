using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SudokuSolverDesktop
{
    class SudokuPuzzle
    {

        public static SudokuPuzzle ThePuzzle = new SudokuPuzzle();

        private int[,] Puzzel;

        public SudokuPuzzle()
        {
            Puzzel = new int[9, 9];
        }

        /// <summary>
        /// Get a list of the values this cell could be based on the other cells
        /// </summary>
        public List<int> GetValidValues(int x, int y)
        {
            List<int> result = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            // Remove values that already exist along the rows/columns
            for (int xCheck = 0; xCheck < 9; xCheck++)
            {
                result.Remove(Puzzel[xCheck, y]);
            }

            for (int yCheck = 0; yCheck < 9; yCheck++)
            {
                result.Remove(Puzzel[x, yCheck]);
            }

            // Remove values that already exist in the square

            // Find which square
            int xSquare = x / 3;
            int ySquare = y / 3;

            // Find the first index of the square
            int startXSquare = xSquare * 3;
            int startYSquare = ySquare * 3;

            // Check the square
            for (int xCheck = startXSquare; xCheck < startXSquare + 3; xCheck++)
            {
                for (int yCheck = startYSquare; yCheck < startYSquare + 3; yCheck++)
                {
                    result.Remove(Puzzel[xCheck, yCheck]);
                }
            }

            return result;
        }

        internal void SetCell(int x, int y, int val)
        {
            Puzzel[x, y] = val;
        }

        public void Solve()
        {
            int x = 0;
            int y = 0;
            SolveNext(x, y);
        }

        /// <summary>
        /// Recursivley check each valid value to find the solution
        /// </summary>
        private bool SolveNext(int x, int y)
        {
            // Check if we're past the end
            if(y > 8)
            {
                return true;
            }

            NextIndexes(x, y, out int xNext, out int yNext);

            // Skip over pre-solved cells
            if (Puzzel[x,y] != 0)
            {
                return SolveNext(xNext, yNext);
            }

            List<int> ValuesToTry = GetValidValues(x, y);

            // Try each valid value
            foreach(int valueToTry in ValuesToTry)
            {
                Puzzel[x, y] = valueToTry;
                if(SolveNext(xNext, yNext))
                {
                    return true;
                }
            }

            Puzzel[x, y] = 0;
            return false;
        }

        private void NextIndexes(int x, int y, out int xNext, out int yNext)
        {
            xNext = x + 1;
            yNext = y;
            if(xNext > 8)
            {
                xNext = 0;
                yNext++;
            }
        }

        internal int GetCell(int x, int y)
        {
            return Puzzel[x, y];
        }
    }
}
