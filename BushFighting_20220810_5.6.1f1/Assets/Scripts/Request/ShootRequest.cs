/****************************************************

	文件：
	作者：WWS
	日期：2022/08/17 20:41:27
	功能：同步箭的实例（移动，位置）

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class ShootRequest : BaseRequest
{    
    
    public PlayerMgr playerMgr;
    private bool isShoot = false;
    private RoleType rt;
    private Vector3 pos;
    private Vector3 rotation;

    public override void Awake()
    {
        reqCode = ReqCode.Game;
        actionCode = ActionCode.Shoot;
        base.Awake();
    }
    private void Update()
    {
        if (isShoot)
        {
            playerMgr.RemoteShoot(rt, pos, rotation);
            isShoot = false;
        }
    }

    public void SendRequest(RoleType rt,Vector3 pos,Vector3 rot)
    {
        string data = string.Format("{0}|{1},{2},{3}|{4},{5},{6}", (int)rt, 
            pos.x, pos.y, pos.z, 
            rot.x, rot.y, rot.z);
        base.SendRequest(data);
    }
    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        RoleType rt = (RoleType)int.Parse(strs[0]);
        Vector3 pos = CommonUnity.Split_Str2Vec3(strs[1]);
        Vector3 rot = CommonUnity.Split_Str2Vec3(strs[2]);
        isShoot = true;
        this.rt = rt;
        this.pos = pos;
        this.rotation = rot;
    }
}
