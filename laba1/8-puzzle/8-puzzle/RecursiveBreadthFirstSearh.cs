using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _8_puzzle
{
    class RecursiveBreadthFirstSearh
    {
        private int _minHeuristicValue = int.MaxValue;
        /*public Node RBFS(Node startNode, int hLimit)
        {
            if (startNode.IsGoal())
                return startNode;

            startNode.ExpandNode();
            if (startNode.Children.Count == 0)
                return null;
            foreach(var child in startNode.Children)
            {
                child.ManhattanDistance = ManhattanDistance.CalculateManhattanDistance(child);
            }

            while(true)
            {
                var best = startNode.Children[IndexOfSmallestHeuristic(startNode.Children)];

                var alt = startNode.Children[IndexOfSmallestHeuristic(startNode.Children, best)];

                if (best.ManhattanDistance > hLimit)
                    return best;

                var bestAlternative = Math.Min(alt.ManhattanDistance, hLimit);
                var result = RBFS(best, bestAlternative);
            }
        }*/
        
        Node result;
        public Node RBFS(Node node, int hLimit)
        {
            List<List<object>> successors = new List<List<object>>(); 
            if (node.IsGoal())
                return node;
            node.ExpandNode();
            if (node.Children.Count == 0)
                return null;
            int count = -1;

            foreach(var child in node.Children)
            {
                count += 1;
                var dist = ManhattanDistance.CalculateManhattanDistance(child);
                successors.Add(new List<object>() { dist, count, child });
            }
            
            while(successors.Count != 0)
            {
                List<List<object>> sortedList = successors.OrderBy(lst => lst[0]).ToList();
                successors = sortedList;
                Node bestNode = successors[0][2] as Node;
                if (bestNode.ManhattanDistance > hLimit)
                    return null;
                int alternative = (int)successors[1][0];
                result = RBFS(bestNode, Math.Min(hLimit, alternative));
                successors[0] = new List<object>() { bestNode.ManhattanDistance, successors[0][1], bestNode };
                if (result != null)
                    break;
            }
            return result;
        }
        private int IndexOfSmallestHeuristic(List<Node> children, Node node = null)
        {
            _minHeuristicValue = int.MaxValue;
            var index = 0;
            for(int i = 0; i < children.Count; i++)
            {
                if (children[i].ManhattanDistance < _minHeuristicValue && children[i] != node)
                {
                    _minHeuristicValue = children[i].ManhattanDistance;
                    index = i;
                }
            }
            return index;
        }
    }
}
