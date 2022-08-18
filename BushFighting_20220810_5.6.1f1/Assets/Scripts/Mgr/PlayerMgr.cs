using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class PlayerMgr : BaseManager
{

    #region 字属构造
 public PlayerMgr(GameFacade facade) : base(facade) { }

    private UserData userData;
    private Dictionary<RoleType, RoleData> roleDataDic = new Dictionary<RoleType, RoleData>();
    /// <summary>角色生成位置父节点</summary>
    private Transform roleSpawnPosParent;

    /// <summary>当前角色类型</summary> 
    private RoleType curRoleType;

   /// <summary>当前角色物体，相机跟随的目标</summary>
    private GameObject curRoleGo;
    /// <summary>玩家同步请求</summary>
    private GameObject playerSyncRequest;
 
    private GameObject remoteRoleGo;

    private ShootRequest shootRequest;
    private AttackRequest attackRequest;     
    
    public UserData UserData
    {
        set { userData = value; }
        get { return userData; }
    }
    #endregion


    #region 生命
    public override void OnInit()
    {
        roleSpawnPosParent = GameObject.Find("RolePositions").transform;
        InitRoleDataDict();
    }

    #endregion
    public void UpdateResult(int totalCount,int winCount)
    {
        userData.TotalCount = totalCount;
        userData.WinCount = winCount;
    }

    public void SetCurrentRoleType(RoleType rt)
    {
        curRoleType = rt;
    }


    private void InitRoleDataDict()
    {
        roleDataDic.Add(RoleType.Home, new RoleData(RoleType.Home, "Hunter_BLUE", "Arrow_BLUE", "Explosion_BLUE",roleSpawnPosParent.Find("Position1")));
        roleDataDic.Add(RoleType.Away, new RoleData(RoleType.Away, "Hunter_RED", "Arrow_RED", "Explosion_RED", roleSpawnPosParent.Find("Position2")));
    }

    /// <summary>
    /// 生成角色 （）
    /// </summary>
    public void SpawnRoles()
    {
        foreach(RoleData rd in roleDataDic.Values)
        {
            GameObject go= GameObject.Instantiate(rd.RolePrefab, rd.SpawnPosition, Quaternion.identity);
            go.tag = Tags.Player;
            if (rd.RoleType == curRoleType)
            {
                curRoleGo = go;
                curRoleGo.GetComponent<PlayerInfo>().isLocal = true;
            }
            else
            {
                remoteRoleGo = go;
            }
        }
    }
    public GameObject GetCurRoleGameObject()
    {
        return curRoleGo;
    }
    private RoleData GetRoleData(RoleType rt)
    {
        RoleData rd = null;
        roleDataDic.TryGetValue(rt, out rd);
        return rd;
    }

    /// <summary>
    /// 增加角色控制脚本
    /// </summary>
    public void AddControlScript()
    {
        curRoleGo.AddComponent<PlayerMove>();
        PlayerAttack playerAttack = curRoleGo.AddComponent<PlayerAttack>();
        RoleType rt = curRoleGo.GetComponent<PlayerInfo>().roleType;
        RoleData rd = GetRoleData(rt);
        playerAttack.arrowPrefab = rd.ArrowPrefab;
        playerAttack.SetPlayerMgr(this);
    }

    /// <summary>
    /// 常见角色，需要处理需要的所有request
    /// </summary>
    public void CreateSyncRequest()
    {
        playerSyncRequest=new GameObject("PlayerSyncRequest");
        MoveRequest moveRequest = playerSyncRequest.AddComponent<MoveRequest>().SetLocalPlayer(
            curRoleGo.transform,
            curRoleGo.GetComponent<PlayerMove>());
        moveRequest.SetRemotePlayer(remoteRoleGo.transform); //初始对手信息

        shootRequest=playerSyncRequest.AddComponent<ShootRequest>();
        shootRequest.playerMgr = this;
        attackRequest = playerSyncRequest.AddComponent<AttackRequest>();
    }
    public void Shoot(GameObject arrowPrefab,Vector3 pos,Quaternion rotation)
    {
        facade.PlayUIAudio(AudioMgr.Sound_Timer);
        GameObject.Instantiate(arrowPrefab, pos, rotation).GetComponent<Arrow>().isLocal = true;
        shootRequest.SendRequest(arrowPrefab.GetComponent<Arrow>().roleType, pos, rotation.eulerAngles);
    }
    public void RemoteShoot(RoleType rt, Vector3 pos, Vector3 rotation)
    {
        GameObject arrowPrefab = GetRoleData(rt).ArrowPrefab;
        Transform transform = GameObject.Instantiate(arrowPrefab).GetComponent<Transform>();
        transform.position = pos;
        transform.eulerAngles = rotation;
    }
    public void SendAttack(int damage)
    {
        attackRequest.SendRequest(damage);
    }
    public void GameOver()
    {
        //private GameObject currentRoleGameObject;
        //private GameObject playerSyncRequest;
        //private GameObject remoteRoleGameObject;

        //private ShootRequest shootRequest;
        //private AttackRequest attackRequest;
        GameObject.Destroy(curRoleGo);
        GameObject.Destroy(playerSyncRequest);
        GameObject.Destroy(remoteRoleGo);
        shootRequest = null;
        attackRequest = null;
    }
}
