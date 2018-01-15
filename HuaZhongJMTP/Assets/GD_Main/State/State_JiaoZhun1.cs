using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using System;
using DG.Tweening;
/// <summary>
/// 校准步骤
/// </summary>
public class State_JiaoZhun1 :State_Base {
   

    bool _DoorClose = true;
    bool readyToJiaoZhun = false;
    private float delayTime=1f;
    public override void OnBegin()
    {
        mouseClock = false;
        SingleModel.UI_TextDanWeiInfo.text = SingleModel.danWei;
        SingleModel.UI_TextInfo.text = SingleModel.zero;
        // CameraViewer.Instance.ChangeCameraView(SingleModel.Model_FXTP, 0, 60, 0.4f, 2, false);
        CameraViewer.Instance.IsAutoChangeView = false;
        readyToJiaoZhun = false;
        _DoorClose = true;
    }
    public override void OnEnd()
    {

        MyLogger.Instance.HideLog();
        if (_CallBack != null)
        {
            _CallBack(1);
        }
    }
    public override void OnUpdate()
    {
    }
    /// <summary>
    /// 开门操作
    /// </summary>
    void OpenTheDoor()
    {
        SingleModel.Model_Door.DOMove(Vector3.forward * .15f, 2).OnComplete(() => { _DoorClose = false; });
    }
    /// <summary>
    /// 关门
    /// </summary>
    void CloseTheDoor()
    {
        SingleModel.Model_Door.DOMove(Vector3.zero, 2).OnComplete(() => { _DoorClose = true; });
    }

    public override void ButtonOnClick(string buttonName)
    {
        if (mouseClock) return;
        InfoPanel.Instance.HideInfo();
        switch (buttonName)
        {
            case "_TareR":
            case "_TareL":
                //提示无可去皮操作
                       InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
                XMLManager.Instance.strConfigDir["NoWayToRemoveInfoLog"],155);
               // mouseClock = true;
               // MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["NoWayToRemoveInfoLog"], 350, MyLogger.TextAlign.Center,delegate { mouseClock = false; });
                break;
            case "_Door":
                if (_DoorClose)
                {
                    OpenTheDoor();
                }
                else
                {
                    CloseTheDoor();
                }
                break;
        }
    }

    public override void ButtonOnDrag(string buttonName)
    {
        if (mouseClock) return;
        switch (buttonName)
        {
            case "_Cal":
                if (!_DoorClose)
                {
                                InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
                    XMLManager.Instance.strConfigDir["NoCloseInfoLog"],160,delegate { CloseTheDoor(); });
                   // mouseClock = true;
                   // MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["NoCloseInfoLog"], 350, MyLogger.TextAlign.Center,delegate { mouseClock = false; });
                    return;
                }
                ////已经处在校准状态了
                if (readyToJiaoZhun)
                {
                    return;
                }
                  SingleModel.UI_LoadingCal.fillAmount += Time.deltaTime/delayTime;
                if (SingleModel.UI_LoadingCal.fillAmount >= 1f)
                {
                    SingleModel.UI_TextInfo.text = "Cal";
                    SingleModel.UI_TextDanWeiInfo.text =SingleModel.Close;
                    readyToJiaoZhun = true;
                }
                break;
            case "_Power":
                SingleModel.UI_LoadingPower.fillAmount += Time.deltaTime / delayTime;
                if (SingleModel.UI_LoadingPower.fillAmount >= 1f)
                {
                    SingleModel.UI_LoadingPower.fillAmount = 0;
                  //  RemoveAll();
                    FXTPMainManager.Instance.BackOnState();
                }
                break;
        }
    }

    public override void ButtonOnUp(string buttonName)
    {
        if (mouseClock) return;
        switch (buttonName)
        {
            case "_Cal":
                //如果已经处在校准的模式 提示操作者
               // if(SingleModel.UI_LoadingCal.fillAmount < 0.5f&& readyToJiaoZhun)
               // {
                  //  InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
                                 //    XMLManager.Instance.strConfigDir["CheckButtonInfoLog"]);
                   // mouseClock = true;
                   // MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["CheckButtonInfoLog"], 350, MyLogger.TextAlign.Center,delegate { mouseClock = false; });
                   // SingleModel.UI_LoadingCal.fillAmount = 0;
                   // return;
               // }
                if (readyToJiaoZhun)
                {
                   // SingleModel.UI_LoadingCal.fillAmount = 0;
                    OnEnd();
                }
                SingleModel.UI_LoadingCal.fillAmount = 0;
                break;
            case "_Power":
                    SingleModel.UI_LoadingPower.fillAmount = 0;
                break;
        }
    }

    protected override void RemoveAll()
    {
        MyLogger.Instance.HideLog();
        SingleModel.Model_Door.DOPause();
        SingleModel.UI_TextInfo.DOFade(1, 0f);
        SingleModel.Model_Door.position = Vector3.zero;
       
    }
    /// <summary>
    /// 初始化工作
    /// </summary>
    public override void Resert()
    {
        RemoveAll();
        //OnBegin();
        
    }
}
