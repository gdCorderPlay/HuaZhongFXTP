using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using System;
using DG.Tweening;
using UnityEngine.UI;
public class State_JiaoZhun2 : State_Base {

    /// <summary>
    /// 当移动时触发
    /// </summary>
    Action<Vector3> m_Action;
    RaycastHit hit;
    I_MoveModle currentModle1, currentModle2;
   
    bool _DoorClose;
    bool _FaMaReady = false;

   
    private float delayTime = 1f;

    public override void OnBegin()
    {
       // stateEnd = false;
        mouseClock = false;
        _DoorClose = true;
        _FaMaReady = false;
        SingleModel.UI_TextInfo.text = "200.000";
        SingleModel.UI_TextDanWeiInfo.text = SingleModel.danWei;
        SingleModel.UI_TextInfo.DOFade(0, 0.5f).SetLoops(-1, LoopType.Yoyo);
        InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
       XMLManager.Instance.strConfigDir["WrongWeightInfoLog"],390);

        //  currentModle2 = CreatFaMa("weight_100g", Vector3.right*0.2f).GetComponent<I_MoveModle>();
        //currentModle1 = CreatFaMa("weight_200g", Vector3.right*0.16f).GetComponent<I_MoveModle>();
        currentModle2 = CreatFaMa("weight_100g", new Vector3(0.29f,0,0.05f)).GetComponent<I_MoveModle>();
        currentModle2.transform.DOMoveX(0.40f, 1).From();
        currentModle2.transform.DOScale(Vector3.one, 1f);
        currentModle1 = CreatFaMa("weight_200g", new Vector3(0.29f,0,0.12f)).GetComponent<I_MoveModle>();
        //正确的砝码被放入天平中时记录下来
        currentModle1.transform.DOMoveX(0.40f, 1).From();
        currentModle1.transform.DOScale(Vector3.one, 1f);
        currentModle1._CallBack +=  MoveFaMa;
        currentModle1.transform.GetChild(0).gameObject.SetActive(false);
       // CameraViewer.Instance.ChangeCameraView(SingleModel.Model_Door, -30, 45, 0.5f, 2,false);
    }
   
    /// <summary>
    /// 生成砝码
    /// </summary>
    GameObject CreatFaMa(string name, Vector3 pos)
    {
        GameObject obj = Resources.Load(name) as GameObject;
        GameObject beaker = GameObject.Instantiate(obj);
        beaker.name = name;
        beaker.transform.position=pos;
       // beaker.transform.eulerAngles.Set(0, 180, 0);
        beaker.transform.localScale.Set(0, 0, 0);
        beaker.transform.SetParent(GameObject.Find("+ Models").transform);

        m_Action += beaker.GetComponent<I_MoveModle>().MoveTo;
        return beaker;
    }
    public override void OnEnd()
    {
        RemoveAll();
        if (_CallBack != null)
        {
            _CallBack(1);
        }
    }

    void MoveFaMa()
    {
        //if (_DoorClose)
       // {
            //提示门没有打开
           // mouseClock = true;
           // MyLogger.Instance.Log( XMLManager.Instance.strConfigDir["NoOpenInfoLog"], 350, MyLogger.TextAlign.Center,delegate { mouseClock = false; });

         //   return;
       // }
       // mouseClock = true;
        
       // MyLogger.Instance.Log(XMLManager.Instance.clickConfigDir["WrongMoveFaMaInfoLog"], 350, MyLogger.TextAlign.Center, delegate
       InfoPanel.Instance.ShowInfo(currentModle1.transform.Find("showPoint"), 
           XMLManager.Instance.clickConfigDir["WrongMoveFaMaInfoLog"],350,delegate  //提示需要使用镊子操作
        {
            currentModle1.GetComponent<Collider>().enabled = false;
            CameraViewer.Instance.ChangeCameraView(Vector3.up * 0.15f, -42, 25, 0.6f, 0.5f, delegate
            {
               // mouseClock = false;
                SingleModel.Model_Door.DOMove(Vector3.forward * .15f, 1).OnComplete(() => {

                    //使用镊子移动砝码
                    currentModle1.transform.GetChild(0).gameObject.SetActive(true);
                    currentModle1.transform.DOMove(currentModle1.transform.position + Vector3.up * .1f, 1)
                    .OnComplete(() => {
                        currentModle1.transform.DOMove(new Vector3(0, 0.1f, 0.03f), 1f).
                          OnComplete(() => {
                              _FaMaReady = true;
                              mouseClock = false;
                              currentModle1.transform.GetChild(0).gameObject.SetActive(false);

                              CloseTheDoor();
                          });
                    });
                });

                return true;
            });
           
        });
    }
    public override void OnUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            
            if (mouseClock) return;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10f, LayerMask.GetMask("taimian")))
                {    
                    if (m_Action != null)
                        m_Action(hit.point);
                }
        }
    }
    /// <summary>
    /// 开门操作
    /// </summary>
    void OpenTheDoor()
    {
        mouseClock = true;
        SingleModel.Model_Door.DOMove(Vector3.forward * .15f, 1).OnComplete(() => {mouseClock=false; _DoorClose = false; });
    }
    /// <summary>
    /// 关门操作
    /// </summary>
    void CloseTheDoor()
    {
        mouseClock = true;
        SingleModel.Model_Door.DOMove(Vector3.zero, 1).OnComplete(() => {
            CameraViewer.Instance.ChangeCameraView(0, 10, 0.5f, 0.5f, false);
            mouseClock = false;
            _DoorClose = true;
            if (_DoorClose && _FaMaReady)
            {
                //stateEnd = true;// 开始结束状态
                //CameraViewer.Instance.ChangeCameraView(SingleModel.Model_Door, 0, 66, 0.5f, 2, false);
                SingleModel.UI_TextInfo.text =SingleModel.zero;

                SingleModel.UI_TextInfo.DOFade(0, 0.5f).SetLoops(4, LoopType.Yoyo).OnComplete(()=> {
                    // if (!stateEnd) return;
                    // mouseClock = true;
                    // MyLogger.Instance.Log(XMLManager.Instance.clickConfigDir["AppJiaoZhunOverInfoLog"], 350, MyLogger.TextAlign.Center,
                    //   delegate {
                    //     mouseClock = false;
                    //if (currentModle1 != null)
                    //    GameObject.Destroy(currentModle1.gameObject);

                    I_MoveModle[] modles = GameObject.FindObjectsOfType<I_MoveModle>();
                    for (int i = 0; i < modles.Length; i++)
                    {
                        GameObject.Destroy(modles[i].gameObject);
                    }

                    ViewAnimation();
                     //  });
                  
                });
                   
            }
        });
    }
    /// <summary>
    /// 屏幕显示变化
    /// </summary>
    void ViewAnimation()
    {

        //  SingleModel.UI_TextInfo.DOFade(1, 0f);
        SingleModel.UI_TextInfo.DOPause();
        SingleModel.UI_TextInfo.color = SingleModel.defaltColor;
        SingleModel.UI_TextInfo.text = "CAL done";
        SingleModel.UI_TextDanWeiInfo.text = SingleModel.Close;
        SingleModel.UI_TextInfo.DOFade(0, 0.5f).SetLoops(4, LoopType.Yoyo).OnComplete(()=> {
           // if (!stateEnd) return;
            SingleModel.UI_TextInfo.text = SingleModel.zero;
            SingleModel.UI_TextDanWeiInfo.text = SingleModel.danWei;
            SingleModel.UI_TextInfo.DOFade(0, 0.5f).SetLoops(4, LoopType.Yoyo).OnComplete(()=> {
               // if (!stateEnd) return;
                OnEnd(); });
        });
        

    }

    public override void ButtonOnClick(string buttonName)
    {
        if (mouseClock) return;
        InfoPanel.Instance.HideInfo();
        switch (buttonName)
        {
            case "_Cal":
                //       提示已经处在校准状态中了
                    InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
                XMLManager.Instance.strConfigDir["CheckButtonInfoLog"],280);
               // mouseClock = true;
               // MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["CheckButtonInfoLog"], 350, MyLogger.TextAlign.Center,delegate { mouseClock = false; });
                break;
            case "_TareR":
            case "_TareL":
                //提示无可去皮操作
                //  InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
                //XMLManager.Instance.strConfigDir["NoWayToRemoveInfoLog"]);
                FXTPMainManager.Instance.ChangeNextState(-1, GlobalUIManager.STEP_CONTROLL_STATE.Auto);
              //  mouseClock = true;
               MyLogger.Instance.Log(XMLManager.Instance.clickConfigDir["WrongRemoveInfoLog"], 350, MyLogger.TextAlign.Center,
                              delegate
                              {
                                //  mouseClock = false;
                                 // RemoveAll();
                                  
                              }
                              );
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
           
                
            case "_Power":
                SingleModel.UI_LoadingPower.fillAmount += Time.deltaTime / delayTime;
                if (SingleModel.UI_LoadingPower.fillAmount >= 1f)
                {

                    SingleModel.UI_LoadingPower.fillAmount = 0;
                   // RemoveAll();
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
            case "_Power":
                SingleModel.UI_LoadingPower.fillAmount = 0;
                break;
        }
    }

    protected override void RemoveAll()
    {
        MyLogger.Instance.HideLog();
        SingleModel.UI_TextInfo.DOPause();
        //SingleModel.UI_TextInfo.DOFade(1, 0f);
        SingleModel.UI_TextInfo.color = SingleModel.defaltColor;
        SingleModel.Model_Door.DOPause();
        SingleModel.Model_Door.position = Vector3.zero;

        I_MoveModle[] modles = GameObject.FindObjectsOfType<I_MoveModle>();
        for (int i = 0; i < modles.Length; i++)
        {
            GameObject.Destroy(modles[i].gameObject);
        }

    }
    /// <summary>
    /// 初始化工作
    /// </summary>
    public override void Resert()
    {
       // stateEnd = false;//结束关闭的动画
        RemoveAll();
       // OnBegin();

    }
}
