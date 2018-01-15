using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using DG.Tweening;
/// <summary>
/// 连接电源
/// </summary>
public class State_LinkPower : State_Base
{
    /// <summary>
    /// 移动的目的点
    /// </summary>
    private Vector3 target;
    //刚进入时 
    public override void OnBegin()
    {
        GameObject.Find("+ Models/FXTP/Balance_FX/Balance_FX_base_01").GetComponent<Renderer>().sharedMaterial.
            SetColor("_EmissionColor", new Color(0, 0, 0));
        SingleModel.Model_DianXian.position = new Vector3(0.15f, 0.09f, 0.45f);
       // CameraViewer.Instance.ChangeCameraView(-27, 25, 0.83f, 0,false);
        MyLogger.Instance.Log(XMLManager.Instance.clickConfigDir["NoPowerInfoLog"], 350, MyLogger.TextAlign.Center, delegate
        {
            mouseClock = true;
            CameraViewer.Instance.ChangeCameraView(SingleModel.Model_FXTP, -20, 20, 0.7f, 1, delegate {mouseClock=false; MoveTheElectricWire();return true; });
           
        });
    }

    /// <summary>
    /// 电线移动的动画
    /// </summary>
    void MoveTheElectricWire()
    {
        
        target = GameObject.Find("+ Models/FXTP/end").transform.position;
        mouseClock = true;
        SingleModel.Model_DianXian.DOMove(target, 1.5f).OnComplete(OnEnd);
       // return true;
    }
    /// <summary>
    /// 结束时要执行的方法
    /// </summary>
    public override void OnEnd()
    {
        mouseClock = false;
        if (_CallBack != null)
        {
            _CallBack(1);
        }
    }

    protected override void RemoveAll()
    {
        MyLogger.Instance.HideLog();
        SingleModel.Model_DianXian.DOPause();
       
    }

    public override void Resert()
    {
        RemoveAll();
    }
}
