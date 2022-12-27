using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5PA
{
    public static class Algorithm
    {
        public static int StartPosition;
        private static Node[] Nodes = new Node[150];
        public static int A = 2;
        public static int B = 3;
        public static double R = 0.4;
        public static double Lmin;

        public static void InitMap()
        {
            Random rnd = new Random();
            for (int i = 0; i < 150; i++)
            {
                Nodes[i] = new Node();
                Nodes[i].Key = i;
            }
            for (int i = 0; i < 150; i++)
            {
                for (int j = 0; j < 150; j++)
                {
                    Nodes[i].Pheromones[j] = 0.5;
                    if (Nodes[i].Key == j)
                    {
                        Nodes[i].Distances[j] = 0;
                    }
                    else if (Nodes[j].Distances[i] == 0)
                    {
                        double num = rnd.Next(5, 50);
                        Nodes[i].Distances[j] = num;
                        Nodes[j].Distances[i] = num;
                    }
                    else if (Nodes[j].Distances[i] != 0)
                    {
                        Nodes[i].Distances[j] = Nodes[j].Distances[i];
                    }


                }
            }
        }
        public static void AntAlgorithm()
        {
            InitMap();
            Lmin = GreedyAlgorithm();

            Random rnd = new Random();
            List<Ant> ants = new List<Ant>();



            for (int i = 0; i < 100; i++)
            {
                ants.Clear();
                for (int j = 0; j < 35; j++)
                {
                    ants.Add(new Ant(Nodes[0], Nodes.ToList()));
                }

                for (int j = 0; j < 149; j++)
                {
                    for (int k = 0; k < ants.Count; k++)
                    {
                        ants[k].Move(A, B);
                    }
                }
                for (int j = 0; j < 150; j++)
                {
                    for (int k = 0; k < 149; k++)
                    {
                        Nodes[j].Pheromones[k] *= 1 - R;
                    }
                }
                for (int k = 0; k < ants.Count; k++)
                {
                    ants[k].SpreadPheromone();
                }

                for (int k = 0; k < ants.Count; k++)
                {
                    ants[k].Move(A, B);
                }
            }

            for (int i = 0; i < ants.Count; i++)
            {
                string path = "";
                for (int j = 0; j < ants[i].ExploredNodes.Count; j++)
                {
                    path += ants[i].ExploredNodes[j].Key.ToString() + " - ";
                }

                Console.WriteLine(path + "\n");
            }

            int repeats = 0;
            for (int i = 0; i < ants.Count; i++)
            {
                int rep = 0;
                for (int j = 0; j < ants.Count; j++)
                {
                    if (CompareLists(ants[i].ExploredNodes, ants[j].ExploredNodes))
                    {
                        rep++;
                    }
                }
                if (rep > repeats)
                {
                    repeats = rep;
                }
            }

            Console.WriteLine(repeats);

        }

        public static double GreedyAlgorithm()
        {
            Ant ant = new Ant(Nodes[0], Nodes.ToList());

            for (int i = 0; i < 149; i++)
            {
                ant.Move(0, 1);
            }

            return ant.GetLength();
        }

        private static bool CompareLists(List<Node> list1, List<Node> list2)
        {
            bool flag = true;
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i])
                {
                    flag = false;
                }
            }
            return flag;
        }
    }
}
