/****************************************************

	文件：
	作者：WWS
	日期：2022/08/18 11:19:03
	功能：用户信息

*****************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class UserData
{
    #region 字属 构造
    public int Id { get; private set; }
    public string Username { get; private set; }
    public int TotalCount { get; set; }
    public int WinCount { get; set; }

    public UserData(string userData)
    {
        string[] strs = userData.Split(',');
        this.Id = int.Parse(strs[0]);
        this.Username = strs[1];
        this.TotalCount = int.Parse(strs[2]);
        this.WinCount = int.Parse(strs[3]);
    }
    public UserData(string username, int totalCount, int winCount)
    {
        this.Username = username;
        this.TotalCount = totalCount;
        this.WinCount = winCount;
    }
    /// <summary>
    /// 创建房间多用
    /// </summary>
    /// <param name="id"></param>
    /// <param name="username"></param>
    /// <param name="totalCount"></param>
    /// <param name="winCount"></param>
    public UserData(int id,string username, int totalCount, int winCount)
    {
        this.Id = id;
        this.Username = username;
        this.TotalCount = totalCount;
        this.WinCount = winCount;
    }
    #endregion
  

}
