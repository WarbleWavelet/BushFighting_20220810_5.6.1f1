/****************************************************
    文件：StartPanel.cs
	作者：
    邮箱: 
    日期：2022年8月12日
	功能：
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Protocol;
public class RegisterPanel : BasePanel //注册 
{

    private InputField usernameIF;
    private InputField passwordIF;
    private InputField rePasswordIF;
    private RegisterRequest registerRequest;



    #region 生命
   private void Start()
    {
        registerRequest = GetComponent<RegisterRequest>();

        usernameIF = transform.Find("UsernameLabel/UsernameInput").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswordInput").GetComponent<InputField>();
        rePasswordIF = transform.Find("RePasswordLabel/RePasswordInput").GetComponent<InputField>();

        transform.Find("RegisterButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);
        transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);

        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.2f);
        transform.localPosition = new Vector3(1000, 0, 0);
        transform.DOLocalMove(Vector3.zero, 0.2f);
    }

  
    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }
    #endregion  
 


    #region Click
  private void OnRegisterClick()
    {
        PlayClickSound();
        string msg = "";

        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "用户名不能为空";
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "\n密码不能为空";
        }
        if ( passwordIF.text!=rePasswordIF.text )
        {
            msg += "\n密码不一致";
        }
        if (msg != "")
        {
            uiMgr.ShowMgr(msg);
            return;
        }
        
        registerRequest.SendRequest(usernameIF.text, passwordIF.text); //进行注册 发送到服务器端
    }

    private void OnCloseClick()
    {
        PlayClickSound();
        transform.DOScale(0, 0.4f);
        Tweener tweener = transform.DOLocalMove(new Vector3(1000, 0, 0), 0.4f);
        tweener.OnComplete(() =>
        {
            uiMgr.PopPanel();
            uiMgr.SetIFText(usernameIF.text, passwordIF.text);//将信息填入登录页的输入框

        });//设置Login);         
    }



    #endregion

    public void OnRegisterResponse(ReturnCode returnCode)
    {       
        if (returnCode == ReturnCode.Success)
        {
            uiMgr.ShowMsgSync("注册成功");
           
        }
        else
        {
            uiMgr.ShowMsgSync("用户名重复");
        }
    }

    public string GetIFText( )
    {
        return usernameIF.text + "," + passwordIF.text;
    }   
}
