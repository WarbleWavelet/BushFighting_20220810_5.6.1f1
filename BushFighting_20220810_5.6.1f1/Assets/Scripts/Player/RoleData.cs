/****************************************************

	文件：
	作者：WWS
	日期：2022/08/16 18:04:32
	功能：人物上挂载的所有物体、组件、脚本

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;


public class RoleData
{
    private const string PREFIX_PREFAB = DefinePath.Prefab_Role_Prefix;
    /// <summary>角色类型</summary> 
    public RoleType RoleType { get; private set; } 
    public GameObject RolePrefab { get; private set; }
    /// <summary>箭</summary>
    public GameObject ArrowPrefab { get; private set; }
    /// <summary>人物生成位置</summary>
    public Vector3 SpawnPosition { get; private set; }
    public GameObject ExplostionEffect { get; private set; }

    public RoleData(RoleType roleType,string rolePath,string arrowPath,string explosionPath, Transform spawnPos)
    {
        this.RoleType = roleType;
        this.RolePrefab = Resources.Load( PREFIX_PREFAB + rolePath) as GameObject;
        this.ArrowPrefab = Resources.Load( PREFIX_PREFAB + arrowPath) as GameObject;
        this.ExplostionEffect = Resources.Load( PREFIX_PREFAB + explosionPath) as GameObject;
        ArrowPrefab.GetComponent<Arrow>().explosionEffect = ExplostionEffect;
        this.SpawnPosition = spawnPos.position;
    }
}
