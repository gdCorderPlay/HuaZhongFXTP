using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using WJ;
/// <summary>
/// 可以拖动的物体的基类 已实现跟随鼠标的移动
/// </summary>
public abstract class I_MoveModle :MonoBehaviour {

    protected bool isDrag = false;
    protected bool firstClick = true;
    /// <summary>
    /// 鼠标点击的偏移量
    /// </summary>
    protected Vector3 offsetPos;
    public abstract void MoveTo(Vector3 target);

    /// <summary>
    /// 回调事件
    /// </summary>
    public Action _CallBack;


    void OnMouseDown()
    {
        CameraViewer.Instance.IsAutoChangeView = true;
        
        isDrag = true;
        
    }

    void OnMouseUp()
    {
        CameraViewer.Instance.IsAutoChangeView = false;
        Cursor.visible = true;
        isDrag = false;
        firstClick = true;
    }
}
