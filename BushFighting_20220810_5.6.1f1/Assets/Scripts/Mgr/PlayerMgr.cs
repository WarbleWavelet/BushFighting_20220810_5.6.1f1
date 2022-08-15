using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;

public class PlayerMgr : BaseManager
{

    #region ��������
 public PlayerMgr(GameFacade facade) : base(facade) { }

    private UserData userData;
    private Dictionary<RoleType, RoleData> roleDataDict = new Dictionary<RoleType, RoleData>();
    private Transform rolePositions;

    private RoleType currentRoleType;
    private GameObject currentRoleGameObject;
    private GameObject playerSyncRequest;
    private GameObject remoteRoleGameObject;

    private ShootRequest shootRequest;
    private AttackRequest attackRequest;
    #endregion
   

    public void UpdateResult(int totalCount,int winCount)
    {
        userData.TotalCount = totalCount;
        userData.WinCount = winCount;
    }
    public void SetCurrentRoleType(RoleType rt)
    {
        currentRoleType = rt;
    }
    public UserData UserData
    {
        set { userData = value; }
        get { return userData; }
    }
    public override void OnInit()
    {
        rolePositions = GameObject.Find("RolePositions").transform;
        InitRoleDataDict();
    }
    private void InitRoleDataDict()
    {
        roleDataDict.Add(RoleType.Blue, new RoleData(RoleType.Blue, "Hunter_BLUE", "Arrow_BLUE", "Explosion_BLUE",rolePositions.Find("Position1")));
        roleDataDict.Add(RoleType.Red, new RoleData(RoleType.Red, "Hunter_RED", "Arrow_RED", "Explosion_RED", rolePositions.Find("Position2")));
    }
    public void SpawnRoles()
    {
        foreach(RoleData rd in roleDataDict.Values)
        {
            GameObject go= GameObject.Instantiate(rd.RolePrefab, rd.SpawnPosition, Quaternion.identity);
            go.tag = Tags.Player;
            if (rd.RoleType == currentRoleType)
            {
                currentRoleGameObject = go;
                currentRoleGameObject.GetComponent<PlayerInfo>().isLocal = true;
            }
            else
            {
                remoteRoleGameObject = go;
            }
        }
    }
    public GameObject GetCurrentRoleGameObject()
    {
        return currentRoleGameObject;
    }
    private RoleData GetRoleData(RoleType rt)
    {
        RoleData rd = null;
        roleDataDict.TryGetValue(rt, out rd);
        return rd;
    }
    public void AddControlScript()
    {
        currentRoleGameObject.AddComponent<PlayerMove>();
        PlayerAttack playerAttack = currentRoleGameObject.AddComponent<PlayerAttack>();
        RoleType rt = currentRoleGameObject.GetComponent<PlayerInfo>().roleType;
        RoleData rd = GetRoleData(rt);
        playerAttack.arrowPrefab = rd.ArrowPrefab;
        playerAttack.SetPlayerMng(this);
    }
    public void CreateSyncRequest()
    {
        playerSyncRequest=new GameObject("PlayerSyncRequest");
        playerSyncRequest.AddComponent<MoveRequest>().SetLocalPlayer(
            currentRoleGameObject.transform,
            currentRoleGameObject.GetComponent<PlayerMove>())
            .SetRemotePlayer(remoteRoleGameObject.transform);

        shootRequest=playerSyncRequest.AddComponent<ShootRequest>();
        shootRequest.playerMng = this;
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
        GameObject.Destroy(currentRoleGameObject);
        GameObject.Destroy(playerSyncRequest);
        GameObject.Destroy(remoteRoleGameObject);
        shootRequest = null;
        attackRequest = null;
    }
}
