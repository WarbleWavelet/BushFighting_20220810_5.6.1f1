/****************************************************

	文件：
	作者：WWS
	日期：2022/08/15 20:55:51
	功能：房间UI

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Protocol;

public class RoomPanel : BasePanel
{

    
    #region 字属
    private Text homePlayerUsername;
    private Text homePlayerTotalCount;
    private Text homePlayerWinCount;

    private Text awayPlayerUsername;
    private Text awayPlayerTotalCount;
    private Text awayPlayerWinCount;

    private Transform homePanel;//房主的UI面板
    private Transform awayPanel;//对手UI
    private Transform startButton;
    private Transform exitButton;

    private UserData ud = null;
    private UserData udHome = null;
    private UserData udAway = null;

    private QuitRoomRequest quitRoomRequest;
    private StartGameRequest startGameRequest;
    private UpdateRoomRequest updateRoomRequest;

    private bool isPopPanel = false; //异步改这个值，主线程中收起UI
    #endregion


    #region 生命
    private void Start()
    {
        homePlayerUsername = transform.Find("BluePanel/Username").GetComponent<Text>();
        homePlayerTotalCount = transform.Find("BluePanel/TotalCount").GetComponent<Text>();
        homePlayerWinCount = transform.Find("BluePanel/WinCount").GetComponent<Text>();
        awayPlayerUsername = transform.Find("RedPanel/Username").GetComponent<Text>();
        awayPlayerTotalCount = transform.Find("RedPanel/TotalCount").GetComponent<Text>();
        awayPlayerWinCount = transform.Find("RedPanel/WinCount").GetComponent<Text>();

        homePanel = transform.Find("BluePanel");
        awayPanel = transform.Find("RedPanel");
        startButton = transform.Find("StartButton");
        exitButton = transform.Find("ExitButton");


        AddBtnListener( transform.Find("StartButton") , OnStartClick );
        AddBtnListener( transform.Find("ExitButton") , OnExitClick );

         //
        quitRoomRequest = GetOrAddComponent<QuitRoomRequest>(gameObject);
        updateRoomRequest = GetOrAddComponent<UpdateRoomRequest>(gameObject);
        startGameRequest = GetOrAddComponent<StartGameRequest>(gameObject);

        EnterAnim();
    }    
    
    
    private void Update()
    {
        if (ud != null)//设置玩家信息
        {
            SetRoleRes(RoleType.Home, ud);
            ClearAwayPlayerRes();
            ud = null;
        }
        if (udHome != null) //房主
        {
            SetRoleRes(RoleType.Home, udHome);
            if (udAway != null)
            {
                SetRoleRes(RoleType.Away, udAway);
            }
            else //对手没了
            {
                ClearAwayPlayerRes();
            }

            udHome = null;
            udAway = null;
        }
        if (isPopPanel) //主线程中收起UI
        {                                                                        
            uiMgr.PopPanel();
            isPopPanel = false;
        }
    }
    public override void OnEnter()
    {
        if (homePanel != null)
        { 
            EnterAnim();
        }
            
    }

    public override void OnPause()
    {
        ExitAnim();
    }

    public override void OnExit()
    {
        ExitAnim();
    }

    public override void OnResume()
    {
        EnterAnim();
    }
    #endregion  
  

  



    #region 异步
    public void SetLocalPlayerResAsync()
    {
        ud = facade.GetUserData();
    }

    public void SetAllPlayerResAsync(UserData ud1,UserData ud2)
    {
        this.udHome = ud1;
        this.udAway = ud2;
    }
    #endregion


    #region SetRoleRes
     /// <summary>
    /// 根据RoleType去设置对应RoleType的UI
    /// </summary>
    /// <param name="roleType"></param>
    /// <param name="username"></param>
    /// <param name="totalCount"></param>
    /// <param name="winCount"></param>
    public void SetRoleRes(RoleType roleType, UserData ud)
    { 
        switch (roleType)
        {
            case RoleType.Home:
                {
                    SetHomePlayerRes( ud);
                }
                break;            
            case RoleType.Away:
                {
                    SetAwayPlayerRes( ud);
                }
                break;
            default: break;
        }
    }

    /// <summary>
    /// 主场角色
    /// </summary>
    /// <param name="ud"></param>
    void SetHomePlayerRes( UserData ud)
    {
        homePlayerUsername.text = ud.Username;
        homePlayerTotalCount.text = "总场数：" + ud.TotalCount;
        homePlayerWinCount.text = "胜利：" + ud.WinCount;
    }

    /// <summary>
    /// 客场角色
    /// </summary>
    /// <param name="ud"></param>
    void SetAwayPlayerRes(UserData ud)
    {
        awayPlayerUsername.text = ud.Username;
        awayPlayerTotalCount.text = "总场数：" + ud.TotalCount;
        awayPlayerWinCount.text = "胜利：" + ud.WinCount;
    }

    /// <summary>
    /// 清除客人
    /// </summary>
    public void ClearAwayPlayerRes()
    {
        awayPlayerUsername.text = "";
        awayPlayerTotalCount.text = "等待玩家加入....";
        awayPlayerWinCount.text = "";
    }
    #endregion




    #region Click与回复
    /// <summary>
    /// 点击开始游戏
    /// </summary>
    private void OnStartClick()
    {
        startGameRequest.SendRequest();
    }


    /// <summary>
    /// 开始游戏回复
    /// </summary>
    /// <param name="returnCode"></param>
    public void OnStartResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Fail)
        {
            uiMgr.ShowMsgSync("您不是房主，无法开始游戏！！");
        }
        else
        {
            uiMgr.PushPanelSync(UIPanelType.Game);
            facade.EnterPlayingSync();
        }
    }



    private void OnExitClick()
    {
        quitRoomRequest.SendRequest();
    }

    public void OnExitResponse()
    {
        isPopPanel = true;
    }
    #endregion

  






    private void EnterAnim()
    {
        gameObject.SetActive(true);
        homePanel.localPosition = new Vector3(-1000, 0, 0);
        homePanel.DOLocalMoveX(-174, 0.4f);
        awayPanel.localPosition = new Vector3(1000, 0, 0);
        awayPanel.DOLocalMoveX(174, 0.4f);
        startButton.localScale = Vector3.zero;
        startButton.DOScale(1, 0.4f);
        exitButton.localScale = Vector3.zero;
        exitButton.DOScale(1, 0.4f);
    }
    private void ExitAnim()
    {
        homePanel.DOLocalMoveX(-1000, 0.4f);
        awayPanel.DOLocalMoveX(1000, 0.4f);
        startButton.DOScale(0, 0.4f);
        exitButton.DOScale(0, 0.4f).OnComplete(() => gameObject.SetActive(false));
    }
}
