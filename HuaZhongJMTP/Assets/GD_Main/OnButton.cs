using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
///  定义了物体被点击时回掉方法
/// </summary>
public class OnButton : MonoBehaviour {

    /// <summary>
    /// 被点击时触发
    /// </summary>
   public  Action <string> CallBack_Click;

    /// <summary>
    /// 离开时触发
    /// </summary>
   public Action<string> CallBcak_UP;

    /// <summary>
    /// 维持在上面时触发
    /// </summary>
    public Action<string> CallBack_Drag;


    private void OnMouseDrag()
    {if(CallBack_Drag!=null)
        CallBack_Drag(transform.name);
       
    }

    private void OnMouseDown()
    {
        if(CallBack_Click!=null)
        CallBack_Click(transform.name);
    }

    private void OnMouseUp()
    {
        if(CallBcak_UP!=null)
        CallBcak_UP(transform.name);
       
    }

}
