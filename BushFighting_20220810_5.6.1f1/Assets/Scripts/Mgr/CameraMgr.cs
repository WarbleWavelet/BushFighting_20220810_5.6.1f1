using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CameraMgr : BaseManager
{

    #region 字属构造
    private GameObject cameraGo;
    private Animator cameraAnim;
    private FollowTarget followTarget;
    private Vector3 originalPosition;
    private Vector3 originalRotation;

    bool getKeyCodeRetuen = false;//test
    bool getKeyCodeEsc = false;

    public CameraMgr(GameFacade facade) : base(facade) { }
    #endregion


    #region 生命
   public override void OnInit()
    {
        getKeyCodeRetuen = false;
        getKeyCodeEsc = false;
        cameraGo = Camera.main.gameObject;  
        cameraAnim = cameraGo.GetComponent<Animator>();
        followTarget = cameraGo.GetComponent<FollowTarget>();
    
    }

    public override void Update()
    {


       // Test_CameraTween();
    }
  
    #endregion
  /// <summary>
   /// 测试相机的推进拉远，Update
   /// </summary>
    private void Test_CameraTween()
    {
        if (Input.GetKeyDown(KeyCode.Return) && getKeyCodeRetuen == false && getKeyCodeEsc == false) ///回车 ，防止多次按下
        {
            getKeyCodeRetuen = true;
            FollowRole();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && getKeyCodeRetuen == false && getKeyCodeEsc == false) //Esc           
        {
            getKeyCodeEsc = true;
            WalkthroughScene();
        }
    }

    /// <summary>
    /// 相机跟随主角
    /// </summary>
    public void FollowRole()
    {
          cameraAnim.enabled = false;         
        followTarget.target = facade.GetCurrentRoleGameObject().transform;
        //followTarget.target = GameObject.FindWithTag(Tags.Player).transform;//测试用的
      
        originalPosition = cameraGo.transform.position;
        originalRotation = cameraGo.transform.eulerAngles;

        Quaternion targetQuaternion = Quaternion.LookRotation(followTarget.target.position - cameraGo.transform.position);  //看向玩家
        cameraGo.transform.DORotateQuaternion(targetQuaternion, 1f).OnComplete(delegate()                                   //移动到玩家附近
        {
            followTarget.enabled = true;
            getKeyCodeRetuen = false;
        });
    }

    /// <summary>
    /// 相机漫游场景
    /// </summary>
    public void WalkthroughScene()
    {
        followTarget.enabled = false;
        cameraGo.transform.DOMove(originalPosition, 1f); //镜头拉远
        cameraGo.transform.DORotate(originalRotation, 1f).OnComplete( delegate()  //镜头摆正还原
        {
            cameraAnim.enabled = true;
            getKeyCodeEsc = false;
        });
    }
}
