using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Test00_BytesLength(); 
            Test10_BytesLength(); 
        }


        /// <summary>
        /// 字节：数字 空格 字母 1个；汉字 3个
        /// </summary>
        static void Test00_BytesLength()
        {

            byte[] data = Encoding.UTF8.GetBytes("    都 s ， ,");//字符串
            foreach (var item in data)
            {
                Console.Write(item + " ");
            }

            Console.WriteLine();
            //Console.ReadLine();
        }

        /// <summary>
        /// 统统4字节
        /// </summary>
        static void Test10_BytesLength()
        {
            int cnt = 1;
            byte[] data = BitConverter.GetBytes(cnt);//字符串
            foreach (var item in data)
            {
                Console.Write(item + " ");
            }

            Console.WriteLine();

            int cnt2 = 1000000;
            byte[] data2 = BitConverter.GetBytes(cnt2);//字符串
            foreach (var item in data)
            {
                Console.Write(item + " ");
            }

            Console.ReadLine();
        }
    }
}
