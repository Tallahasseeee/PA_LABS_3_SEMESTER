using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PALab5
{
    class Chromosome
    {
        public Node[] Path = new Node[Algorithm.AmountOfCities];
        public double Length => GetLength();


        public Chromosome(Node[] path)
        {
            for (int i = 0; i < path.Length; i++)
            {
                Path[i] = path[i];
            }
        }
        public double GetLength()
        {
            double length = 0;
            for (int i = 0; i < Path.Length - 1; i++)
            {
                length += Path[i].Distances[Path[i + 1].Key];
            }

            length += Path[Path.Length - 1].Distances[Path[0].Key];

            return length;
        }

    }


}
