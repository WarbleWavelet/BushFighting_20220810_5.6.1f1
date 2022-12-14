using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
public class RequestMgr : BaseManager
{
    public RequestMgr(GameFacade facade) : base(facade) { }

    private Dictionary<ActionCode, BaseRequest> requestDic = new Dictionary<ActionCode, BaseRequest>();



    #region Request增删处理
  public void AddRequest(ActionCode actionCode,BaseRequest request)
    {
        requestDic.Add(actionCode, request);
    }
    public void RemoveRequest(ActionCode actionCode)
    {
        requestDic.Remove(actionCode);
    }

    /// <summary>
    /// 处理回复
    /// </summary>
    /// <param name="actionCode"></param>
    /// <param name="data"></param>
    public void HandleReponse(ActionCode actionCode, string data)
    {
        BaseRequest request = requestDic.TryGet<ActionCode, BaseRequest>(actionCode);
        if (request == null)
        {
            Debug.LogError("无法得到ActionCode[" + actionCode + "]对应的Request类");
            return;
        }
        request.OnResponse(data);
    }
    #endregion
  
}
