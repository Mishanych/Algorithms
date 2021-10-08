using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_puzzle
{
    class Generator
    {
        public static int[,] GeneratePuzzle()
        {

            Random rnd = new Random();
            int[,] puzzle = new int[Node.col, Node.row];
            List<int> addedValues = new List<int>();

            for (int i = 0; i < Node.col; i++)
            {
                for (int j = 0; j < Node.row; j++)
                {
                    var value = rnd.Next(0, 9);
                    while (addedValues.Contains(value))
                    {
                        value = rnd.Next(0, 9);
                    }
                    addedValues.Add(value);
                    puzzle[i, j] = value;
                }
            }

            return puzzle;
        }
    }
}
