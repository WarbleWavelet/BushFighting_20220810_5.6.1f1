/****************************************************

	文件：
	作者：WWS
	日期：2022/08/18 11:08:17
	功能：中途退出战斗

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;



public class QuitBattleRequest : BaseRequest
{
    private bool isQuitBattle = false;
    private GamePanel gamePanel;
    public override void Awake()
    {
        reqCode = ReqCode.Game;
        actionCode = ActionCode.QuitBattle;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }
    public override void SendRequest()
    {
        base.SendRequest("r");
    }
    private void Update()
    {
        if (isQuitBattle)
        {
            gamePanel.OnExitResponse();
            isQuitBattle = false;
        }
    }
    public override void OnResponse(string data)
    {
        isQuitBattle = true;
    }
}
