/****************************************************

	文件：
	作者：WWS
	日期：2022/08/18 11:11:58
	功能：进入游戏之后，开始战斗之前的 倒计时

*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
public class ShowTimerRequest : BaseRequest
{


 
    private GamePanel gamePanel;
    public override void Awake()
    {
        //ReqCode = ReqCode.Game;
        actionCode = ActionCode.ShowTimer;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        int time = int.Parse(data);
        gamePanel.ShowTimeAsync(time);
    }
}
