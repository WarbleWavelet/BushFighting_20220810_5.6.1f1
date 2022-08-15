using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour 
{

    #region 字属
    private int id;
    public Text username;
    public Text totalCount;
    public Text winCount;
    public Button joinButton;


    private RoomListPanel panel;

    #endregion


	void Start () 
    {
        if (joinButton != null)
        {
            joinButton.onClick.AddListener(OnJoinClick);
        }
    }


    public void SetRoomInfo(int id, string username, int totalCount, int winCount, RoomListPanel panel)
    {
        this.id = id;
        this.username.text = username;
        this.totalCount.text = "总场数\n" + totalCount.ToString();
        this.winCount.text = "胜利\n" + winCount.ToString();
        this.panel = panel;
    }


    /// <summary>
    /// 加入房间
    /// </summary>
    private void OnJoinClick()
    {
        panel.OnJoinClick(id);
    }

    public void DestroySelf()
    {
        GameObject.Destroy(this.gameObject);
    }
}
