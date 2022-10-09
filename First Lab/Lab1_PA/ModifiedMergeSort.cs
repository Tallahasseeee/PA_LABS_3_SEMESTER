using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_PA
{
    public static class ModifiedMergeSort
    {
        private static long _arraySize = 1024*1024*50/4;
        //private static bool _smolFile = false;
        public static void Divide(string filePathA, string filePathB, string filePathC, long portion)
        {
            BinaryReader binaryReaderA = new BinaryReader(new FileStream(filePathA, FileMode.OpenOrCreate));
            BinaryWriter binaryWriterB = new BinaryWriter(new FileStream(filePathB, FileMode.OpenOrCreate));
            BinaryWriter binaryWriterC = new BinaryWriter(new FileStream(filePathC, FileMode.OpenOrCreate));

            int counter = 0;


            while(binaryReaderA.BaseStream.Length != binaryReaderA.BaseStream.Position)
            {
                if (counter % 2 == 0)
                {
                    for (int i = 0; i < portion/_arraySize; i++)
                    {
                        binaryWriterB.Write(ConvertToBytes(FillArray(binaryReaderA)));
                    }
                }
                else
                {
                    for (int i = 0; i < portion / _arraySize; i++)
                    {
                        binaryWriterC.Write(ConvertToBytes(FillArray(binaryReaderA)));
                    }
                }
                counter++;
            }

            //binaryWriterC.Write(binaryReaderA.ReadBytes((int)(binaryReaderA.BaseStream.Length - binaryReaderA.BaseStream.Position)));

            binaryReaderA.Close();
            binaryWriterB.Close();
            binaryWriterC.Close();

            //Print1(filePathB, filePathC);
        }

        public static void Merge(string filePathA, string filePathB, string filePathC, long portion)
        {
            BinaryWriter binaryWriterA = new BinaryWriter(new FileStream(filePathA, FileMode.OpenOrCreate));
            BinaryReader binaryReaderB = new BinaryReader(new FileStream(filePathB, FileMode.OpenOrCreate));
            BinaryReader binaryReaderC = new BinaryReader(new FileStream(filePathC, FileMode.OpenOrCreate));

            int cntB, cntC, iteratorB, iteratorC;
            int[] b, c;

            while(binaryReaderC.BaseStream.Length != binaryReaderC.BaseStream.Position)
            {
                cntB = 0;
                cntC = 0;
                iteratorB = 0;
                iteratorC = 0;
                b = FillArray(binaryReaderB);
                c = FillArray(binaryReaderC);

                while(cntB < portion && cntC < portion)
                {
                    if(b[iteratorB] <= c[iteratorC])
                    {
                        binaryWriterA.Write(b[iteratorB]);
                        iteratorB++;
                        cntB++;
                        if(cntB < portion)
                        {
                            if(iteratorB == b.Length)
                            {
                                b = FillArray(binaryReaderB);
                                iteratorB = 0;
                            }
                        }
                        else
                        {
                            while(cntC < portion && binaryReaderC.BaseStream.Length != binaryReaderC.BaseStream.Position || iteratorC != c.Length)
                            {
                                binaryWriterA.Write(c[iteratorC]);
                                iteratorC++;
                                cntC++;
                                if (iteratorC == c.Length && cntC < portion && binaryReaderC.BaseStream.Length != binaryReaderC.BaseStream.Position)
                                {
                                    c = FillArray(binaryReaderC);
                                    iteratorC = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        binaryWriterA.Write(c[iteratorC]);
                        iteratorC++;
                        cntC++;
                        if (cntC < portion && binaryReaderC.BaseStream.Length != binaryReaderC.BaseStream.Position || iteratorC != c.Length)
                        {
                            if (iteratorC == c.Length)
                            {
                                c = FillArray(binaryReaderC);
                                iteratorC = 0;
                            }
                        }
                        else
                        {
                            while (cntB < portion)
                            {
                                binaryWriterA.Write(b[iteratorB]);
                                iteratorB++;
                                cntB++;
                                if (iteratorB == b.Length && cntB < portion)
                                {
                                    b = FillArray(binaryReaderB);
                                    iteratorB = 0;
                                }
                            }
                        }
                    }
                }
            }

            binaryWriterA.Close();
            binaryReaderB.Close();
            binaryReaderC.Close();

            //Print2(filePathA);

            File.Delete(filePathB);
            File.Delete(filePathC);
        }
        public static void PreSort(string filePathA, string tempFile = "tempFile")
        {
            BinaryReader binaryReaderA = new BinaryReader(new FileStream(filePathA, FileMode.Open));
            BinaryWriter tempBinaryWriter = new BinaryWriter(new FileStream(tempFile, FileMode.Create));

            //if (binaryReaderA.BaseStream.Length / 4 < _arraySize)
                //_smolFile = true;


            int[] tempArray;
            byte[] loadArray;
            while(binaryReaderA.BaseStream.Length != binaryReaderA.BaseStream.Position)
            {
                tempArray = FillArray(binaryReaderA);
                Array.Sort(tempArray);
                loadArray = ConvertToBytes(tempArray);
                tempBinaryWriter.Write(loadArray);
                
            }

            binaryReaderA.Close();
            tempBinaryWriter.Close();

            BinaryReader tempBinaryReader = new BinaryReader(new FileStream(tempFile, FileMode.Open));
            BinaryWriter binaryWriterA = new BinaryWriter(new FileStream(filePathA, FileMode.Open));

            while (tempBinaryReader.BaseStream.Length != tempBinaryReader.BaseStream.Position)
            {
                binaryWriterA.Write(tempBinaryReader.ReadBytes((int)_arraySize * 4));
            }

            binaryWriterA.Close();
            tempBinaryReader.Close();
            File.Delete(tempFile);
        }

        public static void Sort(string filePathA, string filePathB, string filePathC, long numEl)
        {
            PreSort(filePathA);
            for (long i = _arraySize; i < numEl; i *= 2)
            {
                Divide(filePathA, filePathB, filePathC, i);
                Merge(filePathA, filePathB, filePathC, i);
            }

        }

        
        public static int[] FillArray(BinaryReader binaryReader)
        {
            int[] arr; 
            byte[] binData;
            if (binaryReader.BaseStream.Length - binaryReader.BaseStream.Position > _arraySize * 4)
            {
                binData = binaryReader.ReadBytes((int)_arraySize * 4);
            }
            else
            {
                binData = binaryReader.ReadBytes((int)(binaryReader.BaseStream.Length - binaryReader.BaseStream.Position));
            }
            arr = new int[binData.Length / 4];
            for (int i = 0; i < binData.Length/4; i++)
            {
                byte[] numInBytes = new byte[4];
                for (int j = 0; j < 4; j++)
                {
                    numInBytes[j] = binData[4*i + j];
                }
                arr[i] = BitConverter.ToInt32(numInBytes);
            }
            return arr;
        }
        public static byte[] ConvertToBytes(int[] arr)
        {
            byte[] binArr = new byte[arr.Length * 4];
            for (int i = 0; i < arr.Length; i++)
            {
                binArr[4 * i] = (byte)arr[i];
                binArr[4 * i + 1] = (byte)(arr[i] >> 8);
                binArr[4 * i + 2] = (byte)(arr[i] >> 0x10);
                binArr[4 * i + 3] = (byte)(arr[i] >> 0x18);
            }
            return binArr;
        }

        public static bool CheckFile(string filePathA)
        {
            using (BinaryReader binaryReader = new BinaryReader(new FileStream(filePathA, FileMode.Open)))
            {
                int[] arr = FillArray(binaryReader);
                int prev = arr[0];
                while (binaryReader.BaseStream.Length != binaryReader.BaseStream.Position)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i] >= prev)
                        {
                            prev = arr[i];
                        }
                        else
                        {
                            return false;
                        }
                    }
                    arr = FillArray(binaryReader);
                }
                return true;
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
