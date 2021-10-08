using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_puzzle
{
    class ManhattanDistance
    {
        public static int CalculateManhattanDistance(Node currentNode)
        {
            var distance = 0;
            for(int i = 0; i < Node.col; i++)
            {
                for(int j = 0; j < Node.row; j++)
                {
                    var value = currentNode.Puzzle[i, j];
                    var indexJ = value / Node.row;
                    var indexI = value % Node.col;
                    distance += Math.Abs(indexI - i) + Math.Abs(indexJ - j);
                }
            }

            currentNode.ManhattanDistance = distance;
            return distance;
        }
    }
}
