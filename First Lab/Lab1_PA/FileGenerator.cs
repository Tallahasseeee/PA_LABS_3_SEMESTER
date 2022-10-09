using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_PA
{
    public static class FileGenerator
    {
        public static void Generate(string filePathA, long integersAmount)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePathA, FileMode.Create)))
            {
                int portion = integersAmount switch
                {
                    < 10 * Constants.Mb / sizeof(Int32) => 1000,
                    (>= 10 * Constants.Mb / sizeof(Int32)) and (< Constants.Gb / sizeof(Int32)) => 100000,
                    >= Constants.Gb / sizeof(Int32) => 100000000,
                };

                Random rng = new Random();
                byte[] buffer = new byte[sizeof(Int32) * portion];
                int data;
                for (int i = 0; i < (int)(integersAmount / portion); i++)
                {
                    for (int j = 0; j < portion; j++)
                    {
                        data = rng.Next(Int32.MinValue, Int32.MaxValue);
                        buffer[4 * j] = (byte)data;
                        buffer[4 * j + 1] = (byte)(data >> 8);
                        buffer[4 * j + 2] = (byte)(data >> 0x10);
                        buffer[4 * j + 3] = (byte)(data >> 0x18);
                    }
                    writer.Write(buffer);
                }

                int lastPortionLength = (int)(integersAmount - integersAmount / portion * portion);
                buffer = new byte[sizeof(Int32) * lastPortionLength];
                for (int k = 0; k < lastPortionLength; k++)
                {
                    data = rng.Next(Int32.MinValue, Int32.MaxValue);
                    buffer[4 * k] = (byte)data;
                    buffer[4 * k + 1] = (byte)(data >> 8);
                    buffer[4 * k + 2] = (byte)(data >> 0x10);
                    buffer[4 * k + 3] = (byte)(data >> 0x18);
                }
                writer.Write(buffer);
            }

        }

        public static bool CheckFile(string filePathA)
        {
            using (BinaryReader binaryReader = new BinaryReader(new FileStream(filePathA, FileMode.Open)))
            {
                int prevTemp = binaryReader.ReadInt32();
                while (binaryReader.BaseStream.Length != binaryReader.BaseStream.Position)
                {
                    int temp = binaryReader.ReadInt32();
                    if (temp >= prevTemp)
                    {
                        prevTemp = temp;
                    }
                    else
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
