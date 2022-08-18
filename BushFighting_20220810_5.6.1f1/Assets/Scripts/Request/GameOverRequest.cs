/****************************************************

	文件：
	作者：WWS
	日期：2022/08/18 11:14:33
	功能：游戏结束

*****************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
public class GameOverRequest:BaseRequest
{


    private GamePanel gamePanel;
    private bool isGameOver = false;
    private ReturnCode returnCode;
    public override void Awake()
    {
        reqCode = ReqCode.Game;
        actionCode = ActionCode.GameOver;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }
    private void Update()
    {
        if (isGameOver)
        {
            gamePanel.OnGameOverResponse(returnCode);
            isGameOver = false;
        }
    }
    public override void OnResponse(string data)
    {
        returnCode = (ReturnCode)int.Parse(data) ;
        isGameOver = true;
    }
}
