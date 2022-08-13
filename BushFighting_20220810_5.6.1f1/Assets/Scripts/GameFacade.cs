using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using System;

public class GameFacade : MonoBehaviour //大佬
{


    #region 字属
  #region 单例
    private static GameFacade _instance;
    public static GameFacade Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GameFacade").GetComponent<GameFacade>();
            }
            return _instance;
        }
    }
    #endregion


    private UIMgr uiMgr;
    private AudioMgr audioMgr;
    private PlayerMgr playerMgr;
    private CameraMgr cameraMgr;
    private RequestMgr requestMgr;
    private ClientMgr clientMgr;

    private bool isEnterPlaying = false;

    internal void SetIFText(string v)
    {                  
        string[] strArr=v.Split(',');
        string username = strArr[0];
        string password = strArr[1];


       
        uiMgr.SetIFText(username, password);
    }

    internal void OpenPanel(UIPanelType login)
    {
        uiMgr.PushPanel(UIPanelType.Login);
    }

    internal void ClosePanel(UIPanelType login)
    {
    }
    #endregion




    #region 生命
    //private void Awake()
    //{
    //    if (_instance != null)
    //    {
    //        Destroy(this.gameObject);return;
    //    }
    //    _instance = this;
    //}


    void Start ()
    {
        InitManager();
	}
	
	void Update () 
    {
        UpdateManager();
        if (isEnterPlaying)
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


    public void AddRequest(ActionCode actionCode, BaseRequest request)
    {
        requestMgr.AddRequest(actionCode, request);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestMgr.RemoveRequest(actionCode);
    }
    public void HandleReponse(ActionCode actionCode, string data)
    {
        requestMgr.HandleReponse(actionCode, data);
    }

    public void SendRequest(ReqCode ReqCode, ActionCode actionCode, string data)
    {
        clientMgr.SendRequest(ReqCode, actionCode, data);
    }

    public void ShowMessage(string msg)
    {
        uiMgr.ShowMgr(msg);
    }

    public void PlayBgSound(string soundName)
    {
        audioMgr.PlayBGMAudio(soundName);
    }
    public void PlayNormalSound(string soundName)
    {
        audioMgr.PlayUIAudio(soundName);
    }

    public void SetUserData(UserData ud)
    {
        playerMgr.UserData = ud;
    }
    public UserData GetUserData()
    {
        return playerMgr.UserData;
    }
    public void SetCurrentRoleType(RoleType rt)
    {
        playerMgr.SetCurrentRoleType(rt);
    }
    public GameObject GetCurrentRoleGameObject()
    {
        return playerMgr.GetCurrentRoleGameObject();
    }


    #region 玩家
  public void EnterPlayingSync()
    {
        isEnterPlaying = true;
    }
    private void EnterPlaying()
    {
        playerMgr.SpawnRoles();
        cameraMgr.FollowRole();
    }
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
