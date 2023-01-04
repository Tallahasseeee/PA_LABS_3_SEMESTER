using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PALab5
{
    class Algorithm
    {

        public List<Chromosome> Population = new List<Chromosome>();
        public static int AmountOfCities = 150;
        private Node[] Nodes;
        public int SizeOfPopulation = 10;
        public double MutationProbability = 5;
        public int iterations = 1000;
        Chromosome chromosome1;
        Chromosome chromosome2;
        Chromosome chromosome3;
        public void InitMap()
        {
            Nodes = new Node[AmountOfCities];
            Random rnd = new Random();
            for (int i = 0; i < AmountOfCities; i++)
            {
                Nodes[i] = new Node();
                Nodes[i].Key = i;
            }
            for (int i = 0; i < AmountOfCities; i++)
            {
                for (int j = 0; j < AmountOfCities; j++)
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

        public void InitPopulation()
        {
            Population.Clear();
            Random rnd = new Random();
            for (int i = 0; i < SizeOfPopulation; i++)
            {
                List<Node> list = new List<Node>();
                foreach (var node in Nodes)
                {
                    list.Add(node);
                }
                Node[] initialPath = new Node[AmountOfCities];
                for (int j = 0; j < AmountOfCities; j++)
                {
                    initialPath[j] = list[rnd.Next(0, list.Count)];
                    list.Remove(initialPath[j]);
                }
                Chromosome chromosome = new Chromosome(initialPath);
                if (Repeats(chromosome) > 1) Console.WriteLine("WTF");
                //DisplayChromosome(chromosome);
                Population.Add(chromosome);
            }
        }

        public int Repeats(Chromosome chromosome)
        {
            int reps = 0;
            for (int i = 0; i < AmountOfCities; i++)
            {
                int tempreps = 0;
                for (int j = 0; j < AmountOfCities; j++)
                {
                    if (chromosome.Path[i] == chromosome.Path[j])
                    {
                        tempreps++;
                    }
                }
                if (tempreps > reps)
                {
                    reps = tempreps;
                }
            }
            return reps;
        }

        public void CrossBreeding(Chromosome parent1, Chromosome parent2)
        {
            Random rnd = new Random();
            int border = rnd.Next(1, AmountOfCities - 1);

            Chromosome child1 = new Chromosome(parent1.Path);
            Chromosome child2 = new Chromosome(parent2.Path);

            //if (Repeats(parent1) > 1 || Repeats(parent2) > 1) Console.WriteLine("AAAAAAAAAAAAAAAAAGH");

            int reps1 = Repeats(child1);
            int reps2 = Repeats(child2);

            //Console.WriteLine(reps1.ToString() + "   " + reps2.ToString());

            int counter = border;

            chromosome1 = new Chromosome(parent1.Path);
            chromosome2 = new Chromosome(parent2.Path);

            for (int i = border; i < AmountOfCities; i++)
            {

                if (!Contains(parent2.Path[i], child1.Path[0..border]))
                {
                    child1.Path[counter] = parent2.Path[i];
                    counter++;
                    if (counter == AmountOfCities)
                    {
                        // Console.WriteLine("SHIT"); 
                    }
                }
            }
            if (Repeats(child1) > 1)
            {
            }
            //chromosome2 = new Chromosome(child1.Path);

            foreach (var node in parent1.Path)
            {
                if (!Contains(node, child1.Path))
                {
                    child1.Path[counter] = node;
                    counter++;
                }
            }

            chromosome3 = child1;



            if (Repeats(child1) > 1)
            {
                //Console.WriteLine("WHY");
            }

            counter = border;

            for (int i = border; i < AmountOfCities; i++)
            {

                if (!Contains(parent1.Path[i], child2.Path[0..border]))
                {
                    child2.Path[counter] = parent1.Path[i];
                    counter++;
                }
            }
            if (Repeats(child2) > 1)
            {

            }

            foreach (var node in parent2.Path)
            {
                if (!Contains(node, child2.Path))
                {
                    child2.Path[counter] = node;
                    counter++;
                }
            }

            chromosome3 = child2;

            if (Repeats(child2) > 1)
            {

            }

            reps1 = Repeats(child1);
            reps2 = Repeats(child2);

            child1 = Mutation(child1);
            child2 = Mutation(child2);

            Population.Add(child1);
            Population.Add(child2);

        }


        public Chromosome Mutation(Chromosome chromosome)
        {

            Random rnd = new Random();
            if (rnd.Next(0, 100) < MutationProbability)
            {
                Chromosome mutated = new Chromosome(chromosome.Path);
                int a = rnd.Next(0, AmountOfCities);
                int b = rnd.Next(0, AmountOfCities);
                Node temp = mutated.Path[a];
                mutated.Path[a] = mutated.Path[b];
                mutated.Path[b] = temp;
                if (mutated.Length < chromosome.Length)
                {
                    return mutated;
                }
            }
            return chromosome;
        }
        public bool Contains(Node targetNode, Node[] nodes)
        {
            foreach (var node in nodes)
            {
                if (targetNode == node)
                {
                    return true;
                }
            }
            return false;
        }

        public void NaturalSelection()
        {
            for (int i = 0; i < Population.Count - SizeOfPopulation; i++)
            {
                Chromosome chromosome = Population[0];
                for (int j = 1; j < Population.Count; j++)
                {
                    if (Population[i].Length > chromosome.Length)
                    {
                        chromosome = Population[j];
                    }
                }
                Population.Remove(chromosome);
            }

        }


        public Chromosome FindBestChromosome()
        {
            Chromosome chromosome = Population[0];
            for (int i = 1; i < Population.Count; i++)
            {
                if (chromosome.Length > Population[i].Length)
                {
                    chromosome = Population[i];
                }
            }
            return chromosome;
        }

        public void DisplayChromosome(Chromosome chromosome)
        {
            foreach (var node in chromosome.Path)
            {
                Console.Write(node.Key.ToString() + " - ");
            }
            Console.WriteLine("\n\n");
        }

        public double GeneticAlgorithm()
        {
            Random rnd = new Random();
            for (int i = 0; i < iterations; i++)
            {
                int a = rnd.Next(0, Population.Count);
                int b = rnd.Next(0, Population.Count);
                if (a == b && b < SizeOfPopulation - 2) b++;
                CrossBreeding(Population[a], Population[b]);
                NaturalSelection();
            }

            /*DisplayChromosome(FindBestChromosome());
            Console.WriteLine("\n\n");
            /*Console.WriteLine(FindBestChromosome().Path.Length);
            Console.WriteLine("\n\n");
            Console.WriteLine(Repeats(FindBestChromosome()));
            Console.WriteLine(FindBestChromosome().Length);*/

            return FindBestChromosome().Length;
        }

        public void Optimisation()
        {
            InitMap();
            double optimalSizeOfPopulation = 2;
            double optimalSizeOfPopulationLength = 100000;
            for (int i = 2; i < 100; i++)
            {
                SizeOfPopulation = i;
                InitPopulation();
                double l = GeneticAlgorithm();
                if (l < optimalSizeOfPopulationLength)
                {
                    optimalSizeOfPopulationLength = l;
                    optimalSizeOfPopulation = SizeOfPopulation;
                }

            }

            Console.WriteLine("\n\n");
            Console.WriteLine("Optimal size of population is " + optimalSizeOfPopulation.ToString() + "\nLength: " + optimalSizeOfPopulationLength.ToString());
            Console.WriteLine("\n\n");
            SizeOfPopulation = 10;
            InitPopulation();

            double optimalMutation = 0;
            double optimalMutationLength = 100000;
            for (int i = 0; i < 100; i++)
            {
                InitPopulation();
                MutationProbability = i;
                double l = GeneticAlgorithm();
                if (l < optimalMutationLength)
                {
                    optimalMutationLength = l;
                    optimalMutation = MutationProbability;
                }

            }

            Console.WriteLine("\n\n");
            Console.WriteLine("Optimal mutation is " + optimalMutation.ToString() + "%\n" + "Length: " + optimalMutationLength.ToString());
            Console.WriteLine("\n\n");
            MutationProbability = 5;

            double optimalIterations = 0;
            double optimalIterationsLength = 100000;
            for (int i = 1; i < 10000; i+= 1000)
            {
                InitPopulation();
                iterations = i;
                double l = GeneticAlgorithm();
                Console.WriteLine(l);
                if (l < optimalIterationsLength)
                {
                    optimalIterationsLength = l;
                    optimalIterations = iterations;
                }

            }

            Console.WriteLine("\n\n");
            Console.WriteLine("Optimal iterations is " + optimalIterations.ToString() + "\nLength: " + optimalIterationsLength.ToString());
            Console.WriteLine("\n\n");
            iterations = 1000;



        }


    }
}
