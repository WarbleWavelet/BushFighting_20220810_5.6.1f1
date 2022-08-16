using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMgr : BaseManager {

    private GameObject cameraGo;
    private Animator cameraAnim;
    private FollowTarget followTarget;
    private Vector3 originalPosition;
    private Vector3 originalRotation;

    public CameraMgr(GameFacade facade) : base(facade) { }

    public override void OnInit()
    {
        cameraGo = Camera.main.gameObject;
        cameraAnim = cameraGo.GetComponent<Animator>();
       // followTarget = cameraGo.GetComponent<FollowTarget>();
    }

    //public override void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        FollowTarget(null);
    //    }
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        WalkthroughScene(); 
    //    }
    //}

    /// <summary>
    /// 相机跟随主角
    /// </summary>
    public void FollowRole()
    {
       // return;
        followTarget.target = facade.GetCurrentRoleGameObject().transform;
        cameraAnim.enabled = false;         
        originalPosition = cameraGo.transform.position;
        originalRotation = cameraGo.transform.eulerAngles;

        Quaternion targetQuaternion = Quaternion.LookRotation(followTarget.target.position - cameraGo.transform.position);
        cameraGo.transform.DORotateQuaternion(targetQuaternion, 1f).OnComplete(delegate()
        {
            followTarget.enabled = true;
        });
    }
    public void WalkthroughScene()
    {
        return;
        followTarget.enabled = false;
        cameraGo.transform.DOMove(originalPosition, 1f);
        cameraGo.transform.DORotate(originalRotation, 1f).OnComplete( delegate()
        {
            cameraAnim.enabled = true;
        });
    }
}
