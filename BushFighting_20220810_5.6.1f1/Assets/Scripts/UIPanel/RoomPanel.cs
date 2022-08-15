using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Protocol;

public class RoomPanel : BasePanel
{

    #region 字属
    private Text localPlayerUsername;
    private Text localPlayerTotalCount;
    private Text localPlayerWinCount;

    private Text enemyPlayerUsername;
    private Text enemyPlayerTotalCount;
    private Text enemyPlayerWinCount;

    private Transform bluePanel;
    private Transform redPanel;
    private Transform startButton;
    private Transform exitButton;

    private UserData ud = null;
    private UserData udHome = null;
    private UserData udAway = null;

    private QuitRoomRequest quitRoomRequest;
    private StartGameRequest startGameRequest;

    private bool isPopPanel = false;
    #endregion  
   

    #region 生命
    private void Start()
    {
        localPlayerUsername = transform.Find("BluePanel/Username").GetComponent<Text>();
        localPlayerTotalCount = transform.Find("BluePanel/TotalCount").GetComponent<Text>();
        localPlayerWinCount = transform.Find("BluePanel/WinCount").GetComponent<Text>();
        enemyPlayerUsername = transform.Find("RedPanel/Username").GetComponent<Text>();
        enemyPlayerTotalCount = transform.Find("RedPanel/TotalCount").GetComponent<Text>();
        enemyPlayerWinCount = transform.Find("RedPanel/WinCount").GetComponent<Text>();

        bluePanel = transform.Find("BluePanel");
        redPanel = transform.Find("RedPanel");
        startButton = transform.Find("StartButton");
        exitButton = transform.Find("ExitButton");


        AddBtnListener( transform.Find("StartButton") , OnStartClick );
        AddBtnListener( transform.Find("ExitButton") , OnExitClick );

         //
        quitRoomRequest = GetOrAddComponent<QuitRoomRequest>(gameObject);
        GetOrAddComponent<UpdateRoomRequest>(gameObject);
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

            else
            {
                ClearAwayPlayerRes();
            }

            udHome = null;
            udAway = null;
        }
        if (isPopPanel)
        {                                                                        
            uiMgr.PopPanel();
            isPopPanel = false;
        }
    }
    public override void OnEnter()
    {
        if (bluePanel != null)
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
        localPlayerUsername.text = ud.Username;
        localPlayerTotalCount.text = "总场数：" + ud.TotalCount;
        localPlayerWinCount.text = "胜利：" + ud.WinCount;
    }

    /// <summary>
    /// 客场角色
    /// </summary>
    /// <param name="ud"></param>
    void SetAwayPlayerRes(UserData ud)
    {
        enemyPlayerUsername.text = ud.Username;
        enemyPlayerTotalCount.text = "总场数：" + ud.TotalCount;
        enemyPlayerWinCount.text = "胜利：" + ud.WinCount;
    }

    /// <summary>
    /// 清除客人
    /// </summary>
    public void ClearAwayPlayerRes()
    {
        enemyPlayerUsername.text = "";
        enemyPlayerTotalCount.text = "等待玩家加入....";
        enemyPlayerWinCount.text = "";
    }
    #endregion  
   



    #region Click
  private void OnStartClick()
    {
        startGameRequest.SendRequest();
    }

    private void OnExitClick()
    {
        quitRoomRequest.SendRequest();
    }
    #endregion

  

    public void OnExitResponse()
    {
        isPopPanel = true;
    }
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

    private void EnterAnim()
    {
        gameObject.SetActive(true);
        bluePanel.localPosition = new Vector3(-1000, 0, 0);
        bluePanel.DOLocalMoveX(-174, 0.4f);
        redPanel.localPosition = new Vector3(1000, 0, 0);
        redPanel.DOLocalMoveX(174, 0.4f);
        startButton.localScale = Vector3.zero;
        startButton.DOScale(1, 0.4f);
        exitButton.localScale = Vector3.zero;
        exitButton.DOScale(1, 0.4f);
    }
    private void ExitAnim()
    {
        bluePanel.DOLocalMoveX(-1000, 0.4f);
        redPanel.DOLocalMoveX(1000, 0.4f);
        startButton.DOScale(0, 0.4f);
        exitButton.DOScale(0, 0.4f).OnComplete(() => gameObject.SetActive(false));
    }
}
