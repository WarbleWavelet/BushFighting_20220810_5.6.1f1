/****************************************************

	文件：
	作者：WWS
	日期：2022/08/18 11:11:01
	功能：结束倒计时，开始战斗

*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;


public class StartPlayRequest : BaseRequest {

    private bool isStartPlaying = false;

    public override void Awake()
    {
        actionCode = ActionCode.StartPlay;
        base.Awake();
    }

    private void Update()
    {
        if (isStartPlaying)
        {
            facade.StartPlaying();
            isStartPlaying = false;
        }
    }

    public override void OnResponse(string data)
    {
        isStartPlaying = true;
    }
}
