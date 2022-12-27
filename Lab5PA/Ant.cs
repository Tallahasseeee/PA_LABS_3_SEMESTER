using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5PA
{
    class Ant
    {
        public List<Node> ExploredNodes = new List<Node>();
        public List<Node> NotExploredNodes = new List<Node>();
        public Node Position;
        public int pheromone = 5;
        public Ant(Node startingPosition, List<Node> allNodes)
        {
            Position = startingPosition;
            ExploredNodes.Add(startingPosition);
            NotExploredNodes = CopyList(allNodes);
            NotExploredNodes.Remove(Position);

        }

        public void Move(int A, int B)
        {
            List<double> probs = new List<double>();

            List<double> nums = new List<double>();
            for (int i = 0; i < NotExploredNodes.Count; i++)
            {
                double num = Math.Pow(ExploredNodes[ExploredNodes.Count - 1].Pheromones[NotExploredNodes[i].Key], A) *Math.Pow(1/ExploredNodes[ExploredNodes.Count - 1].Distances[NotExploredNodes[i].Key], B);
                nums.Add(num);
            }

            for (int i = 0; i < nums.Count; i++)
            {
                probs.Add(nums[i]/Sum(nums));
            }

            Random rnd = new Random();

            double random = rnd.NextDouble();
            //Console.WriteLine(Sum(probs));
            //Console.WriteLine("Sum: " + Sum(probs).ToString() + "\n");
            for (int i = 0; i < probs.Count; i++)
            {
                //System.Console.WriteLine(probs[i]);
                if(random < Sum(probs.GetRange(0, i + 1)))
                {
                    Position = NotExploredNodes[i];
                    NotExploredNodes.Remove(Position);
                    ExploredNodes.Add(Position);
                    i = 1000;
                }

            }

        }


        public void SpreadPheromone()
        {
            double l = GetLength();
            for (int i = 0; i < ExploredNodes.Count - 1; i++)
            {
                ExploredNodes[i].Pheromones[ExploredNodes[i + 1].Key] += Algorithm.Lmin / l;
                ExploredNodes[i + 1].Pheromones[ExploredNodes[i].Key] = ExploredNodes[i].Pheromones[ExploredNodes[i + 1].Key];
            }
            ExploredNodes[ExploredNodes.Count - 1].Pheromones[ExploredNodes[0].Key] += Algorithm.Lmin / l;
            ExploredNodes[0].Pheromones[ExploredNodes[ExploredNodes.Count - 1].Key] = ExploredNodes[ExploredNodes.Count - 1].Pheromones[ExploredNodes[0].Key];
        }

        public double GetLength()
        {
            double length = 0;
            for (int i = 0; i < ExploredNodes.Count -1; i++)
            {
                length += ExploredNodes[i].Distances[ExploredNodes[i + 1].Key];
            }

            length += ExploredNodes[ExploredNodes.Count - 1].Distances[ExploredNodes[0].Key];

            return length;
        }

        public double Sum(List<double> nums)
        {
            double sum = 0;

            foreach(var num in nums)
            {
                sum += num;
            }

            return sum;
        }


        public List<Node> CopyList(List<Node> baseList)
        {
            List<Node> newList = new List<Node>();
            foreach(var element in baseList)
            {
                newList.Add(element);
            }
            return newList;
        }
    }
}
