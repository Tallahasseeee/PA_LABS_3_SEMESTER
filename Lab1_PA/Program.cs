using System;
using System.IO;
using System.Diagnostics;

namespace Lab1_PA
{
    class Program
    {
        private static void Print1()
        {
            BinaryReader binaryReaderA = new BinaryReader(new FileStream(Constants.FilePathA, FileMode.Open));
            while (binaryReaderA.BaseStream.Length != binaryReaderA.BaseStream.Position)
            {
                Console.Write(binaryReaderA.ReadInt32() + " , ");
            }
            binaryReaderA.Close();
        }

        private static void Print2()
        {
            BinaryReader binaryReaderA1 = new BinaryReader(new FileStream(Constants.FilePathA, FileMode.Open));
            while (binaryReaderA1.BaseStream.Length != binaryReaderA1.BaseStream.Position)
            {
                Console.Write(binaryReaderA1.ReadInt32() + " , ");
            }
            binaryReaderA1.Close();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введіть кількість чисел: ");
            long amount = Convert.ToInt32(Console.ReadLine());
            Stopwatch sw = new Stopwatch();
            sw.Start();
            FileGenerator.Generate(Constants.FilePathA, amount);
            Console.WriteLine($"[File generated: {sw.ElapsedMilliseconds} ms]");

            //Print1();

            Console.WriteLine("[Sorting started]");
            sw.Restart();
            ModifiedMergeSort.Sort(Constants.FilePathA, Constants.FilePathB, Constants.FilePathC, amount);
            Console.WriteLine($"[File sorted: {sw.ElapsedMilliseconds} ms]");
            //BinaryFileInspector.Inspect(Constants.FilePathA, "File.csv", 4);

            //Print2();
            sw.Restart();
            if (ModifiedMergeSort.CheckFile(Constants.FilePathA))
                Console.WriteLine($"Файл відсортований, перевірка тривала: {sw.ElapsedMilliseconds} ms");
            else
                Console.WriteLine("Файл НЕ відсортований");

            File.Delete(Constants.FilePathA);
            File.Delete(Constants.FilePathB);
            File.Delete(Constants.FilePathC);
        }
    }
}
