/****************************************************
    文件：CommonUnity.cs
	作者：lenovo
    邮箱: 
    日期：2022/8/17 17:43:2
	功能：底层用到Unity的Common类
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CommonUnity
{
    /// <summary>
    /// Split String to Vector3
    /// </summary>
    /// <param name="str"></param>
    /// <param name="split"></param>
    /// <returns></returns>
    public static Vector3 Split_Str2Vec3(string str, char split =',')
    {
        string[] strs = str.Split(split);
        float x = float.Parse(strs[0]);
        float y = float.Parse(strs[1]);
        float z = float.Parse(strs[2]);
        return new Vector3(x,y,z);
    }
}
