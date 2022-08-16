/****************************************************
    文件：FollowTarget.cs
	作者：lenovo
    邮箱: 
    日期：2022/8/15 13:1:20
	功能：挂载在相机上，目标是玩家（之前跟练得《黑暗之光》还是有恐龙的那个游戏的脚本）
          乱码是第一次的脚本编码不是UTF-8之类
*****************************************************/

using UnityEngine;
using System.Collections;
using System;
using System.IO;
//����͸�� Everything
public class FollowTarget : MonoBehaviour
{


    #region 字属
    public Transform target;
    private Vector3 offset;

    //����
    public float scrollSpeed = 10f;//�����ٶ�
    public float distance;//ʵ������
    public float minDistance = 3.2f;//��������
    public float maxDistance = 30f;//��С����

    //��ת
    private bool isRotate = false;
    public float rotateSpeed = 10f;
    #endregion
    
  

    void Start()
    {
        offset = transform.position - target.transform.position;

        //offset = new Vector3(0, offset.y, offset.z);//x=0�����Ҳ�ƫ��
    }


    void Update()
    {
        ProcessTarget();
    }
    void ProcessTarget()
    {
       // if (player= null) return;
        //
        transform.position = target.transform.position + offset;
        transform.LookAt(target.transform.position);

        RotateView();
        ScrollView();
    }


    /// <summary>伸缩视图</summary> 
    void ScrollView()
    {
        distance = offset.magnitude;
        distance += scrollSpeed * Input.GetAxis("Mouse ScrollWheel");
        distance = Mathf.Clamp(distance, minDistance, maxDistance);//ǯ��
        offset = offset.normalized * distance;
    }

    /// <summary>旋转视图</summary> 
    void RotateView()//������ת
    {
        if (Input.GetMouseButtonDown(1)) //按下
        {
            isRotate = true;
        }
        if (Input.GetMouseButtonUp(1))  //抬起
        {
            isRotate = false;
        }
        if (isRotate)
        {
            //��¼
            Vector3 originalPosition = transform.position;
            Quaternion originalRotation = transform.rotation;
            //��ֵ
            transform.RotateAround(target.transform.position, transform.up, rotateSpeed * Input.GetAxis("Mouse X")); 
            transform.RotateAround(target.transform.position, transform.right, rotateSpeed * Input.GetAxis("Mouse Y"));

            //���Ʒ�Χ
            if (transform.eulerAngles.x < 10 || transform.eulerAngles.x > 80)
            {
                transform.position = originalPosition;
                transform.rotation = originalRotation;
            }
        }

        offset = transform.position - target.transform.position;//��תӰ��������λ�ã�offset�����仯
    }

}
