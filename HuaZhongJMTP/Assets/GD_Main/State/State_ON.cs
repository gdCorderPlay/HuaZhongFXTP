using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using WJ;
using UnityEngine.UI;
using System.Threading;
/// <summary>
/// 进入打开天平的步骤
/// </summary>
public class State_ON : State_Base
{
   
    bool needRemove;
    public override void OnBegin()
    {
        GameObject.Find("+ Models/FXTP/Balance_FX/Balance_FX_base_01").GetComponent<Renderer>().sharedMaterial.
            SetColor("_EmissionColor", new Color(0, 0,0));
        needRemove = true;
        mouseClock = false;
        SingleModel.UI_TextDanWeiInfo.text = SingleModel.Close; //屏幕显示为空的
        SingleModel.UI_TextInfo.text = SingleModel.Close;
        isConnectPower = false; //记录是否通电 控制不要多次点击
        SingleModel.Model_GanZaoBao.SetActive(true);
        // SingleModel.Model_GanZaoBao.transform.position = new Vector3(0, 0.093f, 0.089f);
        SingleModel.Model_GanZaoBao.transform.position = new Vector3(0.008f, 0.1f, 0.029f);
        //SingleModel.Model_GanZaoBao.GetComponent<ModelTipManager>().ShowTip();
        ///-27,27,0.7f
        CameraViewer.Instance.ChangeCameraView(SingleModel.Model_FXTP.position+Vector3.up*0.1f,-70,15,0.6f,1,delegate {
            //ShowTip();
            //提示开机前应该保持空载状态
            InfoPanel.Instance.ShowInfo(SingleModel.Model_GanZaoBao.transform.GetChild(0),  XMLManager.Instance.clickConfigDir["RemoveGanZaoBaoInitInfoLog"],350,
                delegate
                {
                    if (_tp)
                        _tp.Hide();
                    InfoPanel.Instance.HideInfo();
                    OpenTheDoor();
                }
                );
            ; return false;

        });
       // CameraViewer.Instance.IsAutoChangeView = false;

    }
    public override void OnUpdate()
    {
        if (_tp)
        {
            if (_tp.IsShowed)
            {
                _tp.Follow(Camera.main.WorldToScreenPoint(SingleModel.Model_GanZaoBao.transform.position));
            }
        }
    }

    public override void OnEnd()
    { 
        if (_CallBack != null)
        {
            _CallBack(1);
        }
    }
    /// <summary>
    /// 取出干燥包
    /// </summary>
    void RemoveGanZaoBao()
    {
        SingleModel.Model_GanZaoBao.transform.DOMoveX(0.2f, 1.5f).OnComplete(() => { CloseTheDoor(); SingleModel.Model_GanZaoBao.SetActive(false); });
    }
    void OpenTheDoor()
    {
       // isMoveing = true;
        SingleModel.Model_Door.DOMove(Vector3.forward * .15f, 1).OnComplete(RemoveGanZaoBao);
        //更改提示信息
    }
    void CloseTheDoor()
    {
        // SingleModel.Model_Door.DOMove(Vector3.zero, 2).OnComplete(() => { isRemove=true;isMoveing = false; });
        SingleModel.Model_Door.DOMove(Vector3.zero, 1).OnComplete(() => { needRemove = false;  });
        //更改提示信息
        //0, 44, .6f,
        // CameraViewer.Instance.ChangeCameraView(-15, 30, .6f, 1.5f,false);
        CameraViewer.Instance.ChangeCameraView(SingleModel.Model_FXTP.position + Vector3.up * 0.15f, 0, 30, 0.55f, 1, delegate {
            
             return true;
        });
    }

    public override void ButtonOnClick(string buttonName)
    {
        if (mouseClock) return;
        InfoPanel.Instance.HideInfo();
        switch (buttonName)
        {
            case "_Power":
                if (needRemove)
                {
                    ////提示去去皮前需要取出干燥包
                    InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint, XMLManager.Instance.clickConfigDir["RemoveGanZaoBaoInitInfoLog"]);
                    //MyLogger.Instance.Log(XMLManager.Instance.clickConfigDir["RemoveGanZaoBaoInitInfoLog"] , 350, MyLogger.TextAlign.Center, delegate
                    //{
                    //    mouseClock = false;
                    //    //播放取出 干燥包的动画
                    //    OpenTheDoor();
                    //    //转移视角
                    //    CameraViewer.Instance.ChangeCameraView(SingleModel.Model_Door,-60, 50, .6f, 2,false);

                    //});
                }
                else if (!isConnectPower)
                {
                    isConnectPower = true;
                    OpenFXTP();
                }
                break;
            case "_Door":


                break;

            case "ganzaobap":
                if(_tp)
                _tp.Hide();
                InfoPanel.Instance.HideInfo();
                OpenTheDoor();

                break;
            default:
                 InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint, XMLManager.Instance.strConfigDir["NoOnInfoLog"],170);
                //mouseClock = true;
                //MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["NoOnInfoLog"], 350, MyLogger.TextAlign.Center,delegate { mouseClock = false; });

                break;
        }
    }
    public override void Resert()
    {
       // ContinueManager.Instance.StopMyContinue();
        RemoveAll();
       // OnBegin();
    }
    protected override void RemoveAll()
    {
        MyLogger.Instance.HideLog();
        if(_tp)
        _tp.Hide();

        SingleModel.UI_TextInfo.DOPause();
        SingleModel.UI_TextInfo.color = SingleModel.defaltColor;
        SingleModel.Model_Door.DOPause();
        SingleModel.Model_Door.transform.position = Vector3.zero;
        
        // SingleModel.Model_DianXian.transform.position = GameObject.Find("+ Models/FXTP/end").transform.position;
        // SingleModel.UI_TextInfo.text = SingleModel.Close;
        // SingleModel.UI_TextDanWeiInfo.text = SingleModel.Close;
    }
    /// <summary>
    /// 打开开始键 开机
    /// </summary>
    void OpenFXTP()
    {
        GameObject.Find("+ Models/FXTP/Balance_FX/Balance_FX_base_01").GetComponent<Renderer>().
            sharedMaterial.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f));
        SingleModel.UI_TextInfo.text = "- - - - - -";
        SingleModel.UI_TextInfo.DOFade(0, 0.5f).SetLoops(4, LoopType.Yoyo).OnComplete(
            () => {
                SingleModel.UI_TextInfo.text = SingleModel.zero;
                
                SingleModel.UI_TextDanWeiInfo.text = SingleModel.danWei;

                SingleModel.UI_TextInfo.DOFade(1, 0.5f).OnComplete(OnEnd);
               // mouseClock = true;
              //  ContinueManager.Instance.WaitForSecond(0.5f, OnEnd);
                //OnEnd();
            });
       
    }
    TipPanel _tp = null;


    public void ShowTip()
    {
        if (_tp == null)
        {
            GameObject _go =GameObject. Instantiate(Resources.Load("TipPanel")) as GameObject;
            _go.name = "TipPanel";

            RectTransform _tip = _go.GetComponent<RectTransform>();
            _tip.SetParent(GameObject.Find("Canvas/GroupPanel").transform);

            _tip.localScale = Vector3.zero;
            _tip.anchoredPosition = Vector2.zero;
            _tp = _tip.GetComponent<TipPanel>();
        }
        //Invoke("WaitShow", waitTime);
        _tp.ShowTip("干燥包");

        _tp.Follow(Camera.main.WorldToScreenPoint(SingleModel.Model_GanZaoBao.transform.position));

    }
}
