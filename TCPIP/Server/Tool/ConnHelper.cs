using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GameServer.Tool
{
    /// <summary>
    /// 链接数据库，比较不变的部分
    /// </summary>
    class ConnHelper
    {
        public const string CONNECTIONSTRING_1 = "datasource=127.0.0.1;port=3306;database=bushfighting;user=root;pwd=;"; //这种拿不到conn
        public const string CONNECTIONSTRING = "server=localhost;User Id=root;passwrod=;Database=bushfighting;Charset=utf8";

        public static MySqlConnection Connect()
        {
            MySqlConnection conn = new MySqlConnection(CONNECTIONSTRING);
            try
            {
                conn.Open();
                return conn;
            }
            catch(Exception e)
            {
                Console.WriteLine("链接数据库的时候实现异常：" + e);
                return null;
            }
            
        }
        public static void CloseConnection(MySqlConnection conn)
        {
            if (conn != null)
            {
                 conn.Close();
            }
            else
            {
                Console.WriteLine("MySqlConnection不能为空");
            }
        }
    }


}
