using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

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
        facade.PlayUIAudio(AudioMgr.Sound_ButtonClick);
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
    public virtual void SetActive(Text obj, bool state = true)
    {
        obj.gameObject.SetActive(state);
    }
    #endregion
    protected T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null)
        {
            t = go.AddComponent<T>();
        }

        return t;
    }

    #region SetText
    protected void SetText(Text text, string content = "")
    {
        text.text = content;

    }
    protected void SetText(Text text, int num = 0)
    {
        text.text = num.ToString();

    }
    protected void SetText(Transform trans, string content = "")
    {
        trans.GetComponent<Text>().text = content;

    }
    protected void SetText(Transform trans, int num = 0)
    {
        trans.GetComponent<Text>().text = num.ToString();

    }
    #endregion

    protected void AddBtnListener(Transform t, UnityEngine.Events.UnityAction action)
    {
        Button btn = t.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(action);
        }
        else
        {
            Debug.LogErrorFormat("组件{0}没有Button组件", t.gameObject.name);
        }
    }      
    
    protected void SetAndAddBtnListener(ref  Button btn, Transform t, UnityEngine.Events.UnityAction action)
    {
         btn = t.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(action);
        }
        else
        {
            Debug.LogErrorFormat("组件{0}没有Button组件", t.gameObject.name);
        }
    }


    #region GetRect
    protected RectTransform GetRect(Transform _Object)
    { 
      return  _Object.GetComponent<RectTransform>();
    }       
    
    protected RectTransform GetRect(GameObject _Object)
    { 
      return  _Object.GetComponent<RectTransform>();
    }       
    
    protected RectTransform GetRect(LayoutGroup _Object)
    { 
      return  _Object.GetComponent<RectTransform>();
    }
    #endregion


    #region SetParent
    protected void SetParent(GameObject _Object, GameObject parent)
    {
        _Object.transform.SetParent( parent.transform );
    }       
    
    protected void SetParent(Transform _Object, Transform parent)
    {
        _Object.SetParent( parent.transform );
    }        
    
    protected void SetParent(RectTransform _Object, RectTransform parent)
    {
        _Object.SetParent( parent.transform );
    }
    #endregion  
}
