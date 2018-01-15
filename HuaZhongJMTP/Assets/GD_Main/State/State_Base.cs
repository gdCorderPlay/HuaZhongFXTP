using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using System;
using UnityEngine.UI;
/// <summary>
/// 基类 定义了必须具备的一些方法
/// </summary>
public abstract class State_Base {
    /// <summary>
    /// 记录是否已经通电
    /// </summary>
    protected static bool isConnectPower=false;
    protected bool stateEnd ;
    /// <summary>
    /// 锁定鼠标点击事件
    /// </summary>
    public    bool mouseClock;
    public virtual void OnBegin() { }

    public virtual void OnUpdate() { }

    public virtual void OnEnd() { }

    public Action<int> _CallBack;

    /// <summary>
    /// 用于在各个步骤中监听按钮的事件信息
    /// </summary>
    /// <param name="buttonName"></param>
    public virtual void ButtonOnUp(string buttonName) { }
    public virtual void ButtonOnDrag(string buttonName) { }
    public virtual void ButtonOnClick(string buttonName) { }
    /// <summary>
    /// 重置方法
    /// </summary>
    public virtual void Resert()
    {
    }
    protected virtual void RemoveAll() { }
}
