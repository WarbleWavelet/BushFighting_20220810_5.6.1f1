/****************************************************

	文件：
	作者：lenovo
	日期：2022/8/15 11:24:33
	功能：

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour 
{

   

   

    public GameObject arrowPrefab;
    private Animator anim;
    private Transform leftHandTrans;
    private Vector3 shootDir;
    private PlayerMgr playerMgr;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        leftHandTrans = transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand");
	}
	
	// Update is called once per frame
	void Update () {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))//点击地面
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray= Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray, out hit);
                if (isCollider)
                {
                    Vector3 targetPoint = hit.point;
                    targetPoint.y = transform.position.y;
                    shootDir = targetPoint - transform.position;
                    transform.rotation = Quaternion.LookRotation(shootDir);
                    anim.SetTrigger("Attack");
                    //Invoke("Shoot", 0.1f);
                    Invoke("ShootTest", 0.1f);
                }
            }
        }
	}
    public void SetPlayerMng(PlayerMgr playerMng)
    {
        this.playerMgr = playerMng;
    }
    private void Shoot()
    {
        playerMgr.Shoot(arrowPrefab, leftHandTrans.position, Quaternion.LookRotation(shootDir));
    }

    void ShootTest()
    {
       // facade.PlayUIAudio(AudioMgr.Sound_Timer);
        GameObject.Instantiate(arrowPrefab, leftHandTrans.position, Quaternion.LookRotation(shootDir)).GetComponent<Arrow>().isLocal = true;
    }
}
