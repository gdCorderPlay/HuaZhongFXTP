using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using System;
/// <summary>
/// 分析天平程序的主入口
/// </summary>
 
public class FXTPMainManager : MonoBehaviour
{
    #region 主入口的单例
    private static FXTPMainManager _instance;
    public static FXTPMainManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FXTPMainManager>();
                if (_instance == null)
                {
                    GameObject _go = new GameObject();
                    _go.name = "+ Singleton_GlobalUIManager";
                    _instance = _go.AddComponent<FXTPMainManager>();
                }
            }
            return _instance;
        }
    }
    #endregion
    
    /// <summary>
    /// 各个阶段的状态枚举
    /// </summary>
    public enum StateType { 连接电源=0,调整水平,开启天平,去皮,校准1,校准2,测量,结束}

    /// <summary>
    /// 当前的状态 默认是连接电源
    /// </summary>
    [HideInInspector]
     private int currentType =7; //此处为了方便调试改为private 

    public StateType currentStateType
    {
        get { return (StateType)currentType; }
    }
    /// <summary>
    /// 是否进入了展示界面
    /// </summary>
    public bool isShowDes=false;
    /// <summary>
    /// 记录大步骤0表示准备阶段 1表示测量阶段
    /// </summary>
    public int _Step = 0;
    /// <summary>
    /// 当前所处在的阶段
    /// </summary>
    public State_Base currentState;

   

    private IEnumerator Start()
    {
        GlobalUIManager.Instance.InitUIData(XMLManager.Instance .tempStepData);
        Init();
        yield return new WaitForSeconds(1);
        OnBegin();
    }
    /// <summary>
    /// 初始化设置 绑定事件
    /// </summary>
    private void Init()
    {
        isShowDes = false;
        OnButton[] buttons = GameObject.FindObjectsOfType<OnButton>();

        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].CallBack_Click += ButtonOnClick;
            buttons[i].CallBack_Drag += ButtonOnDrag;
            buttons[i].CallBcak_UP += ButtonOnUp;
        }
    }

    private void ButtonOnUp(string buttonName)
    {
        if (currentState != null&&!isShowDes)
            currentState.ButtonOnUp(buttonName);
    }

    private void ButtonOnDrag(string buttonName)
    {
        if (currentState != null && !isShowDes)
            currentState.ButtonOnDrag(buttonName);
    }

    private void ButtonOnClick(string buttonName)
    {
        if (currentState != null && !isShowDes)
            currentState.ButtonOnClick(buttonName);
    }

    public void SetMouseClock(bool mouseClock)
    {
        
        currentState.mouseClock = mouseClock;
    }
    /// <summary>
    /// 开始状态 默认是自动跳转 如果传递的参数是false 表示手动跳转过去需要执行刷新程序
    /// </summary>
    private void OnBegin(bool isAuto=true)
    {
        //关闭上一步的显示
       // MyLogger.Instance.HideLog();
        switch (currentType)
        {
            case (int)StateType.连接电源: currentState = new State_LinkPower(); break;
            case (int)StateType.调整水平:currentState = new State_Gradienter(); break;
            case (int)StateType.开启天平: currentState = new State_ON(); break;
            case (int)StateType.去皮: currentState = new State_Remove (); break;
            case (int)StateType.校准1: currentState = new State_JiaoZhun1();  break;
            case (int)StateType.校准2: currentState = new State_JiaoZhun2(); break;
            case (int)StateType.测量:  currentState = new State_CeLiang(); break;
            case (int)StateType.结束:  currentState = new State_End();  break;
            default:currentType = Mathf.Clamp(currentType, 0, (int)StateType.结束);
                 return ;
        }
        if (currentState._CallBack == null)
        { currentState._CallBack = delegate (int step) { EndCallBack(step); }; }
        if (isAuto)
        { currentState.OnBegin(); }
        //else
        //{
        //    currentState.Resert();
        //}
    }
    /// <summary>
    /// 当前步骤下要进行的操作
    /// </summary>
   private  void Update()
    {
        if(currentState!=null && !isShowDes)
        currentState.OnUpdate();
    }
    /// <summary>
    /// 步骤结束时回调该方法
    /// </summary>
    /// <param name="_bool"></param>
    void EndCallBack(int step)
    {
       ChangeNextState(step);
    }
    /// <summary>
    /// 进入到下一个阶段
    /// </summary>
    public void ChangeNextState(int step )
    {

        GlobalUIManager.Instance.ChangeStepIndexValue(step, GlobalUIManager.STEP_CONTROLL_STATE.Auto);
        currentType+=step;
        OnBegin();
    }
    public void ChangeNextState(int step, GlobalUIManager.STEP_CONTROLL_STATE stepControllState)
    {
        //if(currentType)
        //不可以在准备阶段跳转
        if (stepControllState == GlobalUIManager.STEP_CONTROLL_STATE.Click)
        {
            if (step == 0)
            {
                //OnBegin();
                currentState.Resert();
                currentState.OnBegin() ;
                return;
            }
            if (currentType < (int)StateType.测量 || currentType + step < (int)StateType.测量)
            {
                currentState.mouseClock = true;
                MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["PrepareingInfoLog"], 350, MyLogger.TextAlign.Center,
                    delegate { currentState.mouseClock = false; });
            }
            else if (currentType + step>(int)StateType.结束)
            {
                currentState.mouseClock = true;
                MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["LastStepInfoLog"], 350, MyLogger.TextAlign.Center,
                    delegate { currentState.mouseClock = false; });

            }
            else
            {
                GlobalUIManager.Instance.ChangeStepIndexValue(step, stepControllState);

                currentType += step;
                //点击跳转时需要 
                currentState.Resert();
                OnBegin();
            }
        }
        ///可以跳转至准备阶段
        if (stepControllState == GlobalUIManager.STEP_CONTROLL_STATE.Auto)
        {
            GlobalUIManager.Instance.ChangeStepIndexValue(step, GlobalUIManager.STEP_CONTROLL_STATE.Click);
            currentType += step;
            currentState.Resert();
            OnBegin();
        }
    }
    /// <summary>
    /// 回到打开电源的阶段
    /// </summary>
    public void BackOnState()
    {
        currentState.Resert();
        currentType = (int)StateType.开启天平;
        OnBegin();
        GlobalUIManager.stepIndex = 0;
        GlobalUIManager.stepSubIndex = currentType;
        GlobalUIManager.Instance.StepChangeEvent();
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        InfoPanel.Instance.ShowInfo(GameObject.Find("+ Models/FXTP/InfoPoint").transform, "浮屠玄冷喝的声音，爆响而起，那之中蕴含的怒意，令得天地都是寂静下来，圣品之威，显露无疑。", delegate {
    //            print("信息面板已关闭");
    //        });
    //    }
    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        MyLogger.Instance.Log("浮屠玄冷喝的声音，爆响而起，那之中蕴含的怒意，令得天地都是寂静下来，圣品之威，显露无疑。", 350, MyLogger.TextAlign.Center, delegate {
    //            print("Log窗口已关闭");
    //        });
    //    }
    //}
   
}
