/****************************************************

   文件：
   作者：WWS
   日期：2022/8/15 13:20:34
   功能：大佬

*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using System;

public class GameFacade : MonoBehaviour 
{


    #region 字属
    #region 单例
    private static GameFacade _instance;
    public static GameFacade Instance
    {
        get//facade释放，有的client还没释放，会报null
        {
            GameObject go = GameObject.Find("GameFacade");
            if (go == null)
            {
                return null;
            }

            _instance = GameObject.Find("GameFacade").GetComponent<GameFacade>();

            return _instance;
        }
    }
    #endregion

    public bool test;
    private UIMgr uiMgr;
    private AudioMgr audioMgr;
    private PlayerMgr playerMgr;
    private CameraMgr cameraMgr;
    private RequestMgr requestMgr;
    private ClientMgr clientMgr;

    private bool isEnterPlaying = false; //从开始游戏进入开始战斗的标志




    #endregion


    internal void SetIFText(string v)
    {                  
        string[] strArr=v.Split(',');
        string username = strArr[0];
        string password = strArr[1];


       
        uiMgr.SetIFText(username, password);
    }

    #region 生命
    void Awake()
    {
        //Screen.SetResolution(1280,800,false);//设置分辨率
    }

    void Start ()
    {
        InitManager();
	}
	
	void Update () 
    {
        UpdateManager();
        if (isEnterPlaying)//从开始游戏进入开始战斗的计时
        {
            EnterPlaying();
            isEnterPlaying = false;
        }
	}
    
    private void OnDestroy()
    {
        DestroyManager();
    }
    #endregion



    #region Mgr 生命
  private void InitManager()
    {
        uiMgr = new UIMgr(this);
        audioMgr = new AudioMgr(this);
        playerMgr = new PlayerMgr(this);
        cameraMgr = new CameraMgr(this);
        requestMgr = new RequestMgr(this);
        clientMgr = new ClientMgr(this);

        uiMgr.OnInit();
        audioMgr.OnInit();
        playerMgr.OnInit();
        cameraMgr.OnInit();
        requestMgr.OnInit();
        clientMgr.OnInit();
    }
    private void UpdateManager()
    {
        uiMgr.Update();
        audioMgr.Update();
        playerMgr.Update();
        cameraMgr.Update();
        requestMgr.Update();
        clientMgr.Update();
    }

    private void DestroyManager()
    {
        uiMgr.OnDestroy();
        audioMgr.OnDestroy();
        playerMgr.OnDestroy();
        cameraMgr.OnDestroy();
        requestMgr.OnDestroy();
        clientMgr.OnDestroy();
    }
    #endregion




    #region req和rsp
    /// <summary>
    /// 增加请求
    /// </summary>
    /// <param name="actionCode"></param>
    /// <param name="request"></param>
    public void AddRequest(ActionCode actionCode, BaseRequest request)
    {
        requestMgr.AddRequest(actionCode, request);
    }


    /// <summary>
    /// 移除请求
    /// </summary>
    /// <param name="actionCode"></param>
    public void RemoveRequest(ActionCode actionCode)
    {
        requestMgr.RemoveRequest(actionCode);
    }
    /// <summary>
    /// 发送请求
    /// </summary>
    /// <param name="ReqCode"></param>
    /// <param name="actionCode"></param>
    /// <param name="data"></param>
    public void SendRequest(ReqCode ReqCode, ActionCode actionCode, string data)
    {
        clientMgr.SendRequest(ReqCode, actionCode, data);
    }

    /// <summary>
    /// 处理回复
    /// </summary>
    /// <param name="actionCode"></param>
    /// <param name="data"></param>
    public void HandleReponse(ActionCode actionCode, string data)
    {
        requestMgr.HandleReponse(actionCode, data);
    }


    #endregion

  

    public void ShowMessage(string msg)
    {
        uiMgr.ShowMsg(msg);
    }



    #region Sound
     public void PlayBgmMusic(string soundName)
    {
        audioMgr.PlayBGMusic(soundName);
    }
    public void PlayUIAudio(string soundName = DefinePath.Sound_Alert)
    {
        audioMgr.PlayUIAudio(soundName);
    }
    #endregion


    #region UserData
     public void SetUserData(UserData ud)
    {
        playerMgr.UserData = ud;
    }
    public UserData GetUserData()
    {
        return playerMgr.UserData;
    }
    #endregion  
   
    public void SetCurrentRoleType(RoleType rt)
    {
        playerMgr.SetCurrentRoleType(rt);
    }
    public GameObject GetCurrentRoleGameObject()
    {
        return playerMgr.GetCurRoleGameObject();
    }


    #region 玩家
    /// <summary>
    ///  从开始游戏进入开始战斗的任务转移，跑在主线程的Update
    /// </summary>
    public void EnterPlayingSync()
    {
        isEnterPlaying = true;
    }

    /// <summary>
    /// 从开始游戏进入开始战斗的过程 （倒计时中）
    /// </summary>
    private void EnterPlaying()
    {
        playerMgr.SpawnRoles();
        cameraMgr.FollowRole();
    }


    /// <summary>开始战斗</summary> 
    public void StartPlaying()
    {
        playerMgr.AddControlScript();
        playerMgr.CreateSyncRequest();
    }
    public void SendAttack(int damage)
    {
        playerMgr.SendAttack(damage);
    }
    #endregion  
  

    public void GameOver()
    {
        cameraMgr.WalkthroughScene();
        playerMgr.GameOver();
    }
    public void UpdateResult(int totalCount, int winCount)
    {
        playerMgr.UpdateResult(totalCount, winCount);
    }


}
