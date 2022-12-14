using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Test_MySql
{
    public class UserTest

    {
        public string username;
        public string password;
        public int id;
    }
    class Program
    {
        static void Main(string[] args)
        {
            DBMgr.Instance.Init("bushfighting");
            // DBMgr.Instance.QueryTable( "user",new string[] { "username","password"} );
            //DBMgr.Instance.InsertTable("user", "赵云", "12345678");

            // Init();
            // DBMgr.Instance.InsertTable_Injection("user", "马超", "12345678';delete from user;"); //sql注入

            // DBMgr.Instance.DeleteRow("user", 21);
            DBMgr.Instance.UpdateUser(new UserTest { id = 23, username = "张飞", password = "1234" });

            //DBMgr.Instance.QueryTable("user", new string[] { "username", "password" });

            Console.ReadLine();
        }


        /// <summary>
        /// 备用
        /// </summary>
        static void Init()
        {
            DBMgr.Instance.InsertTable("user", "刘备", "12345");
            DBMgr.Instance.InsertTable("user", "关羽", "123456");
            DBMgr.Instance.InsertTable("user", "张飞", "1234567");
            DBMgr.Instance.InsertTable("user", "赵云", "12345678");
        }
    }

     public  class DBMgr
    {
        #region 单例
        private static DBMgr _instance;
        public static DBMgr Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DBMgr();
                }
                return _instance;
            }

        }
        #endregion

        MySqlConnection conn;

        public void Init(string database)
        {
            Console.WriteLine("Init DBMgr");
            conn = new MySqlConnection("server=localhost;User Id=root;passwrod=;Database=" + database + ";Charset=utf8");
            if (conn.State != ConnectionState.Open)
            {
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("连不上" + ex);
                }
            }
        }


        #region 增删查改
        public int InsertTable(string table, string username, string password)
        {
            int id = -1;
            try
            {
                string sql = "insert into " + table + " set " +
                "username=@username,password=@password";

                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);

                cmd.ExecuteNonQuery();
                id = (int)cmd.LastInsertedId;
                Console.WriteLine("已增id:" + id);

            }
            catch (Exception e)
            {

            }
            finally
            {

            }
            return id;
        }


        #region 说明 演示sql注入 
        /// <summary>
        /// 演示sql注入
        /// </summary>
        /// <param name="table"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int InsertTable_Injection(string table, string username, string password)
        {
            int id = -1;
            try
            {
                //string sql = "insert into "+table+" set " +
                //"username=@username,password=@password";
                string sql = "insert into " + table + " set " +
                "username='" + username + "'" + " ,password='" + password;
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);

                cmd.ExecuteNonQuery();
                id = (int)cmd.LastInsertedId;
                Console.WriteLine("已增id:" + id);

            }
            catch (Exception e)
            {

            }
            finally
            {

            }
            return id;
        }

        #endregion


        public void QueryTable(string table, string[] colArr)
        {

            MySqlDataReader reader = null;

            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from " + table, conn);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < colArr.Length; i++)
                    {
                        Console.Write("{0}:{1}\t", colArr[i], reader.GetString(colArr[i]));
                    }
                    Console.WriteLine();

                }

                reader.Close();
            }
            catch (Exception e)
            {

            }
            finally
            {
                //写在这里而不是try，防止catch的内容太多

            }


        }


        public void DeleteRow(string table, int id)
        {

            MySqlDataReader reader = null;

            try
            {
                MySqlCommand cmd = new MySqlCommand("delete from " + table + " where id=@id", conn);



                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
                Console.WriteLine("已删id:" + id);

            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            finally
            {
                //写在这里而不是try，防止catch的内容太多

            }


        }




        public bool UpdateUser(UserTest user)
        {
            try
            {
                string sql = "update user set " +
                    " username = @username,password = @password" +
                    " where Id=@Id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("username", user.username);
                cmd.Parameters.AddWithValue("password", user.password);
                cmd.Parameters.AddWithValue("id", user.id);

                cmd.ExecuteNonQuery();

                Console.WriteLine("更新id成功：" + user.id);
            }
            catch (Exception e)
            {
                Console.WriteLine("查找id错误：" + user.id);
                return false;
            }
            finally
            {

            }
            return true;
        }

        #endregion



        /**  以前用过的代码，先放在这，方便复制粘贴
        #region 增删查改

        public int InsertPlayerData(string acct, string pass, PlayerData pd)
        {
            int id = -1;
            try
            {
                string sql = "insert into account set " +
                "acct=@acct,pass=@pass,name = @name,lv = @lv,exp = @exp,power = @power," +
                "coin = @coin,diamond = @diamond,crystal=@crystal," +
                "ad = @ad,ap = @ap,addef = @addef,apdef = @apdef,dodge = @dodge,critical = @critical,pierce = @pierce," +
                "guideid=@guideid,strong=@strong,time=@time,taskreward=@taskreward,instance=@instance";
                MySqlCommand cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("acct", acct);
                cmd.Parameters.AddWithValue("pass", pass);
                cmd.Parameters.AddWithValue("name", pd.name);
                cmd.Parameters.AddWithValue("lv", pd.lv);
                cmd.Parameters.AddWithValue("exp", pd.exp);
                cmd.Parameters.AddWithValue("power", pd.power);
                cmd.Parameters.AddWithValue("coin", pd.coin);
                cmd.Parameters.AddWithValue("diamond", pd.diamond);
                cmd.Parameters.AddWithValue("crystal", pd.crystal);
                cmd.Parameters.AddWithValue("hp", pd.hp);
                cmd.Parameters.AddWithValue("ad", pd.ad);
                cmd.Parameters.AddWithValue("ap", pd.ap);
                cmd.Parameters.AddWithValue("addef", pd.addef);
                cmd.Parameters.AddWithValue("apdef", pd.apdef);
                cmd.Parameters.AddWithValue("dodge", pd.dodge);
                cmd.Parameters.AddWithValue("critical", pd.critical);
                cmd.Parameters.AddWithValue("pierce", pd.pierce);
                cmd.Parameters.AddWithValue("guideid", pd.guideid);
                cmd.Parameters.AddWithValue("strong", Strong_ArrToString(pd.strongArr));
                cmd.Parameters.AddWithValue("time", pd.time);
                cmd.Parameters.AddWithValue("taskreward", TaskReward_ArrToString(pd.taskRewardArr));
                cmd.Parameters.AddWithValue("instance", pd.instance);
                cmd.ExecuteNonQuery();
                id = (int)cmd.LastInsertedId;
                Console.WriteLine("已增id:" + id);

            }
            catch (Exception e)
            {

                Console.WriteLine("Insert PlayerData失败，原因：" + e, LogType.Error);
            }
            finally
            {

            }
            return id;
        }


        /// <summary>
        /// 查账号
        /// </summary>
        /// <param name="acct"></param>
        /// <param name="pass"></param>
        /// <returns></returns>

        public PlayerData QueryPlayerData(string acct, string pass)
        {
            bool isNew = true;//新账号
            PlayerData playerData = null;
            MySqlDataReader reader = null;

            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from account where acct=@acct", conn);
                cmd.Parameters.AddWithValue("acct", acct);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string _acct = reader.GetString("acct");

                    if (_acct.Equals(acct))
                    {
                        isNew = false;

                        playerData = new PlayerData
                        {
                            id = reader.GetInt32("id"),
                            name = reader.GetString("name"),
                            lv = reader.GetInt32("lv"),
                            exp = reader.GetInt32("exp"),
                            power = reader.GetInt32("power"),
                            coin = reader.GetInt32("coin"),
                            diamond = reader.GetInt32("diamond"),
                            crystal = reader.GetInt32("crystal"),
                            hp = reader.GetInt32("hp"),
                            ad = reader.GetInt32("ad"),
                            ap = reader.GetInt32("ap"),
                            addef = reader.GetInt32("addef"),
                            apdef = reader.GetInt32("apdef"),
                            critical = reader.GetInt32("critical"),
                            pierce = reader.GetInt32("pierce"),
                            dodge = reader.GetInt32("dodge"),
                            guideid = reader.GetInt32("guideid"),
                            strongArr = Strong_StringToArr(reader.GetString("strong")),
                            time = reader.GetInt64("time"),
                            taskRewardArr = TaskReward_StringToArr(reader.GetString("taskreward")),
                            instance = reader.GetInt32("instance")

                        };
                        Console.WriteLine("已查到acct:" + acct);
                    }

                }

                reader.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Query PlayerData By Acct&Pass Error:" + e, LogType.Error);
            }
            finally
            {
                //写在这里而不是try，防止catch的内容太多
                if (isNew)
                {
                    playerData = new PlayerData
                    {
                        id = -1,
                        name = "",
                        lv = 1,
                        exp = 0,
                        power = 50,
                        coin = 5000,
                        diamond = 500,
                        crystal = 100,
                        hp = 2000,
                        ad = 275,
                        ap = 265,
                        addef = 67,
                        apdef = 43,
                        critical = 2,
                        pierce = 5,
                        dodge = 7,
                        guideid = 1001,
                        strongArr = new int[] { 0, 0, 0, 0, 0, 0 },
                        time = TimerSvc.Instance.GetNowTime(),
                        taskRewardArr = new string[] {
                        "1|0|2",
                        "2|0|2",
                        "3|0|2",
                        "4|0|2",
                        "5|0|2",
                        "6|0|2"
                    },
                        instance = 10001
                    };
                    playerData.id = InsertPlayerData(acct, pass, playerData);
                }

            }


            return playerData;
        }

        public bool QueryNameData(string name)
        {
            bool exist = false;
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from account where name=@name", conn);
                cmd.Parameters.AddWithValue("name", name);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    exist = true;

                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("查找name错误：" + e, LogType.Error);
            }
            finally
            {

            }
            return exist;
        }

        public bool UpdatePlayerData(int id, PlayerData pd)
        {
            try
            {
                string sql = "update account set " +
                    " name = @name,lv = @lv,exp = @exp,power = @power," +
                    "coin = @coin,diamond = @diamond,crystal=@crystal," +
                    " ad = @ad,ap = @ap,addef = @addef,apdef = @apdef,dodge = @dodge,critical = @critical,pierce = @pierce," +
                    " guideid=@guideid,strong=@strong,time=@time,taskreward=@taskreward, instance=@instance" +
                    " where id=@id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("name", pd.name);
                cmd.Parameters.AddWithValue("lv", pd.lv);
                cmd.Parameters.AddWithValue("exp", pd.exp);
                cmd.Parameters.AddWithValue("power", pd.power);
                cmd.Parameters.AddWithValue("coin", pd.coin);
                cmd.Parameters.AddWithValue("diamond", pd.diamond);
                cmd.Parameters.AddWithValue("crystal", pd.crystal);
                cmd.Parameters.AddWithValue("hp", pd.hp);
                cmd.Parameters.AddWithValue("ad", pd.ad);
                cmd.Parameters.AddWithValue("ap", pd.ap);
                cmd.Parameters.AddWithValue("addef", pd.addef);
                cmd.Parameters.AddWithValue("apdef", pd.apdef);
                cmd.Parameters.AddWithValue("critical", pd.critical);
                cmd.Parameters.AddWithValue("dodge", pd.dodge);
                cmd.Parameters.AddWithValue("pierce", pd.pierce);
                cmd.Parameters.AddWithValue("guideid", pd.guideid);
                cmd.Parameters.AddWithValue("strong", Strong_ArrToString(pd.strongArr));
                cmd.Parameters.AddWithValue("time", pd.time);
                cmd.Parameters.AddWithValue("taskreward", TaskReward_ArrToString(pd.taskRewardArr));
                cmd.Parameters.AddWithValue("instance", pd.instance);

                cmd.Parameters.AddWithValue("id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("查找id错误：" + e, LogType.Error);
                return false;
            }
            finally
            {

            }
            return true;
        }

        #region   强化

        string Strong_ArrToString(int[] strongArr)
        {

            string strong = "";
            if (strongArr == null) return strong;
            for (int i = 0; i < strongArr.Length; i++)
            {
                strong += "#" + strongArr[i].ToString();
            }

            return strong;
        }

        int[] Strong_StringToArr(string strong)
        {

            if (strong == null)
                return null;


            string[] _strongArr = strong.Split('#');//解析后第一个是""
            int len = _strongArr.Length;
            for (int i = 0; i < _strongArr.Length; i++)
            {
                if (_strongArr[i] == "")
                {
                    len--;
                }
            }
            //
            int[] strongArr = new int[len];
            int j = 0;
            for (int i = 0; i < _strongArr.Length; i++)
            {
                if (_strongArr[i] == "")
                {
                    j--;
                    continue;
                }
                j++;
                strongArr[j] = int.Parse(_strongArr[i]);
            }

            return strongArr;
        }

        #endregion

        #region 任务奖励
        string TaskReward_ArrToString(string[] taskRewardArr)
        {

            string taskReward = "";
            if (taskRewardArr == null)
                return "";

            for (int i = 0; i < taskRewardArr.Length; i++)
            {
                taskReward += "#" + taskRewardArr[i];
            }

            return taskReward;
        }

        string[] TaskReward_StringToArr(string taskReward)
        {

            if (taskReward == null)
                return null;


            string[] _taskRewardArr = taskReward.Split('#');//解析后第一个是""
            int len = _taskRewardArr.Length;
            for (int i = 0; i < _taskRewardArr.Length; i++)
            {
                if (_taskRewardArr[i] == "")
                {
                    len--;
                }
            }
            //
            string[] taskRewardArr = new string[len];
            int j = 0;
            for (int i = 0; i < _taskRewardArr.Length; i++)
            {
                if (_taskRewardArr[i] == "")
                {
                    j--;
                    continue;
                }
                j++;
                taskRewardArr[j] = _taskRewardArr[i];
            }

            return taskRewardArr;
        }
        #endregion

        #endregion

        //**/
    }
}
