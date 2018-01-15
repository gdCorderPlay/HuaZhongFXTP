using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using System;
using DG.Tweening;

/// <summary>
/// 测量步骤
/// </summary>
public class State_CeLiang : State_Base
{
    public static float targetWeight=0;
   
    /// <summary>
    /// 记录们是否是打开的
    /// </summary>
    bool isOpen = false;
    /// <summary>
    /// 当移动时触发
    /// </summary>
    Action<Vector3> m_Action;
    /// <summary>
    /// 杯子是否已经进入天平
    /// </summary>
    bool beakerInTP = false;

    /// <summary>
    /// 符合标准的烧杯
    /// </summary>
    I_MoveModle modleBeaker;
    RaycastHit hit;
    string tipeInfo,desInfo;
   
    public override void OnBegin()
    {
       
        mouseClock = true;
        targetWeight = 0;
          
            InputerPanel.Instance.ShowInputer(delegate(float value)
            {
                targetWeight =value;
                if (targetWeight <= 0||targetWeight>200)
                {
                   // mouseClock = true;
                    //提示输入不合法
                    MyLogger.Instance.Log(XMLManager.Instance.clickConfigDir["WrongRangeInfoLog"], 350, MyLogger.TextAlign.Center, OnBegin);
                }
                else
                {
                    CameraViewer.Instance.ChangeCameraView(Vector3.up * 0.15f, 0, 21, .9f, 1, delegate
                    {
                        
                        return true;
                    });
                    Init();
                }
            });
    }
    /// <summary>
    /// 一些初始化的操作
    /// </summary>
    void Init()
    {
        beakerInTP = false; 
        mouseClock = false;                                      //鼠标锁住无法拖拽物体 
        SingleModel.UI_TextInfo.text = SingleModel.zero;        //面板显示0.000
        SingleModel.UI_TextDanWeiInfo.text = SingleModel.danWei; //单位显示 g
        
        
        GlobalUIManager.Instance.SetMainStepTitle(targetWeight); //变更顶部提示信息


        //I_MoveModle beaker1 = CreatModle("beaker_250ml", new Vector3(0.23f, 0, -0.04f));//实例化烧杯
        //I_MoveModle beaker2 = CreatModle("beaker_500ml", new Vector3(0.23f, 0, 0.05f));
        //I_MoveModle beaker3 = CreatModle("beaker_1000ml", new Vector3(0.23f, 0, 0.15f));

        I_MoveModle beaker1 = CreatModle("beaker_250ml", new Vector3(0.4f, 0.001f, 0.27f));//实例化烧杯
        I_MoveModle beaker2 = CreatModle("beaker_500ml", new Vector3(0.31f, 0.001f, 0.27f));
        I_MoveModle beaker3 = CreatModle("beaker_1000ml", new Vector3(0.2f, 0.001f, 0.27f));
        beaker1.transform.DOMoveX(0.6f, 1).From(); //控制烧杯做一小段位移
      //  beaker1.transform.DOScale(Vector3.one, 1f);
        beaker2.transform.DOMoveX(0.45f, 1).From();
        //beaker2.transform.DOScale(Vector3.one, 1f);
        beaker3.transform.DOMoveX(0.3f, 1).From();
       // beaker3.transform.DOScale(Vector3.one, 1f);
        m_Action += beaker1.MoveTo;
        m_Action += beaker2.MoveTo;
        m_Action += beaker3.MoveTo;

        if (targetWeight < 10)
        {
            modleBeaker = beaker1;
            modleBeaker._CallBack += CallBack;
            tipeInfo = "请选择250ml的烧杯";
            desInfo = "250ml烧杯适用于10克以下药剂的称量";
        } //选择合适的烧杯"250ml适用于10克以下的称量"
        else if (targetWeight < 50)
        {
            modleBeaker = beaker2;
            modleBeaker._CallBack += CallBack;
            tipeInfo = "请选择500ml的烧杯";
            desInfo = "500ml烧杯适用于10至50克药剂的称量";
        }
        else
        {
            modleBeaker = beaker3;
            modleBeaker._CallBack += CallBack;
            tipeInfo = "请选择1000ml的烧杯";
            desInfo = "1000ml烧杯适用于50克以上药剂的称量";
        }
       
        tipeInfo = string.Format("{0}{1}{2}{3}", XMLManager.Instance.clickConfigDir["RandomWeightInfoLog"], targetWeight,"g", tipeInfo);

        //mouseClock = true;
        //MyLogger.Instance.Log( tipeInfo, 350, MyLogger.TextAlign.Center, delegate
        //{
        //    mouseClock = false;
        //});
        InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint, tipeInfo,350);

    }

    public override void OnEnd()
    {
        RemoveAll();
        if (_CallBack!=null)
        _CallBack(1);
    }

    
    public override void OnUpdate()
    {
        if (mouseClock) { return; }
        //if()

            if (Input.GetMouseButton(0))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10f,LayerMask.GetMask("taimian") ))
                {
                    if(m_Action!=null)
                    m_Action(hit.point);
                }
            }
           
        
    }
    /// <summary>
    /// 碰撞发生后的回调方法
    /// </summary>
    void CallBack()
    {
        
        if (isOpen)
        {
            mouseClock = true;
            InfoPanel.Instance.ShowInfo(modleBeaker.transform.Find("ShowPoint"), desInfo, 350,delegate
            {
                mouseClock = false;
                modleBeaker.GetComponent<Collider>().enabled = false;
                mouseClock = true;
                modleBeaker.transform.DOMove(modleBeaker.transform.position + Vector3.up * .1f, 1)
                    .OnComplete(() =>
                    {
                        modleBeaker.transform.DOMove(new Vector3(0, 0.1f, 0.03f), 1f).OnComplete(() =>
                        {
                        //显示重量
                        SingleModel.UI_TextInfo.text = "15.000";
                            beakerInTP = true;
                        //关门
                        CloseTheDoor();
                        });
                    });

            });
           
        }
        else
        {
            //  InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,XMLManager.Instance.strConfigDir["NoOpenInfoLog"] );
            // mouseClock = true;
            // MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["NoOpenInfoLog"], 350, MyLogger.TextAlign.Center,delegate { mouseClock = false; });
            mouseClock = true;
            InfoPanel.Instance.ShowInfo(modleBeaker.transform.Find("ShowPoint"), desInfo, 350, delegate {
                mouseClock = false;
                CameraViewer.Instance.ChangeCameraView(Vector3.up * 0.15f, -35, 35, .9f, 1, delegate { return true; });
                modleBeaker.GetComponent<Collider>().enabled = false;
                SingleModel.Model_Door.DOMove(Vector3.forward * .15f, 1).SetDelay(1f).OnComplete(() => {
                    isOpen = true; //mouseClock = false;


                    //  mouseClock = true;
                    modleBeaker.transform.DOMove(modleBeaker.transform.position + Vector3.up * .1f, 1)
                        .OnComplete(() =>
                        {
                            modleBeaker.transform.DOMove(new Vector3(0, 0.1f, 0.03f), 1f).OnComplete(() =>
                            {
                            //显示重量
                            SingleModel.UI_TextInfo.text = "15.000";
                                beakerInTP = true;
                            //关门
                            CloseTheDoor();
                            });
                        });
                });
            });

          
           
        }
    }
    /// <summary>
    /// 打开天平的门
    /// </summary>
    void OpenTheDoor()
    {
        mouseClock = true;
        SingleModel.Model_Door.DOMove(Vector3.forward * .15f, 1).OnComplete(()=> { isOpen = true;mouseClock = false; });
        //更改提示信息
    }
    /// <summary>
    /// 关门
    /// </summary>
    void CloseTheDoor()
    {
       // mouseClock = true;
        SingleModel.Model_Door.DOMove(Vector3.zero, 1).OnComplete(() => {
           // mouseClock = false;
            isOpen = false;
            if (beakerInTP)
            {
                OnEnd(); }
            });
    }
    protected override void RemoveAll()
    {
        SingleModel.Model_Door.DOPause();
        modleBeaker.DOPause();
        SingleModel.Model_Door.position = Vector3.zero;
        isOpen = false;
        I_MoveModle[] modles = GameObject.FindObjectsOfType<I_MoveModle>();
        for(int i = 0; i < modles.Length; i++)
        {
            GameObject.Destroy(modles[i].gameObject);
        }
    }
    public override void Resert()
    {
       // GlobalUIManager.Instance.SetMainStepTitle();
        RemoveAll();
       // OnBegin();
    }
    /// <summary>
    /// 生成模型
    /// </summary>
    /// <returns></returns>
    I_MoveModle CreatModle(string name, Vector3 pos)
    {
        GameObject obj = Resources.Load(name) as GameObject;

        GameObject beaker = GameObject.Instantiate(obj);
        beaker.name = name;
        beaker.transform.position = pos;
        beaker.transform.eulerAngles.Set(0, 180, 0);
        beaker.transform.localScale.Set(1, 1, 1);
        beaker.transform.SetParent(GameObject.Find("+ Models").transform);
        return beaker.GetComponent<I_MoveModle>();
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
                   // RemoveAll();
                    SingleModel.UI_LoadingPower.fillAmount = 0;
                    FXTPMainManager.Instance.BackOnState();
                }
                break;
        }
    }

    public override void ButtonOnClick(string buttonName)
    {
        if (mouseClock) return;

        InfoPanel.Instance.HideInfo();
        switch (buttonName)
        {
            case "_TareR":
            case "_TareL":
                if (beakerInTP)
                {
                    //去皮完成
                    SingleModel.UI_TextInfo.text = SingleModel.zero;
                    beakerInTP = false;
                }
                else
                {
                    //提示不需要进行去皮操作
                    InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
                    XMLManager.Instance.strConfigDir["NoWayToRemoveInfoLog"],160);
                   // mouseClock = true;
                  //  MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["NoWayToRemoveInfoLog"], 350, MyLogger.TextAlign.Center,delegate { mouseClock = false; });


                }
                break;
            case "_Cal":
                //如果已经处在校准的模式 提示操作者
                InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
                                 XMLManager.Instance.strConfigDir["CheckButtonInfoLog"],280);
               // mouseClock = true;
                //MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["CheckButtonInfoLog"], 350, MyLogger.TextAlign.Center,delegate { mouseClock = false; });
                break;
            case "_Door":
                if (isOpen)
                {
                    CloseTheDoor();
                }
                else
                {
                    OpenTheDoor();
                }
                break;
        }
       
    }
}
