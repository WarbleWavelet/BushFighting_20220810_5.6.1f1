using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol
{

    public class Msg
    {
        const int maxCnt = 1024;
        byte[] data = new byte[maxCnt];
        /// <summary>存储数据的索引</summary>
        int startIndex = 0;
        /// <summary>头部信息长度</summary>
      const  int headSize = 4;

        public byte[] Data { get => data; set => data = value; }
        public int StartIndex { get => startIndex; set => startIndex = value; }
        public int RemainSize()
        {
            return data.Length - startIndex;
        }

        /// <summary>
        ///  比如外面读了5个字节数据，startIndex就等于5.表示现在“针”停在上次操作的索引上
        /// </summary>
        /// <param name="addCnt"></param>
        public void AddCnt(int addCnt) 
        {
            startIndex += addCnt;
        }

        public void Read()
        {
            while (true)
            {
                if (startIndex < headSize)                                              //只有长度信息
                {
                     return;
                }

                int costSize = BitConverter.ToInt32(data, 0);                           // 读1字节数据， 1字节
                int preCostSize = headSize;                                             // 4字节   
                int totalCostSize = costSize+preCostSize;                               //5字节              
                int haveSize = startIndex - preCostSize;                                //5-4=1字节
                int remainSize = haveSize - costSize;                                   //1-1=0，读完就没了 ，超过1024为负数，报错
                if (0 <= remainSize)                                                    //0<=1-1
                {
                    string s = Encoding.UTF8.GetString(data, preCostSize, costSize);    //读取那1字节的数据
                    Console.WriteLine("解析出一条数据：{0}", s);
                    Array.Copy( data, totalCostSize,                                    //从5，没数据了
                        data, 0,                      
                        remainSize );  
                    startIndex -= (totalCostSize);                                      //读取了就慢慢往回撤
                }
                else
                {
                    Console.WriteLine("数据超过"+maxCnt);
                    break;
                }
            }

        }
    }
}
