using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_PA
{
    class StraightMergeSort
    {
        private static void Divide(string filePathA, string filePathB, string filePathC, long portion)
        {
            BinaryReader binaryReaderA = new BinaryReader(new FileStream(filePathA, FileMode.OpenOrCreate));
            BinaryWriter binaryWriterB = new BinaryWriter(new FileStream(filePathB, FileMode.OpenOrCreate));
            BinaryWriter binaryWriterC = new BinaryWriter(new FileStream(filePathC, FileMode.OpenOrCreate));
            int counter = 0;

            while(binaryReaderA.BaseStream.Length != binaryReaderA.BaseStream.Position)
            {
                if (counter % 2 == 0)
                {
                    for (int i = 0; i < portion; i++)
                    {
                        if(binaryReaderA.BaseStream.Length != binaryReaderA.BaseStream.Position)
                            binaryWriterB.Write(binaryReaderA.ReadInt32());
                    }
                }
                else
                {
                    for (int i = 0; i < portion; i++)
                    {
                        if(binaryReaderA.BaseStream.Length != binaryReaderA.BaseStream.Position)
                            binaryWriterC.Write(binaryReaderA.ReadInt32());
                    }
                }
                counter++;
            }
            binaryReaderA.Close();
            binaryWriterB.Close();
            binaryWriterC.Close();

            //Print1(filePathB, filePathC);
        }

        private static void Merge(string filePathA, string filePathB, string filePathC, long portion)
        {
            BinaryWriter binaryWriterA = new BinaryWriter(new FileStream(filePathA, FileMode.OpenOrCreate));
            BinaryReader binaryReaderB = new BinaryReader(new FileStream(filePathB, FileMode.OpenOrCreate));
            BinaryReader binaryReaderC = new BinaryReader(new FileStream(filePathC, FileMode.OpenOrCreate));

            int b, c;
            int cntB, cntC;


            while (binaryReaderC.BaseStream.Length != binaryReaderC.BaseStream.Position)
            {
                cntB = 0;
                cntC = 0;
                b = binaryReaderB.ReadInt32();
                c = binaryReaderC.ReadInt32();
                while (cntB < portion && cntC < portion)
                {
                    if (b <= c)
                    {
                        binaryWriterA.Write(b);
                        cntB++;
                        if (cntB < portion)
                        {
                            //if (binaryReaderB.BaseStream.Length == binaryReaderB.BaseStream.Position)
                                //break;
                            b = binaryReaderB.ReadInt32();
                        }
                        else
                        {

                            while (cntC < portion)
                            {
                                binaryWriterA.Write(c);
                                cntC++;
                                if (cntC < portion && binaryReaderC.BaseStream.Length != binaryReaderC.BaseStream.Position)
                                {
                                    //if (binaryReaderC.BaseStream.Length == binaryReaderC.BaseStream.Position)
                                        //break;
                                    c = binaryReaderC.ReadInt32();
                                }
                            }
                        }
                    }
                    else
                    {
                        binaryWriterA.Write(c);
                        cntC++;
                        if (cntC < portion && binaryReaderC.BaseStream.Length != binaryReaderC.BaseStream.Position)
                        {
                            //if (binaryReaderC.BaseStream.Length == binaryReaderC.BaseStream.Position)
                                //break;
                            c = binaryReaderC.ReadInt32();
                        }
                        else
                        {
                            while (cntB < portion)
                            {
                                binaryWriterA.Write(b);
                                cntB++;
                                if (cntB < portion)
                                {
                                    //if (binaryReaderB.BaseStream.Length == binaryReaderB.BaseStream.Position)
                                        //break;
                                    b = binaryReaderB.ReadInt32();
                                }
                            }
                        }
                    }
                }
                
            }

            while(binaryReaderB.BaseStream.Length != binaryReaderB.BaseStream.Position)
            {
                binaryWriterA.Write(binaryReaderB.ReadInt32());
            }

            binaryWriterA.Close();
            binaryReaderB.Close();
            binaryReaderC.Close();

            //Print2(filePathA);

            File.Delete(filePathB);
            File.Delete(filePathC);
        }

        public static void Sort(string filePathA, string filePathB, string filePathC, long numEl)
        {
            for (long i = 1; i < numEl; i*=2)
            {
                Divide(filePathA,filePathB,filePathC,i);
                Merge(filePathA, filePathB, filePathC, i);
            }
        }




        public static void Print1(string filePathB, string filePathC)
        {
            BinaryReader binaryReaderB = new BinaryReader(new FileStream(filePathB, FileMode.OpenOrCreate));
            BinaryReader binaryReaderC = new BinaryReader(new FileStream(filePathC, FileMode.OpenOrCreate));
            Console.WriteLine("\n\n");
            while (binaryReaderB.BaseStream.Length != binaryReaderB.BaseStream.Position)
            {
                Console.Write(binaryReaderB.ReadInt32() + " , ");
            }
            Console.WriteLine(" ");
            while (binaryReaderC.BaseStream.Length != binaryReaderC.BaseStream.Position)
            {
                Console.Write(binaryReaderC.ReadInt32() + " , ");
            }
            Console.WriteLine("\n\n");
            binaryReaderB.Close();
            binaryReaderC.Close();
        }

        public static void Print2(string filePathA)
        {
            BinaryReader binaryReaderA = new BinaryReader(new FileStream(filePathA, FileMode.OpenOrCreate));
            while (binaryReaderA.BaseStream.Length != binaryReaderA.BaseStream.Position)
            {
                Console.Write(binaryReaderA.ReadInt32() + " , ");
            }
            binaryReaderA.Close();
        }
    }
}
