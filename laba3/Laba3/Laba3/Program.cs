using System;

namespace Laba3
{
    class Program
    {
        static void Main(string[] args)
        {
            StartProgram.Init();
            Answer[] answers = new Answer[MainParams.COLONY_LIFETIME];
            bool flag = false;

            //for (int i = 0; i < MainParams.AMOUNT_OF_VERTICES; i++)
            //{
            //    for (int j = 0; j < MainParams.AMOUNT_OF_VERTICES; j++)
            //    {
            //        Console.Write(MainParams.Distances[i,j] + "  ");
            //    }
            //    Console.WriteLine();
            //    Console.WriteLine();
            //}

            for (int i = 0; i < MainParams.COLONY_LIFETIME; i++)
            {

                StartProgram.PlaceAnts();
                answers[i] = new Answer();
                MainParams.UpdatedHormoneConcentration = new double[MainParams.AMOUNT_OF_VERTICES, MainParams.AMOUNT_OF_VERTICES];


                for (int j = 0; j < StartProgram.AllAnts.Length; j++)
                {
                    var indexOfVertex = 0;
                    while (StartProgram.AllAnts[j].VisitedVertices.Count != MainParams.AMOUNT_OF_VERTICES)
                    {
                        
                        var max = (double)int.MinValue;
                        for (int v = 0; v < MainParams.AMOUNT_OF_VERTICES; v++)
                        {
                            if (!StartProgram.AllAnts[j].VisitedVertices.Contains(v) && v != StartProgram.AllAnts[j].InitialVertex)
                            {
                                var probability = CountWayProbability(StartProgram.AllAnts[j], StartProgram.AllAnts[j].CurrentVertex, v);

                                if (probability > max)
                                {
                                    max = probability;
                                    indexOfVertex = v;
                                    flag = true;
                                }
                            }
                        }

                        if (flag)
                        {
                            StartProgram.AllAnts[j].VisitedVertices.Add(indexOfVertex);
                            StartProgram.AllAnts[j].RouteLength += MainParams.Distances[StartProgram.AllAnts[j].CurrentVertex, indexOfVertex];
                            StartProgram.AllAnts[j].CurrentVertex = indexOfVertex;
                            flag = false;
                        }
                    }

                    PreUpdatePheromones(j);
                }

                for (int a = 0; a < StartProgram.AllAnts.Length; a++)
                {
                    var currRouteLength = StartProgram.AllAnts[a].RouteLength;

                    if (currRouteLength < answers[i].RouteLength)
                    {
                        answers[i].RouteLength = currRouteLength;
                        answers[i].ShortestRoute = StartProgram.AllAnts[a].VisitedVertices;
                    }
                }

                UpdatePheromones();

                PrintResult(answers[i], i);
                //Console.WriteLine( "\n\n");
                //PrintHormoneConcentration();
            }
        }   



        private static double CountWayProbability(Ant ant, int startVertex, int endVertex)
        {
            var probability = (Math.Pow(MainParams.HormoneConcentration[startVertex, endVertex], MainParams.ALPHA)) * (Math.Pow(MainParams.Visibility[startVertex, endVertex], MainParams.BETA));
            double sum = 0;
            for (int i = 0; i < MainParams.AMOUNT_OF_VERTICES; i++)//cycle of bypassing vertices
            {
                if (!ant.VisitedVertices.Contains(i))
                {
                    sum += (Math.Pow(MainParams.HormoneConcentration[startVertex, i], MainParams.ALPHA)) * (Math.Pow(MainParams.Visibility[startVertex, i], MainParams.BETA));
                }
            }
            probability = probability / sum;

            return probability;
        }


        private static void PreUpdatePheromones(int indexOfAnt)
        {
            StartProgram.AllAnts[indexOfAnt].RouteLength += MainParams.Distances[StartProgram.AllAnts[indexOfAnt].CurrentVertex, StartProgram.AllAnts[indexOfAnt].InitialVertex];

            for (int p = 0; p < StartProgram.AllAnts[indexOfAnt].VisitedVertices.Count - 1; p++)
            {

                if (StartProgram.AllAnts[indexOfAnt].IsElite)
                {
                    MainParams.UpdatedHormoneConcentration[p, p + 1] = (MainParams.ShortestRouteLength / StartProgram.AllAnts[indexOfAnt].RouteLength) * 2;
                }
                else
                {
                    MainParams.UpdatedHormoneConcentration[p, p + 1] = MainParams.ShortestRouteLength / StartProgram.AllAnts[indexOfAnt].RouteLength;
                }
            }
            MainParams.UpdatedHormoneConcentration[StartProgram.AllAnts[indexOfAnt].CurrentVertex, StartProgram.AllAnts[indexOfAnt].InitialVertex] = MainParams.ShortestRouteLength / StartProgram.AllAnts[indexOfAnt].RouteLength;
        }


        private static void UpdatePheromones()
        {
            for (int i = 0; i < MainParams.AMOUNT_OF_VERTICES; i++)
            {
                for (int j = 0; j < MainParams.AMOUNT_OF_VERTICES; j++)
                {
                    var temp = MainParams.HormoneConcentration[i, j];
                    temp = temp * (1 - MainParams.RHO) + MainParams.UpdatedHormoneConcentration[i,j];
                    MainParams.HormoneConcentration[i, j] = temp;
                }
            }
        }


        private static void PrintResult(Answer answer, int iteration)
        {
            Console.WriteLine("The shortest route in " + iteration + " iteration: " + answer.RouteLength);
            Console.WriteLine();
            Console.WriteLine("The route consists of: ");
            for (int p = 0; p < answer.ShortestRoute.Count; p++)
            {

                if (answer.ShortestRoute.Count - 1 == p)
                {
                    Console.Write(answer.ShortestRoute[p]);
                }
                else
                {
                    Console.Write(answer.ShortestRoute[p] + " -> ");
                }

            }
            Console.WriteLine("\n\n");
        }


        private static void PrintHormoneConcentration()
        {
            for (int i = 0; i < MainParams.AMOUNT_OF_VERTICES; i++)
            {
                for (int j = 0; j < MainParams.AMOUNT_OF_VERTICES; j++)
                {
                    Console.Write(MainParams.HormoneConcentration[i, j] + "  ");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
