using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 去皮步骤
/// </summary>
public class State_Remove : State_Base {

    /// <summary>
    /// 标识是否已经完成去皮操作
    /// </summary>
   // bool isRemove = false;
    //记录当前是否正在移动
    bool isMoveing = false;
   
    
  
    public override void OnBegin()
    {
        mouseClock = false;
        //CameraViewer.Instance.ChangeCameraView(SingleModel.Model_FXTP, 0, 55, .4f, 2, false);
        CameraViewer.Instance.IsAutoChangeView=false;
        //SingleModel.Model_GanZaoBao.SetActive(true);
       // SingleModel.Model_GanZaoBao.transform.position = new Vector3(0, 0.093f, 0.089f);
        isMoveing = false;
       // isRemove = false;
        SingleModel.UI_TextInfo.text = "0.150";
        SingleModel.UI_TextDanWeiInfo.text = SingleModel.danWei;
    }
    public override void OnEnd()
    {   MyLogger.Instance.HideLog();
        SingleModel.UI_TextInfo.text = SingleModel.zero;
        if (_CallBack != null)
        {
            _CallBack(1);
        }
    }
    public override void OnUpdate()
    {
    }

    void RemoveGanZaoBao()
    {
        SingleModel.Model_GanZaoBao.transform.DOMoveX(0.2f, 1.5f).OnComplete(() => { CloseTheDoor(); SingleModel.Model_GanZaoBao.SetActive(false); });
    }
    /// <summary>
    /// 打开天平的门
    /// </summary>
    void OpenTheDoor()
    {
        isMoveing = true;
        SingleModel.Model_Door.DOMove(Vector3.forward * .15f, 2).OnComplete( RemoveGanZaoBao);
        //更改提示信息
    }
    /// <summary>
    /// 关门
    /// </summary>
    void CloseTheDoor()
    {
        // SingleModel.Model_Door.DOMove(Vector3.zero, 2).OnComplete(() => { isRemove=true;isMoveing = false; });
        SingleModel.Model_Door.DOMove(Vector3.zero, 2).OnComplete(() => { //isRemove = true;
            isMoveing = false; OnEnd(); });
        //更改提示信息
        // CameraViewer.Instance.ChangeCameraView( SingleModel.Model_FXTP,0, 55, .4f, 2,false);
    }
    public override void ButtonOnUp(string buttonName)
    {
        if (mouseClock) return;
        switch (buttonName)
        {
            case "_Power":
                    SingleModel.UI_LoadingPower.fillAmount = 0;
                break;
        }
    }
    public override void ButtonOnDrag(string buttonName)
    {
        if (mouseClock) return;
        switch (buttonName)
        {


            case "_Power":
                SingleModel.UI_LoadingPower.fillAmount += Time.deltaTime / 2f;
                if (SingleModel.UI_LoadingPower.fillAmount >= 1f)
                {
                    SingleModel.UI_LoadingPower.fillAmount = 0;
                    FXTPMainManager.Instance.BackOnState();
                }
                break;
        }
    }
    public override void ButtonOnClick(string buttonName)
    {
        if (mouseClock) return;

        if (isMoveing)
        {
            return;
        }
        InfoPanel.Instance.HideInfo();
        switch (buttonName)
        {
                case "_TareR":
                case "_TareL":
               // OpenTheDoor();
               // if (isRemove)
               // {
                    //去皮完成
                   
                   SingleModel.UI_TextInfo.text = SingleModel.zero;
                    //结束当前步骤
                    OnEnd();

                //播放取出动画
              //  }
               // else
               // {
                    ////提示去去皮前需要取出干燥包
                    //mouseClock = true;
                    //MyLogger.Instance.Log(XMLManager.Instance.clickConfigDir["RemoveGanZaoBaoInitInfoLog"] , 350, MyLogger.TextAlign.Center, delegate
                    //{
                    //    mouseClock = false;
                    //    //播放取出 干燥包的动画
                    //    OpenTheDoor();
                    //    //转移视角
                    //    CameraViewer.Instance.ChangeCameraView(SingleModel.Model_Door,-60, 50, .6f, 2,false);
                       
                    //});
               // }
                break;
            //case "_Cal":
            //         InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
            //    XMLManager.Instance.strConfigDir["NoRemoveInfoLog"]);
                //mouseClock = true;
               // MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["NoRemoveInfoLog"], 350, MyLogger.TextAlign.Center,delegate { mouseClock = false; });
               // break;
            case "_Power":break;
            //case "ganzaobap":OpenTheDoor(); break;
                
            default:
               // mouseClock = true;
              //  MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["NoRemoveInfoLog"], 350, MyLogger.TextAlign.Center, delegate { mouseClock = false; });

                InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
                XMLManager.Instance.strConfigDir["RemoveInitInfoLog"],120);//等待去皮
                break;
        }
    }
    protected override void RemoveAll()
    {
        MyLogger.Instance.HideLog();
        SingleModel.Model_Door.DOPause();
        SingleModel.Model_Door.position = Vector3.zero;
        //SingleModel.Model_GanZaoBao.transform.DOPause();
    }
    public override void Resert()
    {
        RemoveAll();
       // OnBegin();
    }
}
