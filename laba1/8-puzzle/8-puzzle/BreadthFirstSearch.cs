using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_puzzle
{
    class BreadthFirstSearch
    {
        public static int AmountOfGeneratedStates = 0;
        public static int AmountOfStatesToGoal = 0;

        public List<Node> BFS(Node root)
        {
            List<Node> pathToSolution = new List<Node>();
            List<Node> openedList = new List<Node>();
            List<Node> closedList = new List<Node>();

            openedList.Add(root);
            bool goalFound = false;
            AmountOfGeneratedStates++;
            while(openedList.Count > 0 && !goalFound)
            {
                Node currentNode = openedList[0];
                closedList.Add(currentNode);
                openedList.RemoveAt(0);

                currentNode.ExpandNode();
                //currentNode.PrintPuzzle();
                //Console.ReadKey();
                AmountOfGeneratedStates++;
                for (int i = 0; i < currentNode.Children.Count; i++)
                {
                    Node currentChild = currentNode.Children[i];
                    if(currentChild.IsGoal())
                    {
                        Console.WriteLine("\n\n---------------------Goal found!---------------------");
                        goalFound = true;
                        PathTrace(pathToSolution, currentChild);
                    }

                    if (!Contains(openedList, currentChild) && !Contains(closedList, currentChild))
                    {
                        openedList.Add(currentChild);
                    }
                }
            }

            

            return pathToSolution;
        }


        public void PathTrace(List<Node> path, Node node)
        {
            Node currentNode = node;
            path.Add(currentNode);
            while(currentNode.Parent != null)
            {
                currentNode = currentNode.Parent;
                path.Add(currentNode);
            }
        }

        public bool Contains(List<Node> list, Node node)
        {
            var contains = false;
            for(int i = 0; i < list.Count; i++)
            {
                if(list[i].IsSamePuzzle(node.Puzzle))
                {
                    contains = true;
                }    
            }
            return contains;
        }
    }
}
