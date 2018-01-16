using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 调整水平仪的步骤
/// </summary>
public class State_Gradienter : State_Base
{
    /// <summary>
    /// 还需调整的次数
    /// </summary>
    private int count;
    //记录变换目标前摄像机的目标
    private Transform cameraTarget;
    /// <summary>
    /// 需要看向的物体
    /// </summary>
    Transform _FX_Base;

    private Transform FXTP_Base;
    /// <summary>
    /// 水平仪的中心点
    /// </summary>
    RectTransform centerPoint;
    /// <summary>
    /// 显示校准面板
    /// </summary>
    private Image image_Left, image_Right;
    CanvasGroup canvasGroup;
    /// <summary>
    /// 记录摄像机是否已经照向指定的位置
    /// </summary>
   // private bool cameraMoveEnd = false;
    public override void OnBegin()
    {
        
        Init();
        LookAt();
       // OpenBalanceTip();
    }
    //初始化操作
    void Init()
    {
        FXTP_Base = GameObject.Find("+ Models/FXTP/Balance_FX/Balance_FX_base_02").transform;
           mouseClock = false;
        centerPoint= GameObject.Find("TipCanvas/BanlanceTip/T_Balance/CircleTip/shadow")
            .GetComponent<RectTransform>();
        centerPoint.anchoredPosition = new Vector2(57, 19);
        count = 3;
        if (canvasGroup == null)
        {
            canvasGroup = GameObject.Find("TipCanvas/BanlanceTip").GetComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
        Button[] buttons = canvasGroup.transform.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++)
        {
            //if (buttons[i].onClick == null)
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener( Balance);
        }
        if (image_Right == null)
        {
            image_Right = canvasGroup.transform.Find("Right").GetComponent<Image>();
            image_Left = canvasGroup.transform.Find("Left").GetComponent<Image>();
        }
    }
    /// <summary>
    /// 打开水平调节面板
    /// </summary>
    void OpenBalanceTip()
    {
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1f, 1f);
       image_Left .DOFade(0.2f, 0.8f).SetLoops(-1, LoopType.Yoyo);
        image_Right.DOFade(0.2f, 0.8f).SetLoops(-1, LoopType.Yoyo);
    }
    /// <summary>
    /// 关闭水平调节面板
    /// </summary>
    void CloseBalanceTip()
    {
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0f, 1f);
    }
    //校准的方法
    void Balance()
    {
        if (FXTPMainManager.Instance.isShowDes) return;
        if (mouseClock) return;

        FXTP_Base.DOLocalRotate(FXTP_Base.localEulerAngles + Vector3.up * 20, 1f);  
        switch (count)
        {
            case 1: mouseClock = true; centerPoint.DOAnchorPos(new Vector2(0, -4), 1f).OnComplete(()=> { mouseClock = false; OnEnd(); }); count--; break;
            case 2: mouseClock = true; centerPoint.DOAnchorPos(new Vector2(52, -3), 1f).OnComplete(() => { mouseClock = false; }); count--; break;
            case 3: mouseClock = true; centerPoint.DOAnchorPos(new Vector2(-44,-56), 1f).OnComplete(() => { mouseClock = false; }); count--;  break;
            default:break;
        }
    }
    /// <summary>
    /// 控制摄像机看向指定点
    /// </summary>
    void LookAt()
    {
        _FX_Base = GameObject.Find("+ Models/FXTP/Balance_FX/Balance_FX_base_02").transform;
        // CameraViewer.Instance.ChangeCameraView(SingleModel.Model_FXTP, -60, 40, 0.3f, 2,delegate { OpenBalanceTip();return true; });
        CameraViewer.Instance.ChangeCameraView(SingleModel.Model_FXTP, -47, 20, 0.35f, 1, delegate { FXTPMainManager.Instance.currentAutoChange = true;OpenBalanceTip(); return false; });
    }
    public override void OnEnd()
    {
        CloseBalanceTip();
        if (_CallBack != null)
        {
            _CallBack(1);
        }
    }
    protected override void RemoveAll()
    {
        MyLogger.Instance.HideLog();
        centerPoint.DOPause();
        image_Left.DOPause();
        image_Right.DOPause();

        canvasGroup.DOPause();
        image_Left.DOFade(1, 0);
        image_Right.DOFade(1, 0);
        //base.RemoveAll();
    }
    public override void Resert()
    {
        RemoveAll();
       // OnBegin();
    }
    public override void ButtonOnClick(string buttonName)
    {
        if (mouseClock) return;
        InfoPanel.Instance.HideInfo();
        if (buttonName.Equals("_Power"))//提示请先完成水平调节
        {
            InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint, XMLManager.Instance.strConfigDir["NoBalanceInfoLog"]); 
            //mouseClock = true;
            //MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["NoBalanceInfoLog"], 350, MyLogger.TextAlign.Center,delegate { mouseClock = false; });
        }
       
        else
        {  //提示需要先开机
            InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint, XMLManager.Instance.strConfigDir["NoOnInfoLog"]);
            //mouseClock = true;
            //MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["NoOnInfoLog"], 350, MyLogger.TextAlign.Center,delegate { mouseClock = false; });
        }
    }
}
