/****************************************************

	文件：
	作者：WWS
	日期：2022/08/18 11:10:14
	功能：大厅左边面板的人物战绩显示

*****************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using Protocol;


public class UpdateResultRequest:BaseRequest
{
    private RoomListPanel roomListPanel;
    private bool isUpdateResult = false;
    private int totalCount;
    private int winCount;
    public override void Awake()
    {
        actionCode = ActionCode.UpdateResult;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }
    private void Update()
    {
        if (isUpdateResult)
        {
            roomListPanel.OnUpdateResultResponse(totalCount,winCount);
            isUpdateResult = false;
        }
    }
    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        totalCount = int.Parse(strs[0]);
        winCount = int.Parse(strs[1]);
        isUpdateResult = true;
    }
}
