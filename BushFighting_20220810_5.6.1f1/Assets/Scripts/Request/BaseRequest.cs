using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
public class BaseRequest : MonoBehaviour 
{
    protected ReqCode reqCode = ReqCode.None;
    protected ActionCode actionCode = ActionCode.None;
    protected GameFacade _facade;

    protected GameFacade facade
    {
        get
        {
            if (_facade == null)
            { 
                  _facade = GameFacade.Instance;
            }
             
            return _facade;
        }
    }


	public virtual void Awake () {
        facade.AddRequest(actionCode, this);
    }
    public virtual void OnDestroy()
    {
        if(facade != null)
            facade.RemoveRequest(actionCode);
    }


    public virtual void SendRequest() { }
    protected void SendRequest(string data)
    {
        facade.SendRequest(reqCode, actionCode, data);
    }


    public virtual void OnResponse(string data) { }


}
