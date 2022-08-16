using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Protocol;

public class GamePanel : BasePanel 
{

    #region 字属构造
    private Text timer;
    private int time = -1;

    private Button successBtn;
    private Button failBtn;
    private Button exitBtn;

    ShowTimerRequest showTimerRequest;
    StartPlayRequest startPlayRequest;
    GameOverRequest gameOverRequest;

    private QuitBattleRequest quitBattleRequest;
    #endregion



    #region 生命
    private void Start()
    {
        timer = GetOrAddComponent<Text>( transform.Find("Timer").gameObject );
         SetActive(timer,false);


        SetAndAddBtnListener(ref  successBtn, transform.Find("SuccessButton"), OnResultClick);
        SetActive(successBtn, false);

        SetAndAddBtnListener(ref failBtn, transform.Find("FailButton"), OnResultClick);
        SetActive(failBtn, false);

        SetAndAddBtnListener(ref exitBtn, transform.Find("ExitButton"), OnExitClick);
        SetActive(exitBtn, false);

        //
        showTimerRequest = GetOrAddComponent<ShowTimerRequest>(gameObject);
        startPlayRequest = GetOrAddComponent<StartPlayRequest>(gameObject);
        gameOverRequest = GetOrAddComponent<GameOverRequest>(gameObject);
        quitBattleRequest = GetOrAddComponent<QuitBattleRequest>(gameObject);

    }
    private void Update()
    {
        if (time > -1)
        {
            ShowTime(time);
            time = -1;
        }
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
    }
    public override void OnExit()
    {
       SetActive(successBtn,false);
       SetActive(failBtn,false);
       SetActive(exitBtn,false);
        gameObject.SetActive(false);
    }

    #endregion

    private void OnResultClick()
    {
        uiMgr.PopPanel();
        uiMgr.PopPanel();
        facade.GameOver();
    }
    private void OnExitClick()
    {
        quitBattleRequest.SendRequest();
    }
    public void OnExitResponse()
    {
        OnResultClick();
    }
    public void ShowTimeAsync(int time)
    {
        this.time = time;
    }


    /// <summary>
    /// 计时UI
    /// </summary>
    /// <param name="time"></param>
    public void ShowTime(int time)
    {
        if (time == 3)
        {
            SetActive(exitBtn,true);
        }
        SetActive(timer,true);
        timer.text = time.ToString();
        timer.transform.localScale = Vector3.one;
        Color tempColor = timer.color;
        tempColor.a = 1;
        timer.color = tempColor;
        //
        timer.transform.DOScale(2, 0.3f).SetDelay(0.3f);
        timer.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(() => SetActive(timer, false));
        facade.PlayUIAudio();
    }
    public void OnGameOverResponse(ReturnCode returnCode)
    {
        Button tempBtn = null;
        switch (returnCode)
        {
            case ReturnCode.Success:
                tempBtn = successBtn;
                break;
            case ReturnCode.Fail:
                tempBtn = failBtn;
                break;
        }
        tempBtn.gameObject.SetActive(true);
        tempBtn.transform.localScale = Vector3.zero;
        tempBtn.transform.DOScale(1, 0.5f);
    }

}
