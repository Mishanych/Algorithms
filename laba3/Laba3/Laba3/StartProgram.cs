using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba3
{
    public class StartProgram
    {
        public static Ant[] AllAnts;

        public static void Init()
        {
            RandomizeDistances();
            GreedyAlgorithm(MainParams.Distances);
            SetupVisibility();
            SetupHormoneConcentration();
            //PlaceAnts();
        }


        private static void RandomizeDistances()
        {
            MainParams.Distances = new int[MainParams.AMOUNT_OF_VERTICES, MainParams.AMOUNT_OF_VERTICES];

            Random rnd = new Random();

            for (int i = 0; i < MainParams.AMOUNT_OF_VERTICES; i++)
            {
                for (int j = 0; j < MainParams.AMOUNT_OF_VERTICES; j++)
                {

                    if(i == j)
                    {
                        MainParams.Distances[i, j] = -1;
                    }
                    else
                    {
                        if (MainParams.Distances[i,j] == 0)
                        {
                            MainParams.Distances[i, j] = rnd.Next(MainParams.MIN_DISTANCE, MainParams.MAX_DISTANCE + 1);
                            MainParams.Distances[j, i] = MainParams.Distances[i, j];
                        }                        
                    }
                }
            }
        }
       

        private static void GreedyAlgorithm(int[,] distances)
        {
            int sum = 0;
            int counter = 0;
            int j = 0, i = 0;
            int min = int.MaxValue;

            List<int> visitedRouteList = new List<int>();

            // Starting from the 0th indexed
            // city i.e., the first city
            visitedRouteList.Add(0);
            int[] route = new int[distances.Length];

            // Traverse the adjacency
            // matrix tsp[,]
            while (i < distances.GetLength(0) &&
                   j < distances.GetLength(1))
            {

                // Corner of the Matrix
                if (counter >= distances.GetLength(0) - 1)
                {
                    break;
                }

                // If this path is unvisited then
                // and if the cost is less then
                // update the cost
                if (j != i &&
                    !(visitedRouteList.Contains(j)))
                {
                    if (distances[i, j] < min)
                    {
                        min = distances[i, j];
                        route[counter] = j + 1;
                    }
                }
                j++;

                // Check all paths from the
                // ith indexed city
                if (j == distances.GetLength(0))
                {
                    sum += min;
                    min = int.MaxValue;
                    visitedRouteList.Add(route[counter] - 1);

                    j = 0;
                    i = route[counter] - 1;
                    counter++;
                }
            }

            // Update the ending city in array
            // from city which was last visited
            i = route[counter - 1] - 1;

            for (j = 0; j < distances.GetLength(0); j++)
            {
                if ((i != j) && distances[i, j] < min)
                {
                    min = distances[i, j];
                    route[counter] = j + 1;
                }
            }
            sum += min;//distances[visitedRouteList[visitedRouteList.Count - 1], 0];


            MainParams.ShortestRouteLength = sum;
        }


        private static void SetupVisibility()
        {
            MainParams.Visibility = new double[MainParams.AMOUNT_OF_VERTICES, MainParams.AMOUNT_OF_VERTICES];

            for (int i = 0; i < MainParams.AMOUNT_OF_VERTICES; i++)
            {
                for (int j = 0; j < MainParams.AMOUNT_OF_VERTICES; j++)
                {

                    if(i == j)
                    {
                        MainParams.Visibility[i, j] = 0;
                    }
                    else
                    {
                        MainParams.Visibility[i, j] = 1 / (double)MainParams.Distances[i, j];
                    }
                }
            }
        }


        private static void SetupHormoneConcentration()
        {
            MainParams.HormoneConcentration = new double[MainParams.AMOUNT_OF_VERTICES, MainParams.AMOUNT_OF_VERTICES];
            for (int i = 0; i < MainParams.AMOUNT_OF_VERTICES; i++)
            {
                for (int j = 0; j < MainParams.AMOUNT_OF_VERTICES; j++)
                {

                    if (i == j)
                    {
                        MainParams.HormoneConcentration[i, j] = 0;
                    }
                    else
                    {
                        Random rnd = new Random();
                        var index = rnd.Next(MainParams.InitHormoneConcentrationOptions.Length);
                        MainParams.HormoneConcentration[i, j] = MainParams.InitHormoneConcentrationOptions[index];
                        //MainParams.HormoneConcentration[i, j] = 0.01;
                    }
                }
            }
        }


        public static void PlaceAnts()
        {
            AllAnts = new Ant[MainParams.ANTS];
            var rndNumbers = new List<int>();
            Random rnd = new Random();

            for (int i = 0; i < AllAnts.Length; i++)
            {
                var vertex = rnd.Next(0, MainParams.AMOUNT_OF_VERTICES);

                while(rndNumbers.Contains(vertex))
                {
                    vertex = rnd.Next(0, MainParams.AMOUNT_OF_VERTICES);
                }
                AllAnts[i] = new Ant();
                AllAnts[i].CurrentVertex = vertex;
                AllAnts[i].InitialVertex = vertex;
                AllAnts[i].VisitedVertices.Add(vertex);
                

                if(i < MainParams.ELITE_ANTS)
                {
                    AllAnts[i].IsElite = true;
                }
            }
        }
    }
}
