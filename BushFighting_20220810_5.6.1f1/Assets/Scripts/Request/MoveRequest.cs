/****************************************************

	文件：
	作者：WWS
	日期：2022/08/17 19:57:19
	功能：角色移动请求（上传角色的移动位置信息给服务器同步）

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
public class MoveRequest : BaseRequest 
{

  
    private Transform localPlayerTrans;
    private PlayerMove localPlayerMove;
    /// <summary>每秒同步几次</summary>
    private int syncPerSec = 30;//影响移动顺滑度

    //
     /// <summary>同步对手信息</summary>
    private bool isSyncRemotePlayer = false;
    private Animator remotePlayerAnim;
    private Transform remotePlayerTrans;
    private Vector3 remotePlayer_Pos;
    private Vector3 remotePlayer_Ang;
    private float remotePlayer_forward;
    

    #region 生命
 public override void Awake()
    {
        reqCode = ReqCode.Game;
        actionCode = ActionCode.Move;
        base.Awake();
    }
    private void Start()
    {
        InvokeRepeating( "SyncLocalPlayer", 1f, 1f / syncPerSec);//等1s再同步
    }
    private void FixedUpdate()
    {
        if (isSyncRemotePlayer)
        {
            SyncRemotePlayer();
            isSyncRemotePlayer = false;
        }
    }
    #endregion


    private void SendRequest(float x, float y, float z, float rotationX, float rotationY, float rotationZ, float forward)
    {
        string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}", x, y, z, rotationX, rotationY, rotationZ, forward);
        base.SendRequest(data);
    }
    public override void OnResponse(string data)
    {
        //27.75,0,1.41-0,0,0-0 //这种不行因为E-xxx，表示极小值也有- 冲突了
        //print(data);
        string[] strs = data.Split('|');
        remotePlayer_Pos = CommonUnity.Split_Str2Vec3(strs[0]);
        remotePlayer_Ang = CommonUnity.Split_Str2Vec3(strs[1]);
        remotePlayer_forward = float.Parse(strs[2]);
        isSyncRemotePlayer = true;
    }


    #region 辅助
    /// <summary>
    ///  初始玩家  get from Mgr
    /// </summary>
    /// <param name="localPlayerTrans"></param>
    /// <param name="localPlayerMove"></param>
    /// <returns></returns>
    public MoveRequest SetLocalPlayer(Transform localPlayerTrans, PlayerMove localPlayerMove)
    {
        this.localPlayerTrans = localPlayerTrans;
        this.localPlayerMove = localPlayerMove;
        return this;
    }


    /// <summary>
    /// push  本地信息推到服务器  Invoke
    /// </summary>
    private void SyncLocalPlayer()
    {
        Vector3 pos = localPlayerTrans.position;
        Vector3 ang = localPlayerTrans.eulerAngles;
        SendRequest(pos.x, pos.y, pos.z,
            ang.x, ang.y, ang.z,
            localPlayerMove.forward);
    }


    /// <summary>
    /// 设置对手信息  get from Mgr
    /// </summary>
    /// <param name="remotePlayerTrans"></param>
    /// <returns></returns>
    public MoveRequest SetRemotePlayer(Transform remotePlayerTrans)
    {
        this.remotePlayerTrans = remotePlayerTrans;
        this.remotePlayerAnim = remotePlayerTrans.GetComponent<Animator>();
        return this;
    }



    /// <summary>
    /// 同步对手信息(移动、位置)  set
    /// 协程，所以设置要放在update fixupdate
    /// </summary>
    private void SyncRemotePlayer()
    {
        remotePlayerTrans.position      = remotePlayer_Pos;
        remotePlayerTrans.eulerAngles   = remotePlayer_Ang;
        remotePlayerAnim.SetFloat("Forward", remotePlayer_forward);
    }
    #endregion
   


    
}
