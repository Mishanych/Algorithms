using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba3
{
    public class Ant
    {
        public int CurrentVertex = 0;
        public int InitialVertex = 0;
        public bool IsElite = false;
        public List<int> VisitedVertices = new List<int>();
        public double RouteLength = 0;
    }
}
