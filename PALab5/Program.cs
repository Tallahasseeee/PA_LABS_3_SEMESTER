using System;

namespace PALab5
{
    class Program
    {
        static void Main(string[] args)
        {
            /*int[] a = { 0, 1, 2, 3, 4 };
            int[] b = a[0..3];
            foreach(var c in b)
            {
                Console.WriteLine(c.ToString() + "  \n\n");
            }*/
            Algorithm algorithm = new Algorithm();
            algorithm.Optimisation();
        }
    }
}
