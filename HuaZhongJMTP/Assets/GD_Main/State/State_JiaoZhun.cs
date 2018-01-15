using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using System;
using DG.Tweening;
/// <summary>
/// 校准步骤1
/// </summary>
public class State_JiaoZhun :State_Base {

    /// <summary>
    /// 当移动时触发
    /// </summary>
    Action<Vector3> m_Action;
    RaycastHit hit;
    I_MoveModle currentModle1, currentModle2;
    Tweener mTweener;

    bool readyToJiaoZhun = false;
    private float delayTime=1f;
    public override void OnBegin()
    {
        readyToJiaoZhun = false;
        //InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
         //  "请开始校准操作");

    }
    /// <summary>
    /// 生成砝码
    /// </summary>
    GameObject CreatFaMa(string name ,float posX)
    {

        GameObject obj = Resources.Load(name) as GameObject;

        GameObject beaker = GameObject.Instantiate(obj);
        beaker.name = name;
        beaker.transform.position.Set(posX, 0, 0.04f);
        beaker.transform.eulerAngles.Set(0, 180, 0);
        beaker.transform.localScale.Set(1, 1, 1);
        beaker.transform.SetParent(GameObject.Find("+ Models").transform);
        
        m_Action += beaker.GetComponent<I_MoveModle>().MoveTo;
        return beaker;
    }
    public override void OnEnd()
    {

        if(mTweener!=null)
        mTweener.Pause();
        SingleModel.UI_TextInfo.text = "0.000";
        SingleModel.UI_TextInfo.DOFade(1, 0.5f);
        GameObject.Destroy(currentModle1.gameObject);
        GameObject.Destroy(currentModle2.gameObject);
        if (_CallBack != null)
        {
            _CallBack(1);
        }
    }

    public override void OnUpdate()
    {
        if (readyToJiaoZhun)
        {
            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10f, LayerMask.GetMask("taimian")))
                {
                    if(m_Action!=null)
                    m_Action(hit.point);
                }
            }
            //松开鼠标时 将当前要移动的物体变为null
            if (Input.GetMouseButtonUp(0))
            {
                //currentModle1 = null;
            }
        }
    }
    /// <summary>
    /// 开门操作
    /// </summary>
    void OpenTheDoor()
    {
        SingleModel.Model_Door.DOMove(Vector3.forward * .15f, 2).OnComplete(() => { });
    }
    /// <summary>
    /// 关门
    /// </summary>
    void CloseTheDoor()
    {
        SingleModel.Model_Door.DOMove(Vector3.zero, 2).OnComplete(() => {
            MyLogger.Instance.Log(XMLManager.Instance.clickConfigDir["AppJiaoZhunOverInfoLog"], 350, MyLogger.TextAlign.Center, delegate
            {
                OnEnd();
            });
           
        });
     }
    /// <summary>
    /// 屏幕显示变化
    /// </summary>
    void ViewAnimation()
    {
    }

    public override void ButtonOnClick(string buttonName)
    {
        switch (buttonName)
        {
            case "_Cal":

                if (readyToJiaoZhun)
                {
                    return;
                }
                break;
            case "_TareR":
            case "_TareL":
                //提示无可去皮操作
                InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
         XMLManager.Instance.strConfigDir["NoWayToRemoveInfoLog"]);
                break;
        }
    }

    public override void ButtonOnDrag(string buttonName)
    {
        switch (buttonName)
        {
            case "_Cal":
                //已经处在校准状态了
                if (readyToJiaoZhun)
                {
                    return;
                }
                  SingleModel.UI_LoadingCal.fillAmount += Time.deltaTime/delayTime;
                if (SingleModel.UI_LoadingCal.fillAmount >= 1f)
                {
                    SingleModel.UI_TextInfo.text = "Cal";

                    readyToJiaoZhun = true;
                }
                break;
            case "_Power":
                SingleModel.UI_LoadingPower.fillAmount += Time.deltaTime / delayTime;
                if (SingleModel.UI_LoadingPower.fillAmount >= 1f)
                {
                   
                    SingleModel.UI_LoadingPower.fillAmount = 0;
                    RemoveAll();
                    FXTPMainManager.Instance.BackOnState();
                }
                break;
        }
    }

    public override void ButtonOnUp(string buttonName)
    {
        switch (buttonName)
        {
            case "_Cal":
                //如果已经处在校准的模式 提示操作者
                if(SingleModel.UI_LoadingCal.fillAmount < 0.5f&& readyToJiaoZhun)
                {
                    InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
                                     XMLManager.Instance.strConfigDir["CheckButtonInfoLog"]);
                    SingleModel.UI_LoadingCal.fillAmount = 0;
                    return;
                }
                if (readyToJiaoZhun)
                {
                    SingleModel.UI_TextInfo.text = "200.000";
                    mTweener= SingleModel.UI_TextInfo.DOFade(0, 0.5f).SetLoops(-1, LoopType.Yoyo);
                    InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
                   XMLManager.Instance.strConfigDir["WrongWeightInfoLog"]);

                    currentModle2= CreatFaMa("weight_100g",0.2f).GetComponent<I_MoveModle>();
                    currentModle1 = CreatFaMa("weight_200g", 0.4f).GetComponent<I_MoveModle>();
                    currentModle1._CallBack+=CloseTheDoor;
                    
                    OpenTheDoor();
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
        SingleModel.UI_TextInfo.text = "0.000";
        SingleModel.UI_TextInfo.DOFade(1, 0.5f);
        SingleModel.Model_Door.position = Vector3.zero;
        if (currentModle1 != null)
            GameObject.Destroy(currentModle1.gameObject);
        if (currentModle2 != null)
            GameObject.Destroy(currentModle2.gameObject);
        if (mTweener != null)
            mTweener.Pause();
    }
    /// <summary>
    /// 初始化工作
    /// </summary>
    public override void Resert()
    {

        RemoveAll();
        OnBegin();
        
    }
}
