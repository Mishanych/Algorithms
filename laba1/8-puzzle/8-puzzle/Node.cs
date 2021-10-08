using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_puzzle
{
    class Node
    {
        public List<Node> Children = new List<Node>();
        public Node Parent;
        public int[,] Puzzle = new int[3,3];
        public int[,] GoalState = new int[3, 3] { {0, 1, 2 }, {3, 4, 5 }, {6, 7, 8 } };
        public int IndexOfSpace = 0;
        public const int col = 3;
        public const int row = 3;
        public int ManhattanDistance = 0;
        public Node(int[,] _puzzle)
        {
            SetPuzzle(_puzzle);
        }

        public void SetPuzzle(int[,] _puzzle)
        {
            for(int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    this.Puzzle[i,j] = _puzzle[i,j];
                }
            }
        }

        public bool IsSamePuzzle(int[,] p)
        {
            var samePuzzle = true;
            for (int i = 0; i < col; i++)
            {
                for (int j = 0; j < row; j++)
                {
                    if(this.Puzzle[i,j] != p[i,j])
                    {
                        samePuzzle = false;
                    }
                }
            }
            return samePuzzle;
        }
        public bool IsGoal()
        {
            int counter = 0;
            for (int i = 0; i < col; i++)
                for (int j = 0; j < row; j++)
                {
                    if (this.Puzzle[i, j] != GoalState[i,j])
                        return false;
                }
            //if (counter == 9)
            //    return true;
            //else
            //    return false;
            return true;
        }
        //public bool IsGoal()
        //{
        //    var lst = new List<int>();
        //    for (int i = 0; i < col; i++)
        //        for (int j = 0; j < row; j++)
        //            lst.Add(Puzzle[i, j]);
        //    var element = lst[0];
        //    for(int i = 1; i < lst.Count; i++)
        //    {
        //        if (element > lst[i])
        //            return false;
        //        element = lst[i];
        //    }
        //    return true;
        //}

        public void ExpandNode()
        {
            var colIndex = 0;
            var rowIndex = 0;
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if(this.Puzzle[i,j] == 0)
                    {
                        rowIndex = i;
                        colIndex = j;
                        break;
                    }
                }
            }

            MoveToRight(Puzzle, rowIndex, colIndex);
            MoveToLeft(Puzzle, rowIndex, colIndex);
            MoveUp(Puzzle, rowIndex, colIndex);
            MoveDown(Puzzle, rowIndex, colIndex);
        }

        public void MoveToRight(int[,] p, int rowIndex, int colIndex)
        {
            if(colIndex < col - 1)
            {
                int[,] copyPuzzle = new int[3, 3];
                CopyPuzzle(p, copyPuzzle);
                int temp = copyPuzzle[rowIndex, colIndex + 1];
                copyPuzzle[rowIndex, colIndex + 1] = copyPuzzle[rowIndex, colIndex];
                copyPuzzle[rowIndex, colIndex] = temp;

                Node child = new Node(copyPuzzle);
                Children.Add(child);
                child.Parent = this;
            }
        }

        public void MoveToLeft(int[,] p, int rowIndex, int colIndex)
        {
            if (colIndex > col - 3)
            {
                int[,] copyPuzzle = new int[3, 3];
                CopyPuzzle(p, copyPuzzle);
                int temp = copyPuzzle[rowIndex, colIndex - 1];
                copyPuzzle[rowIndex, colIndex - 1] = copyPuzzle[rowIndex, colIndex];
                copyPuzzle[rowIndex, colIndex] = temp;

                Node child = new Node(copyPuzzle);
                Children.Add(child);
                child.Parent = this;
            }
        }

        public void MoveUp(int[,] p, int rowIndex, int colIndex)
        {
            if (rowIndex > row - 3)
            {
                int[,] copyPuzzle = new int[3, 3];
                CopyPuzzle(p, copyPuzzle);
                int temp = copyPuzzle[rowIndex - 1, colIndex];
                copyPuzzle[rowIndex - 1, colIndex] = copyPuzzle[rowIndex, colIndex];
                copyPuzzle[rowIndex, colIndex] = temp;

                Node child = new Node(copyPuzzle);
                Children.Add(child);
                child.Parent = this;
            }
        }

        public void MoveDown(int[,] p, int rowIndex, int colIndex)
        {
            if (rowIndex < row - 1)
            {
                int[,] copyPuzzle = new int[3, 3];
                CopyPuzzle(p, copyPuzzle);
                int temp = copyPuzzle[rowIndex + 1, colIndex];
                copyPuzzle[rowIndex + 1, colIndex] = copyPuzzle[rowIndex, colIndex];
                copyPuzzle[rowIndex, colIndex] = temp;

                Node child = new Node(copyPuzzle);
                Children.Add(child);
                child.Parent = this;
            }
        }

        public void CopyPuzzle(int[,] a, int[,] b)
        {
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    b[i, j] = a[i, j];
                }
            }
        }

        public void PrintPuzzle()
        {
            Console.WriteLine();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    Console.Write(Puzzle[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
