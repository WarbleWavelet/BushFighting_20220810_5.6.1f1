using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagePanel :BasePanel {

    private Text txtShow;
    private float showTime = 1;
    private string msg = null;

    public override void OnEnter()
    {
        base.OnEnter();
        txtShow = GetComponent<Text>();
        txtShow.enabled = false;
        uiMgr.InjectMsgPanel(this);
    }

    private void Update()
    {
        if (msg != null)
        {
            ShowMsg(msg);
            msg = null;
        }
    }



    public void ShowMgrSync(string msg)
    {
        this.msg = msg;
    }
    public void ShowMsg(string msg)
    {
        txtShow.CrossFadeAlpha(1, 0.2f, false);
        txtShow.text = msg;
        txtShow.enabled = true;
        Invoke("Hide", showTime);
    }
    private void Hide()
    {
        txtShow.CrossFadeAlpha(0, 1,false);
    }

}
