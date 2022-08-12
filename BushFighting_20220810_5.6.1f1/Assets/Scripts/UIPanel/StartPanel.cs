/****************************************************
    文件：StartPanel.cs
	作者：
    邮箱: 
    日期：2022年8月12日
	功能： 游戏最开始
*****************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class StartPanel : BasePanel 
{
    private Button loginButton;
    private Animator btnAnimator;
    public override void OnEnter()
    {
        base.OnEnter();
        loginButton = transform.Find("LoginButton").GetComponent<Button>();
        btnAnimator = loginButton.GetComponent<Animator>();
        loginButton.onClick.AddListener(OnLoginClick);
    }

    private void OnLoginClick()
    {
        PlayClickSound();
        uiMgr.PushPanel(UIPanelType.Login);
    }


    public override void OnPause()
    {
        base.OnPause();
        btnAnimator.enabled = false;
        loginButton.transform.DOScale(0, 0.3f).OnComplete(() =>
             SetActive(loginButton, false)
        );
    }


    public override void OnResume()
    {
        base.OnResume();
        SetActive(loginButton,true);
        loginButton.transform
            .DOScale(1, 0.3f)
            .OnComplete(() =>
                btnAnimator.enabled = true
                );
    }
}
