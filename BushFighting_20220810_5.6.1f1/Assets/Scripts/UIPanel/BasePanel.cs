using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BasePanel : MonoBehaviour 
{
    //public interface  BindUI {  }

    #region 字属
    protected UIMgr uiMgr;
    protected GameFacade facade;

    public UIMgr UIMng
    {
        set { uiMgr = value; }
    }

    public GameFacade Facade
    {
        set { facade = value; }
    }
    #endregion


    protected void PlayClickSound()
    {
        facade.PlayNormalSound(AudioMgr.Sound_ButtonClick);
    }



    #region 生命
    /// <summary>
    /// 界面被显示出来
    /// </summary>
    public virtual void OnEnter()
    {

    }

    /// <summary>
    /// 界面暂停
    /// </summary>
    public virtual void OnPause()
    {

    }

    /// <summary>
    /// 界面继续
    /// </summary>
    public virtual void OnResume()
    {

    }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关系
    /// </summary>
    public virtual void OnExit()
    {

    }
    #endregion




    #region SetActive
    public virtual void SetActive(GameObject obj, bool state = true) //obj万物源于UnityEngine.Object
    {
        obj.SetActive(state);
    }
    public virtual void SetActive(Transform obj, bool state = true)
    {
        obj.gameObject.SetActive(state);
    }
    public virtual void SetActive(RectTransform obj, bool state = true)
    {
        obj.gameObject.SetActive(state);
    }     
    public virtual void SetActive(Button obj, bool state = true)
    {
        obj.gameObject.SetActive(state);
    }
    #endregion

}
