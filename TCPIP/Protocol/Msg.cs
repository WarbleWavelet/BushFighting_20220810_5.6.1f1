using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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




        #region Read

        public void Read(int newDataSize)
        {
            startIndex += newDataSize;
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


        /// <summary>
        /// 解析数据或者叫做读取数据
        /// </summary>
        public void Read(int newDataAmount, Action<ActionCode, string> processDataCallback)
        {
            startIndex += newDataAmount;
            while (true)
            {
                if (startIndex <= 4) return;
                int count = BitConverter.ToInt32(data, 0);
                if ((startIndex - 4) >= count)
                {

                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 4);
                    string s = Encoding.UTF8.GetString(data, 8, count - 4);
                    processDataCallback(actionCode, s);
                    Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                    startIndex -= (count + 4);
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 解析数据或者叫做读取数据
        /// </summary>
        public void Read(int newDataAmount, Action<ReqCode, ActionCode, string> processDataCallback)
        {
            startIndex += newDataAmount;
            while (true)
            {
                if (startIndex <= headSize) return;
                int count = BitConverter.ToInt32(data, 0);
                if ((startIndex - headSize) >= count)
                {
                    ReqCode requestCode = (ReqCode)BitConverter.ToInt32(data, headSize);
                    ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 8);
                    string s = Encoding.UTF8.GetString(data, 12, count - 8);
                    processDataCallback(requestCode, actionCode, s);
                    Array.Copy(data, count + headSize, data, 0, startIndex - headSize - count);
                    startIndex -= (count + headSize);
                }
                else
                {
                    break;
                }
            }
        }
        #endregion



        public static byte[] PackData(ReqCode reqData, ActionCode actionCode, string data)
        {
            byte[] reqCodeBytes = BitConverter.GetBytes((int)reqData);
            byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataAmount = reqCodeBytes.Length + dataBytes.Length + actionCodeBytes.Length;
            byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);


            return dataAmountBytes
                .Concat(reqCodeBytes).ToArray<byte>()
                .Concat(actionCodeBytes).ToArray<byte>()
                .Concat(dataBytes).ToArray<byte>();
        }
        public static byte[] PackData(ActionCode actionCode, string data)
        {
            byte[] requestCodeBytes = BitConverter.GetBytes((int)actionCode);
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            int dataAmount = requestCodeBytes.Length + dataBytes.Length;
            byte[] dataAmountBytes = BitConverter.GetBytes(dataAmount);
            byte[] newBytes = dataAmountBytes.Concat(requestCodeBytes).ToArray<byte>();//Concat(dataBytes);
            return newBytes.Concat(dataBytes).ToArray<byte>();
        }
    }


    /// <summary>
    ///  对象
    /// </summary>
    public enum ReqCode
    {
        None,
        User,
        Room,
        Game
    }

    /// <summary>
    /// 动作
    /// </summary>
    public enum ActionCode
    {
        None,
        Login,
        Register,
        ListRoom,
        CreateRoom,
        JoinRoom,
        UpdateRoom,
        QuitRoom,
        StartGame,
        ShowTimer,
        StartPlay,
        Move,
        Shoot,
        Attack,
        GameOver,
        UpdateResult,
        QuitBattle
    }


    public enum RoleType
    {
        Blue,
        Red
    }


    public enum ReturnCode
    {
        Success,
        Fail,
        NotFound
    }
}
