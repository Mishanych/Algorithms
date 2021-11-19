using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba3
{
    public static class MainParams
    {
        public const short ALPHA = 3;
        public const short BETA = 2;
        public const float RHO = 0.7f;
        public const int ANTS = 5;
        public const int ELITE_ANTS = 1;
        public const int AMOUNT_OF_VERTICES = 10;
        

        public const int MIN_DISTANCE = 1;
        public const int MAX_DISTANCE = 40;

        public static int[,] Distances;
        public static double[,] Visibility;
        public static double[,] HormoneConcentration;
        public static double[,] UpdatedHormoneConcentration;
        public static float[] InitHormoneConcentrationOptions = new float[3] { 0.1f, 0.2f, 0.3f };

        public static int ShortestRouteLength = 0;

        public const int COLONY_LIFETIME = 100;
    }

    
    public class Answer
    {
        public List<int> ShortestRoute = new List<int>();
        public double RouteLength = int.MaxValue;
    }
}
