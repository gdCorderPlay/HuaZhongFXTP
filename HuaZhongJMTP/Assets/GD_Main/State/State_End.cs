using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;
/// <summary>
/// 实验结束步骤
/// </summary>
public class State_End : State_Base
{
    Transform pingGai;
    enum DoorState { Open, Close }
    /// <summary>
    /// 需要加药的次数
    /// </summary>
    float count = 3;
    /// <summary>
    /// 是否已经准备完成
    /// </summary>
    bool isOk = false;
    /// <summary>
    /// 当移动时触发
    /// </summary>
    Action<Vector3> m_Action;
    /// <summary>
    /// 是否需要去皮
    /// </summary>
    enum RemoveState { 等待去皮, 无需去皮, 禁止去皮 }
    /// <summary>
    /// 当前的去皮操作
    /// </summary>
    RemoveState currentRemoveState;
    /// <summary>
    /// 当前的门的状态
    /// </summary>
    DoorState currentDoorState;
    /// <summary>
    /// 获取到药勺和药瓶
    /// </summary>
    I_MoveModle modleYaoShao, modleYaoPing, modleBeaker;
    RaycastHit hit;
    Vector3 target;
    float delayTime;
    private ParticleSystem _paryicle;

    /// <summary>
    /// 开始时执行
    /// </summary>
    public override void OnBegin()
    {
        //Init();
        //JiaYaoDongHua2();
        GlobalUIManager.Instance.SetMainStepTitle(State_CeLiang.targetWeight);//复原提示信息
        mouseClock = false;
        // CameraViewer.Instance.ChangeCameraView(SingleModel.Model_FXTP, -30, 60, 1f, 2, false); //切换视角
        if (State_CeLiang.targetWeight <= 0)
        {
            mouseClock = true;
            InputerPanel.Instance.ShowInputer(delegate (float value)
            {
                State_CeLiang.targetWeight = (int)value;
                if (State_CeLiang.targetWeight <= 0 || State_CeLiang.targetWeight >= 200)
                {
                    //提示输入不合法
                    // mouseClock = true;
                    MyLogger.Instance.Log(XMLManager.Instance.clickConfigDir["WrongRangeInfoLog"], 350, MyLogger.TextAlign.Center, OnBegin);
                }
                else { Init(); }
            });
        }
        else
        {
            Init();
        }
    }
    /// <summary>
    /// 新版动画
    /// </summary>
    void JiaYaoDongHua2()
    {
        
        mouseClock = true;
        modleYaoShao.GetComponent<Collider>().enabled = false;
        SingleModel.Model_DoorTop.DOMoveZ(0.15f, 1f).OnComplete(
            delegate
            {//开门后
                pingGai = modleYaoPing.transform.GetChild(1);//获取瓶盖
                pingGai.DOMoveY(pingGai.position.y + 0.1f, 1).OnComplete(
               delegate//瓶盖上升后
               {

                   pingGai.gameObject.SetActive(false);//关闭瓶盖
                   modleYaoShao.transform.DORotate(new Vector3(0, 0, 50), 1);

                   //target = new Vector3(modleYaoPing.transform.position.x + 0.04f, modleYaoPing.transform.position.z, modleYaoShao.transform.position.y + 0.13f);
                   modleYaoShao.transform.DOMoveX(modleYaoPing.transform.position.x + 0.04f, 1.2f);
                   modleYaoShao.transform.DOMoveY(modleYaoShao.transform.position.y + 0.15f, 1.2f).
                   // delayTime = Vector3.Distance(target, modleYaoShao.transform.position) * 2f;
                   //
                   // modleYaoShao.transform.DOMove(target,1f).//药勺第一段上升动画

                   OnComplete(
                   delegate
                   {
                      
                               modleYaoShao.transform.DOMove(modleYaoPing.transform.GetChild(2).position, 0.8f).OnComplete(//药勺向瓶子中取药的动画
                              delegate
                              {
                                  modleYaoShao.transform.GetChild(1).localScale = Vector3.one;
                                  modleYaoShao.transform.DOMoveY(0.28f, 1f).OnComplete(delegate
                                  {//上升
                                      modleYaoShao.transform.DOMove(new Vector3(0, 0.28f, 0.033f), 1f).OnComplete(//移动到瓶子的正上方
                                          delegate
                                          {
                                              modleYaoShao.transform.DOMoveY(0.213f, 0.8f).OnComplete( //移动到加药点
                                                  delegate
                                                  {
                                                      JiaYao(); //加药
                                                  });
                                          });
                                  });
                                  #region 曲线加药动画

                                  //Vector3[] path = new Vector3[]
                                  //{

                                  //   //new Vector3(modleYaoPing.transform.position.x+ Mathf.Clamp(0-modleYaoPing.transform.position.x ,-0.05f ,0.05f),

                                  //   // 0.3f,modleYaoPing.transform.position.z+  Mathf.Clamp(0.033f-modleYaoPing.transform.position.z ,-0.05f ,0.05f)),
                                  //   (modleYaoShao.transform.position+Vector3.up*0.1f)
                                  //    ,

                                  //    new Vector3(Mathf.Clamp(modleYaoPing.transform.position.x,-0.07f ,0.07f) ,0.3f,
                                  //   0.033f+Mathf.Clamp(modleYaoPing.transform.position.z-0.033f,-0.075f ,0.075f)),

                                  //    new Vector3(0,0.213f,0.033f)
                                  //};
                                  // modleYaoShao.transform.DOPath(path, 3f, PathType.CatmullRom).OnComplete(
                                  //delegate
                                  //{
                                  //   JiaYao();

                                  //});
                                  #endregion
                              });
                   });
               });
            });

        GameObject obj = Resources.Load("QiongZhiTangFen") as GameObject;
        GameObject salt = GameObject.Instantiate(obj, modleBeaker.transform);

        salt.transform.localScale = Vector3.zero;
        salt.transform.localPosition = Vector3.zero;
    }
    /// <summary>
    /// 结束时执行 
    /// </summary>
    public override void OnEnd()
    {
        
        mouseClock = false;
        CameraViewer.Instance.ChangeCameraView(-48, 26, 0.9f, 1f, false);
        //提示实验结束
        SingleModel.UI_TextInfo.text = SingleModel.Close;
        SingleModel.UI_TextDanWeiInfo.text = SingleModel.Close;
        GameObject.Find("+ Models/FXTP/Balance_FX/Balance_FX_base_01").GetComponent<Renderer>().sharedMaterial.
              SetColor("_EmissionColor", new Color(0, 0, 0));
        // OpenTheDoor();
        //Transform pingGai = modleYaoPing.transform.GetChild(1);

        // modleYaoPing.transform.GetChild(1).DOMoveY(modleYaoPing.transform.GetChild(1).position.y + 0.1f, 1).SetDelay(1f).OnComplete(()=> { });

        
        SingleModel.Model_DianXian.DOMove(new Vector3(0.15f, 0.09f, 0.45f), 1f).SetDelay(1f).OnComplete(() =>
        {
            SingleModel.Model_GanZaoBao.SetActive(true);
            SingleModel.Model_GanZaoBao.transform.position = new Vector3(0.2f, 0.11f, 0.09f);
            // SingleModel.Model_GanZaoBao.transform.position = new Vector3(0.008f, 0.1f, 0.029f);
            // modleBeaker.transform.DOMoveX(0.3f, 1f).SetDelay(2f).OnComplete(() => { GameObject.Destroy(modleBeaker.gameObject); });
            // SingleModel.Model_GanZaoBao.transform.DOMove(new Vector3(0, .1f, .09f), 2f).SetDelay(1f).OnComplete(() => CloseTheDoor());
            SingleModel.Model_GanZaoBao.transform.DOMove(new Vector3(0.008f, 0.1f, 0.029f), 1f).OnComplete(
                delegate
                {
                    SingleModel.Model_Door.DOMove(Vector3.zero, 2).OnComplete(

                        delegate
                        {
                            if (modleBeaker)
                            {
                                GameObject.Destroy(modleBeaker.gameObject);
                            }
                            mouseClock = true;
                            MyLogger.Instance.Log(XMLManager.Instance.clickConfigDir["AppPlayOverInfoLog"], 350, MyLogger.TextAlign.Center, delegate
                            {
                                mouseClock = false;
                                GlobalUIManager.stepIndex = 0;
                                GlobalUIManager.stepSubIndex = 0;

                                SceneManager.LoadScene("FXTPGD");
                            });
                        }
                        );

                }
                  //() => CloseTheDoor()
                
                );
            //提示实验完成
           

        });
    }
    /// <summary>
    /// 实时更新的方法
    /// </summary>
    public override void OnUpdate()
    {

        if (Input.GetMouseButton(0))
        {

            if (mouseClock)
            { return; }
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10f, LayerMask.GetMask("taimian")))
            {
                if (m_Action != null)
                    m_Action(hit.point);
            }
        }
    }
    /// <summary>
    /// 碰撞发生后的回调方法
    /// </summary>
    void CallBack()
    {
        InfoPanel.Instance.HideInfo();
        //if (needRemove)
        //{
        //    InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint, XMLManager.Instance.strConfigDir["NoRemoveInfoLog"]);
        //    // mouseClock = true;
        //    // MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["NoRemoveInfoLog"], 350, MyLogger.TextAlign.Center ,delegate { mouseClock = false; });

        //    return;
        //}
        string showInfo = string.Format("{0}{1}{2}", XMLManager.Instance.clickConfigDir["RandomWeightInfoLog"], State_CeLiang.targetWeight, "g");
       // mouseClock = true;
       // MyLogger.Instance.Log(showInfo, 350, MyLogger.TextAlign.Center, delegate //弹窗提示当前的需要加药的量
     // {
          // CameraViewer.Instance.ChangeCameraView(modleYaoPing.transform, -10, 25, 1f, 1,false);
          //碰到药瓶时触发视角的转换
          //  CameraViewer.Instance.ChangeCameraView(GameObject.Find("Point").transform, -16, 12, 0.8f, 1f);
        //  mouseClock = false;
          JiaYaoDongHua2();
      //});

    }
    /// <summary>
    /// 打开天平的门
    /// </summary>
    void OpenTheDoor()
    {
        isOk = false;
        mouseClock = true;
        SingleModel.Model_Door.DOMove(Vector3.forward * .15f, 2).OnComplete(() =>
        {
            isOk = true;
            mouseClock = false;
            currentDoorState = DoorState.Open;
        });
        //更改提示信息
    }
    /// <summary>
    /// 关门
    /// </summary>
    void CloseTheDoor()
    {
        mouseClock = true;
        isOk = false;
        SingleModel.Model_Door.DOMove(Vector3.zero, 2).OnComplete(() =>
        {
            currentDoorState = DoorState.Open;
            isOk = true;
            mouseClock = false;
        });


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
    /// <summary>
    /// 产生药品 
    /// </summary>
    void Init()
    {
        
        {
            //_paryicle = GameObject.Find("particle").GetComponent<ParticleSystem>();
            // _paryicle = GameObject.Find("YaoFenLiquid").GetComponent<ParticleSystem>();
            _paryicle = GameObject. Instantiate(Resources.Load("YaoFenLiquid") as GameObject).GetComponent<ParticleSystem>();
        }
        _paryicle.gameObject.SetActive(false);
        count = 3; //需要的加药次数
        GlobalUIManager.Instance.SetMainStepTitle(State_CeLiang.targetWeight);
        // Debug.Log("测量目标值"+State_CeLiang.targetWeight);
        mouseClock = false; //解锁鼠标可以拖动

        currentRemoveState = RemoveState.等待去皮; //需要去皮
        SingleModel.UI_TextInfo.text = "15.000"; //显示信息
        SingleModel.UI_TextDanWeiInfo.text = SingleModel.danWei;

        if (State_CeLiang.targetWeight < 10)
        {
            modleBeaker = CreatModle("beaker_250ml", new Vector3(0, 0.1f, 0.03f));
        }// 根据 条件创建相应的烧杯
        else if (State_CeLiang.targetWeight < 50)
        {
            modleBeaker = CreatModle("beaker_500ml", new Vector3(0, 0.1f, 0.03f));
        }
        else
        {
            modleBeaker = CreatModle("beaker_1000ml", new Vector3(0, 0.1f, 0.03f));
        }
        modleBeaker.GetComponent<Collider>().enabled = false;

        currentDoorState = DoorState.Close; //当前的门的开启状态
        modleYaoShao = CreatModle("YaoShao", new Vector3(0.158f, 0.01f, 0.021f));
        modleYaoShao.transform.DOMoveX(0.25f, 1).From();
        modleYaoShao.transform.DOScale(Vector3.one, 1f);
        m_Action += modleYaoShao.MoveTo;
        modleYaoPing = CreatModle("QiongZhiTangGanFenGuan", new Vector3(0.2f, 0, 0.1f));
        modleYaoPing.transform.DOMoveX(0.3f, 1).From();
        modleYaoPing.transform.DOScale(Vector3.one, 1f);
        m_Action += modleYaoPing.MoveTo;
        ////当药勺碰到药瓶时触发
        modleYaoShao._CallBack += CallBack;
    }
    /// <summary>
    /// 加药动画 不合格版
    /// </summary> 
    void JiaYaoDongHua()
    {
        mouseClock = true;
        modleYaoShao.GetComponent<Collider>().enabled = false;
        SingleModel.Model_DoorTop.DOMoveZ(0.15f, 1f);
        pingGai = modleYaoPing.transform.GetChild(1);
        // modleYaoPing.transform.GetChild(1).DOMoveY(modleYaoPing.transform.GetChild(1).position.y + 0.1f, 1).SetDelay(1f).OnComplete(()=> { });
        pingGai.DOMoveY(pingGai.position.y + 0.1f, 1).SetDelay(1f).OnComplete(() =>
        {
            pingGai.gameObject.SetActive(false);
            //上抬
            //  modleYaoShao.transform.DOMove(modleYaoPing.transform.position + Vector3.up * 0.15f, 1);
            //modleYaoShao.transform.DOMove(modleYaoShao.transform.position + Vector3.up * 0.15f, 1).OnComplete(()=> {
            // modleYaoShao.transform.DOMove(modleYaoPing.transform.position + Vector3.up * 0.15f, 1).OnComplete(() =>
            //{
            //
            modleYaoShao.transform.DOMove(modleYaoPing.transform.position + Vector3.up * 0.15f + Vector3.left * 0.1f, 1).OnComplete(() =>
            {
                modleYaoShao.transform.DORotate(new Vector3(0, 0, -35), 1f).OnComplete(() =>
                {
                    modleYaoShao.transform.DOMove(modleYaoPing.transform.position + Vector3.up * 0.128f + Vector3.left * 0.081f, 1)
                        .SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                        {
                            modleYaoShao.transform.DOMoveY(0.35f, 1.5f).OnComplete(() =>
                            {
                                modleYaoShao.transform.DOMove(new Vector3(-0.1f, 0.35f, 0.03f), 1.5f).OnComplete(() =>
                                {
                                    //mouseClock = false;
                                    //加药时切换的视角
                                    // CameraViewer.Instance.ChangeCameraView(SingleModel.Model_DoorTop, 23, 50, 0.8f, 2f);
                                    JiaYao();
                                });
                            });
                        });
                    modleYaoShao.transform.GetChild(1).DOScale(Vector3.one, 0.1f).SetDelay(1);
                });
                /*.OnComplete(() => { modleYaoShao.transform.GetChild(1).localScale=Vector3.one; })*/
                ;
            });
        });
        // });
        //  });

        GameObject obj = Resources.Load("QiongZhiTangFen") as GameObject;
        GameObject salt = GameObject.Instantiate(obj, modleBeaker.transform);

        salt.transform.localScale = Vector3.zero;
        salt.transform.localPosition = Vector3.zero;
    }
    /// <summary>
    /// 具体的添加药品
    /// </summary>
    void JiaYao()
    {
       
        mouseClock = true;
        _paryicle.gameObject.SetActive(true);
        _paryicle.Play();

        count--;
        modleYaoShao.transform.GetChild(1).DOScale(new Vector3(count / 3f, count / 3f, count / 3f), 1f).SetDelay(0.8f);
        modleBeaker.transform.GetChild(4).DOScale(new Vector3((3 - count) / 3f, (3 - count) / 3f, (3 - count) / 3f), 1f).SetDelay(0.8f);
        // modleYaoShao.transform.DOLocalRotate(new Vector3(-45, 45, -55), 1f).SetLoops(2, LoopType.Yoyo).OnComplete(
        modleYaoShao.transform.DORotate(new Vector3(-45f, 0, 0), 1f, RotateMode.LocalAxisAdd).SetLoops(2, LoopType.Yoyo).OnComplete(
             () =>
             {
                // new Vector3(-29.2f, -41.65f, 61.19f)


                //更改重量显示
                SingleModel.UI_TextInfo.text = (State_CeLiang.targetWeight * (3 - (int)count) / 3).ToString("0.000");

                 if (currentRemoveState == RemoveState.等待去皮)
                 {
                    // InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint, XMLManager.Instance.strConfigDir["NoRemoveInfoLog"]);
                    mouseClock = true;
                     MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["NoRemoveInfoLog"], 350, MyLogger.TextAlign.Center, delegate
                     {

                         mouseClock = false;
                         Resert();
                         OnBegin();
                     });

                     return;
                 }
                 currentRemoveState = RemoveState.禁止去皮;


                 if (count == 0)
                 {
                     currentRemoveState = RemoveState.无需去皮;
                     modleYaoShao.transform.DOMoveY(0.31f, 0.5f).OnComplete(() => { GameObject.Destroy(modleYaoShao.gameObject);
                         GameObject.Destroy(modleYaoPing.gameObject);
                     });
                    //加药完成
                    SingleModel.Model_DoorTop.DOMoveZ(0, 1f).OnComplete(() =>
                     {
                        //把瓶子盖 盖回去
                       // pingGai.gameObject.SetActive(true);
                        // pingGai.DOLocalMove(new Vector3(0.0005f, 0.082f, 0), 1).SetDelay(1f);

                        // CameraViewer.Instance.ChangeCameraView(SingleModel.Model_FXTP, -50, 40, 1.1f, 2, false);
                        //mouseClock = false;

                        OpenTheDoor();
                         
                         modleBeaker.transform.DOMoveX(0.2f, 1f).SetDelay(2f).OnComplete(() => { SingleModel.UI_TextInfo.text = "0.000"; });
                         modleBeaker.transform.DOMove(new Vector3(0.2f, 0.001f, 0), 1f).SetDelay(3f).
                        OnComplete(() =>
                        {
                            
                           // GameObject.Destroy(modleYaoPing.gameObject);
                           // GameObject.Destroy(modleYaoShao.gameObject);
                           mouseClock = true;
                            // MyLogger.Instance.Log(XMLManager.Instance.clickConfigDir["AddCompleteInfoLog"], 350, MyLogger.TextAlign.Center, OnEnd);
                            InfoPanel.Instance.ShowInfo(modleBeaker.transform.Find("ShowPoint"), XMLManager.Instance.clickConfigDir["AddCompleteInfoLog"], 300, OnEnd);

                        });

                     });

                     return;
                 }
                 mouseClock = true;
                 //  MyLogger.Instance.Log(XMLManager.Instance.clickConfigDir["GoOnAddInfoLog"], 350, MyLogger.TextAlign.Center, JiaYao);
                 InfoPanel.Instance.ShowInfo( SingleModel.UI_ShowInfoPoint, XMLManager.Instance.clickConfigDir["GoOnAddInfoLog"], 320, JiaYao);
             });

    }
    /// <summary>
    /// 复原场景里的物品
    /// </summary>
    protected override void RemoveAll()
    {
        if (_paryicle != null)
            GameObject.Destroy(_paryicle.gameObject) ;
        // modleBeaker.DOPause();
        // modleYaoPing.DOPause();
        // modleYaoShao.DOPause();
        MyLogger.Instance.HideLog();
        SingleModel.Model_DianXian.DOPause();
        SingleModel.Model_DianXian.position = GameObject.Find("+ Models/FXTP/end").transform.position;
        SingleModel.Model_GanZaoBao.SetActive(false);
        I_MoveModle[] modles = GameObject.FindObjectsOfType<I_MoveModle>();
        for (int i = 0; i < modles.Length; i++)
        {
            modles[i].DOPause();
            GameObject.Destroy(modles[i].gameObject);
        }
        SingleModel.Model_Door.DOPause();
        SingleModel.Model_DoorTop.DOPause();
        SingleModel.Model_DoorTop.position = Vector3.zero;
        SingleModel.Model_Door.position = Vector3.zero;

    }
    public override void Resert()
    {
        GlobalUIManager.Instance.SetMainStepTitle();//复原提示信息
        RemoveAll();
        // OnBegin();
    }
    /// <summary>
    /// 鼠标在按钮上抬起时需要处理的逻辑
    /// </summary>
    /// <param name="buttonName"></param>
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
    /// <summary>
    /// 鼠标在按钮上长按时需要处理的逻辑
    /// </summary>
    /// <param name="buttonName"></param>
    public override void ButtonOnDrag(string buttonName)
    {
        if (mouseClock) { SingleModel.UI_LoadingPower.fillAmount = 0; return; }
        switch (buttonName)
        {

            case "_Power":
                SingleModel.UI_LoadingPower.fillAmount += Time.deltaTime / 2f;
                if (SingleModel.UI_LoadingPower.fillAmount >= 1f)
                {
                    SingleModel.UI_LoadingPower.fillAmount = 0;
                    // RemoveAll();
                    FXTPMainManager.Instance.BackOnState();
                }
                break;
        }
    }
    /// <summary>
    /// 鼠标在按钮上点击时需要处理的逻辑
    /// </summary>
    /// <param name="buttonName"></param>
    public override void ButtonOnClick(string buttonName)
    {
        if (mouseClock) return;
        InfoPanel.Instance.HideInfo();
        switch (buttonName)
        {
            case "_TareR":
            case "_TareL":
                if (currentRemoveState == RemoveState.等待去皮)
                {
                    //去皮完成
                    SingleModel.UI_TextInfo.text = SingleModel.zero;
                    currentRemoveState = RemoveState.无需去皮;
                }
                else if (currentRemoveState == RemoveState.无需去皮)
                {
                    //提示不需要进行去皮操作
                    InfoPanel.Instance.ShowInfo(SingleModel.UI_ShowInfoPoint,
                    XMLManager.Instance.strConfigDir["NoWayToRemoveInfoLog"],160);
                    //  mouseClock = true;
                    //  MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["NoWayToRemoveInfoLog"], 350, MyLogger.TextAlign.Center,delegate { mouseClock = false; });
                }
                else
                {
                    Resert();
                    OnBegin();
                    mouseClock = true;
                    MyLogger.Instance.Log(XMLManager.Instance.clickConfigDir["WrongRemoveInCeLiangInfoLog"], 350, MyLogger.TextAlign.Center, delegate
                    {
                        mouseClock = false;
                    });
                   
                   
                }
                break;
            case "_Door":
                if (currentDoorState == DoorState.Close) { OpenTheDoor(); }
                else { CloseTheDoor(); }
                break;
        }

    }

}
