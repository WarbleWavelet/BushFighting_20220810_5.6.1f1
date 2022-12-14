using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIMgr: BaseManager
{

    #region 单例 字属 构造

    #region 单例
    /// 
    /// 单例模式的核心
    /// 1，定义一个静态的对象 在外界访问 在内部构造
    /// 2，构造方法私有化

    //private static UIManager _instance;

    //public static UIManager Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = new UIManager();
    //        }
    //        return _instance;
    //    }
    //}

 

    #endregion
   private Transform canvasTransform;
    private Dictionary<UIPanelType, string> panelPathDict;//存储所有面板Prefab的路径
    private Dictionary<UIPanelType, BasePanel> panelDict;//保存所有实例化面板的游戏物体身上的BasePanel组件
    private Stack<BasePanel> panelStack;
    private MessagePanel msgPanel;
    private LoginPanel loginPanel;
    private UIPanelType panelTypeToPush = UIPanelType.None;
    private Transform CanvasTransform
    {
        get
        {
            if (canvasTransform == null)
            {
                canvasTransform = GameObject.Find("Canvas").transform;
            }
            return canvasTransform;
        }
    }



    public UIMgr(GameFacade facade) : base(facade)
    {
        Json2Dic_UIPanelType();
    }
    #endregion


    public override void OnInit()
    {
        base.OnInit();
        PushPanel(UIPanelType.Message);
        PushPanel(UIPanelType.Start);
    }



    public override void Update()
    {
        if (panelTypeToPush != UIPanelType.None) //要进行Panel切换
        {
            PushPanel(panelTypeToPush);
            panelTypeToPush = UIPanelType.None;
        }
    }


    #region Panel
  public void PushPanelSync(UIPanelType panelType)
    {
        panelTypeToPush = panelType;
    }


    /// <summary>
    /// 把某个页面入栈，  把某个页面显示在界面上
    /// </summary>
    public BasePanel PushPanel(UIPanelType panelType)
    {
        if (panelStack == null)
        { 
             panelStack = new Stack<BasePanel>();
        }
           

        //判断一下栈里面是否有页面
        if (panelStack.Count > 0)
        {
            BasePanel topPanel = panelStack.Peek();
            topPanel.OnPause();
        }

        BasePanel panel = GetPanel(panelType);
        panel.OnEnter();
        panelStack.Push(panel);
        return panel;
    }



    /// <summary>
    /// 出栈 ，把页面从界面上移除
    /// </summary>
    public void PopPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count <= 0) return;

        //关闭栈顶页面的显示
        BasePanel topPanel = panelStack.Pop();
        topPanel.OnExit();

        if (panelStack.Count <= 0) return;
        BasePanel topPanel2 = panelStack.Peek();
        topPanel2.OnResume();

    }



    /// <summary>
    /// 根据面板类型 得到实例化的面板
    /// </summary>
    /// <returns></returns>
    private BasePanel GetPanel(UIPanelType panelType)
    {
        if (panelDict == null)
        {
            panelDict = new Dictionary<UIPanelType, BasePanel>();
        }

        //BasePanel panel;
        //panelDict.TryGetValue(panelType, out panel);//TODO

        BasePanel panel = panelDict.TryGet(panelType);

        if (panel == null)
        {
            //如果找不到，那么就找这个面板的prefab的路径，然后去根据prefab去实例化面板
            //string path;
            //panelPathDict.TryGetValue(panelType, out path);
            string path = panelPathDict.TryGet(panelType);
            GameObject instPanel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
            panel = instPanel.GetComponent<BasePanel>();

            instPanel.transform.SetParent(CanvasTransform,false);
            panel.UIMng = this;
            panel.Facade = facade;
            panelDict.Add(panelType, panel);
            return panel;
        }
        else
        {
            return panel;
        }

    }
    #endregion
  

    [Serializable]
    class UIPanelTypeJson
    {
        public List<UIPanelInfo> infoList;
    }


    /// <summary>
    /// 解析Json，
    /// </summary>
    private void Json2Dic_UIPanelType(string jsonPath = "UIPanelType")
    {
        panelPathDict = new Dictionary<UIPanelType, string>();

        TextAsset ta = Resources.Load<TextAsset>(jsonPath);

        UIPanelTypeJson jsonObject = JsonUtility.FromJson<UIPanelTypeJson>(ta.text);

        foreach (UIPanelInfo info in jsonObject.infoList) 
        {
            panelPathDict.Add(info.panelType, info.path);
        }
    }



    #region MsgPanel
  public void InjectMsgPanel(MessagePanel msgPanel)
    {
        this.msgPanel = msgPanel;
    }

    public void InjectLoginPanel(LoginPanel loginPanel)
    {
        if (this.loginPanel == null)
        { 
              this.loginPanel = loginPanel;
        }
      
    }

    public void SetIFText(string username, string password)
    {
        if (loginPanel != null)
        { 
              loginPanel.SetIF(username, password);
        }
    
    }


    public void ShowMsg(string msg)
    {
        if (msgPanel == null)
        {
            Debug.Log("无法显示提示信息，MsgPanel为空");return;
        }
        msgPanel.ShowMsg(msg);
    }
    public void ShowMsgSync(string msg)
    {
        if (msgPanel == null)
        {
            Debug.Log("无法显示提示信息，MsgPanel为空"); return;
        }
        msgPanel.ShowMgrSync(msg);
    }
    #endregion
  
    /// <summary>
    /// just for test
    /// </summary>
    //public void Test()
    //{
    //    string path ;
    //    panelPathDict.TryGetValue(UIPanelType.Knapsack,out path);
    //    Debug.Log(path);
    //}
}
