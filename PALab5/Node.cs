using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PALab5
{
    class Node
    {
        public int Key;
        public double[] Distances = new double[Algorithm.AmountOfCities];
        public double[] Pheromones = new double[Algorithm.AmountOfCities];
    }
}
