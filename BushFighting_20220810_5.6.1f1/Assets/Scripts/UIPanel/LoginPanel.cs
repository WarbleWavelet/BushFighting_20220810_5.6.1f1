/****************************************************
    文件：StartPanel.cs
	作者：
    邮箱: 
    日期：2022年8月12日
	功能：登录
*****************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Protocol;            

public class LoginPanel : BasePanel 
{

    private Button closeButton;
    private InputField usernameIF;
    private InputField passwordIF;
    private LoginRequest loginRequest;
    //private Button loginButton;
    //private Button registerButton;


    #region 生命
  private void Start()
    {
        loginRequest = GetComponent<LoginRequest>();
        usernameIF = transform.Find("UsernameLabel/UsernameInput").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
        closeButton = transform.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(OnCloseClick);
        transform.Find("LoginButton").GetComponent<Button>().onClick.AddListener(OnLoginClick);
        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
    }


      


    public override void OnEnter()
    {
        base.OnEnter();
        uiMgr.InjectLoginPanel(this);
        EnterAnimation();
    }

    public override void OnPause()
    {
        HideAnimation();
    }


    public override void OnResume()
    {
        EnterAnimation();
    }

    public override void OnExit()
    {
        HideAnimation();
    }
    #endregion
  




    #region Click
    /// <summary>
    /// 关闭注册窗口
    /// </summary>
    private void OnCloseClick()
    {
        PlayClickSound();
        uiMgr.PopPanel();
    }


 /// <summary>
    /// 登录
    /// </summary>
    private void OnLoginClick()
    {
        PlayClickSound();
        string msg = "";
        if(string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "用户名不能为空 ";
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "密码不能为空 ";
        }
        if (msg != "")
        {
            uiMgr.ShowMgr(msg);
            return;
        }
        loginRequest.SendRequest(usernameIF.text, passwordIF.text);
    }



    /// <summary>
    /// 点击注册
    /// </summary>
    private void OnRegisterClick()
    {
        PlayClickSound();
        uiMgr.PushPanel(UIPanelType.Register);
    }


    #endregion
    /// <summary>
    /// 注册后自动设置用户名，密码
    /// </summary>
    public void SetIF(string username, string password)
    {
        usernameIF.text = username;
        passwordIF.text = password;
    }
   

    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMgr.ShowMsgSync("登陆成功！");

            uiMgr.PushPanelSync(UIPanelType.RoomList);
        }
        else
        {
            uiMgr.ShowMsgSync("用户名或密码错误，无法登录，请重新输入!!");
        }
    }



    #region Animation
    /// <summary>
    /// 开窗动画
    /// </summary>
    private void EnterAnimation()
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.2f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.2f);
    }

    /// <summary>
    /// 关窗动画
    /// </summary>
    private void HideAnimation()
    {
        transform.DOScale(0, 0.3f);
        transform.DOLocalMoveX(1000, 0.3f).OnComplete(() => gameObject.SetActive(false));
    }
    #endregion


}
