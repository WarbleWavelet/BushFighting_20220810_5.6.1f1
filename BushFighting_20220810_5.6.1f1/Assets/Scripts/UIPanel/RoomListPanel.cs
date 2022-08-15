/****************************************************

	文件：
	作者：WWS
	日期：2022/8/15 15:33:29
	功能：大厅，房间列表

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Protocol;
public class RoomListPanel : BasePanel
{

  
    #region 字属构造
    private RectTransform battleRes;
    private RectTransform roomList;
    private VerticalLayoutGroup roomLayout;
    private GameObject roomItemPrefab;
    private ListRoomRequest listRoomRequest;
    private CreateRoomRequest createRoomRequest;
    private JoinRoomRequest joinRoomRequest;
    private UpdateResultRequest updateResultRequest;
    private List<UserData> userdataLst = null;

    private UserData ud1 = null;
    private UserData ud2 = null;
    #endregion



    #region 生命
    private void Start()
    {
        battleRes = transform.Find("BattleRes").GetComponent<RectTransform>();
        roomList = transform.Find("RoomList").GetComponent<RectTransform>();
        roomLayout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        roomItemPrefab = Resources.Load("UIPanel/RoomItem") as GameObject;

        AddBtnListener( transform.Find("RoomList/CloseButton"), OnCloseClick);
        AddBtnListener(transform.Find("RoomList/CreateRoomButton"), OnCreateRoomClick);
        AddBtnListener(transform.Find("RoomList/RefreshButton"), OnRefreshClick);

        //
        listRoomRequest = GetOrAddComponent<ListRoomRequest>(gameObject);
        createRoomRequest = GetOrAddComponent<CreateRoomRequest>(gameObject);
        joinRoomRequest = GetOrAddComponent<JoinRoomRequest>(gameObject);
        updateResultRequest = GetOrAddComponent<UpdateResultRequest>(gameObject);
        //
        EnterAnim();
    }

    private void Update()
    {
        if (userdataLst != null) //异步加载给的lst
        {
            ShowLobby(userdataLst);
            userdataLst = null;
        }

        if (ud1 != null && ud2 != null)//满员，准备战斗
        {
            BasePanel panel = uiMgr.PushPanel(UIPanelType.Room);
            (panel as RoomPanel).SetAllPlayerResAsync(ud1, ud2);
            ud1 = null; 
            ud2 = null;
        }
        //if (Input.GetMouseButtonDown(0)) //测试
        //{
        //    SetRoomItem(new UserData(11,"周仓",5,7));
        //    AdaptRoomListLength();
        //}

    }
    public override void OnEnter()
    {
        SetBattleRes();
        if (battleRes != null)
        { 
            EnterAnim();
        }
           
        listRoomRequest = GetOrAddComponent<ListRoomRequest>(gameObject);

        listRoomRequest.SendRequest();
    }

    public override void OnExit()
    {
        HideAnim();
    }

    public override void OnPause()
    {
        HideAnim();
    }


    /// <summary>
    /// 重新回来（房间=>大厅）
    /// </summary>
    public override void OnResume()
    {
        EnterAnim();
        listRoomRequest.SendRequest();
    }



    #endregion


    #region Click
   private void OnCloseClick()
    {
        PlayClickSound();
        uiMgr.PopPanel();
    }

    /// <summary>
    /// 创建房间
    /// </summary>
    private void OnCreateRoomClick()
    {
        BasePanel panel= uiMgr.PushPanel(UIPanelType.Room);
        createRoomRequest.SetPanel(panel);
        createRoomRequest.SendRequest();
    }

    /// <summary>
    /// 刷新
    /// </summary>
    private void OnRefreshClick()
    {
        listRoomRequest.SendRequest();
    }


    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="id"></param>
    public void OnJoinClick(int id)
    {
        joinRoomRequest.SendRequest(id);
    }
    #endregion

 

    #region Anim
    private void EnterAnim()
    {
        gameObject.SetActive(true);

        battleRes.localPosition = new Vector3(-1000, 0);
        battleRes.DOLocalMoveX(-290, 0.5f);

        roomList.localPosition = new Vector3(1000, 0);
        roomList.DOLocalMoveX(171, 0.5f);
    }
    private void HideAnim()
    {
        battleRes.DOLocalMoveX(-1000, 0.5f);

        roomList.DOLocalMoveX(1000, 0.5f).OnComplete(() => gameObject.SetActive(false));
    }
    #endregion


    /// <summary>
    /// 
    /// </summary>
    private void SetBattleRes()
    {
        UserData ud = facade.GetUserData();
        SetText(transform.Find("BattleRes/Username"), ud.Username);
        SetText(transform.Find("BattleRes/TotalCount"), "总场数:" + ud.TotalCount.ToString());
        SetText(transform.Find("BattleRes/WinCount"), "胜利:" + ud.WinCount.ToString());
    }



     /// <summary>
     /// 接受quest
     /// </summary>
     /// <param name="udList"></param>
    public void LoadLobbyAsync(List<UserData> udList) //Update中会更新
    {
        this.userdataLst = udList;
    }

     /// <summary>
     /// 显示大厅
     /// </summary>
     /// <param name="udList"></param>
    private void ShowLobby( List<UserData> udList )
    {
        RoomItem[] riArray= roomLayout.GetComponentsInChildren<RoomItem>();
        foreach(RoomItem ri in riArray) //清空
        {
            ri.DestroySelf();
        }

        int count = udList.Count;
        for (int i = 0; i < count; i++)
        {
            ShowRoomItem(udList[i]);
        }
        AdaptRoomListLength();
    }

    /// <summary>
    /// 显示大厅中的房间项
    /// </summary>
    /// <param name="ud"></param>
    void ShowRoomItem(UserData ud)
    {
        GameObject roomItem = GameObject.Instantiate(roomItemPrefab);
        roomItem.transform.SetParent(roomLayout.transform);
        roomItem.GetComponent<RoomItem>().SetRoomInfo(ud.Id, ud.Username, ud.TotalCount, ud.WinCount, this);
    }


    void AdaptRoomListLength()
    {
        int roomCount = GetComponentsInChildren<RoomItem>().Length;
        Vector2 size = roomLayout.GetComponent<RectTransform>().sizeDelta;
        roomLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x,
            roomCount * (roomItemPrefab.GetComponent<RectTransform>().sizeDelta.y + roomLayout.spacing));
    }

    #region Response
   public void OnUpdateResultResponse(int totalCount, int winCount)
    {
        facade.UpdateResult(totalCount, winCount);
        SetBattleRes();
    }


    /// <summary>
    /// 点击进入房间的回调 
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="ud1"></param>
    /// <param name="ud2"></param>
    public void OnJoinResponse( ReturnCode returnCode,UserData ud1,UserData ud2)
    {
        switch (returnCode)
        {
            case ReturnCode.NotFound:
                uiMgr.ShowMsgSync("房间被销毁无法加入");
                break;
            case ReturnCode.Fail:
                uiMgr.ShowMsgSync("房间已满，无法加入");
                break;
            case ReturnCode.Success:
                uiMgr.ShowMsgSync("加入房间");
                this.ud1 = ud1;
                this.ud2 = ud2;
                break;
        }
    }
    #endregion
 

}
