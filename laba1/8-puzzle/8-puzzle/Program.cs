using System;
using System.Collections.Generic;

namespace _8_puzzle
{
    class Program
    {
        static void Main(string[] args)
        {

            int[,] puzzle = new int[3, 3];// { { 1, 2, 4 }, { 3, 0, 5 }, { 7, 6, 8 } };//Generator.GeneratePuzzle();
            
            var falseStates = 0;

            var isSolvable = false;
            while(!isSolvable)
            {
                puzzle = Generator.GeneratePuzzle();
                isSolvable = IsSolvable(puzzle);
                if(!isSolvable)
                    falseStates++;
            }
           // puzzle = new int[3, 3] { { 1, 2, 4 }, { 3, 0, 5 }, { 7, 6, 8 } };
            puzzle = new int[3, 3] { { 1, 2, 0 }, { 3, 4, 5 }, { 6, 7, 8 } };

            Node firstNode = new Node(puzzle);
            firstNode.PrintPuzzle();
            //BreadthFirstSearch bfs = new BreadthFirstSearch();
            //List<Node> solution = bfs.BFS(firstNode);
            RecursiveBreadthFirstSearh rbfs = new RecursiveBreadthFirstSearh();
            var res = rbfs.RBFS(firstNode, 10)?.Puzzle;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(res[i, j] + " ");
                }
                Console.WriteLine();
            }
            //if(solution.Count > 0)
            //{
            //    solution.Reverse();
            //    for(int i = 0; i < solution.Count; i++)
            //    {
            //        solution[i].PrintPuzzle();
            //    }
                
            //}
            //else
            //{
            //    Console.WriteLine("No path solution is found:(");
            //}
            //Console.WriteLine("False states: " + falseStates);
            //Console.WriteLine("Total states to goal: " + solution.Count);
            //Console.WriteLine("Total states: " + BreadthFirstSearch.AmountOfGeneratedStates);
        }

        private static int GetInvCount(int[,] arr)
        {
            int inv_count = 0;
            for (int i = 0; i < 3 - 1; i++)
                for (int j = i + 1; j < 3; j++)
                    if (arr[j, i] > 0 && arr[j, i] > arr[i, j])
                        inv_count++;
            return inv_count;
        }
        private static bool IsSolvable(int[,] puzzle)
        {
            int invCount = GetInvCount(puzzle);
            return (invCount % 2 == 0);
        }
    }
}
