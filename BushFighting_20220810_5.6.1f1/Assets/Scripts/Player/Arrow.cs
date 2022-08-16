/****************************************************

	文件：
	作者：WWS
	日期：2022/08/16 17:52:27
	功能：箭

*****************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
public class Arrow : MonoBehaviour
{
    
    public RoleType roleType;
    public int speed = 5;
    public GameObject explosionEffect;
    public bool isLocal = false;
    private Rigidbody rgd;

	void Start () 
    {
        rgd = GetComponent<Rigidbody>();
	}
	
	void Update () 
    {
        rgd.MovePosition( transform.position+ transform.forward * speed * Time.deltaTime);
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.Player)
        {
            GameFacade.Instance.PlayUIAudio(AudioMgr.Sound_ShootPerson);
            if (isLocal)
            {
                bool playerIsLocal = other.GetComponent<PlayerInfo>().isLocal;
                if (isLocal != playerIsLocal)
                {
                    GameFacade.Instance.SendAttack( Random.Range(10,20) );
                }
            }
        }
        else
        {
            GameFacade.Instance.PlayUIAudio(AudioMgr.Sound_Miss);
        }
        GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        GameObject.Destroy(this.gameObject);
    }
}
