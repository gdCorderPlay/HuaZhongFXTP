using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using WJ;
/// <summary>
/// 挂在烧杯上的脚本
/// </summary>

public class OnBeaker :  I_MoveModle
{
    
    /// <summary>
    /// 控制物体的移动跟随
    /// </summary>
    /// <param name="target"></param>
    public override void MoveTo(Vector3 target)
    {
        if (isDrag)
        {
            if (firstClick)
            {
                Cursor.visible = false;
                firstClick = false;
                offsetPos = transform.position - target;
            }
            if (Vector3.Distance(target + offsetPos, transform.position) > SingleModel.maxDistance * 1.1f)
            {
                isDrag = false;
                Cursor.visible = true;
                return;
            }
            transform.position = target + offsetPos;
            //if (Mathf.Abs( Input.GetAxis("Mouse X")) < 2 && Mathf.Abs(Input.GetAxis("Mouse Y")) < 2)
            //{ transform.position = target + offsetPos; }
            //else
            //{
            //    isDrag = false;
            //    Cursor.visible = true;
            //}
        }
    }
    /// <summary>
    /// 碰到指定物体时 触发
    /// </summary>
    /// <param name="other"></param>
    private   void OnTriggerEnter(Collider other)
    {
        InfoPanel.Instance.HideInfo();
        if (other.name.Equals("Balance_FX"))
         {
            if (_CallBack != null)
            {
                Vector3 target = (transform.position - other.transform.position).normalized * 0.04f + transform.position;
                target.y = transform.position.y;
                // transform.DOMove(target, SingleModel.escapeTime);
                transform.position = target;
                _CallBack();
                
            }
            else
            {
                //  InfoPanel.Instance.ShowInfo(GameObject.Find("Point").transform,
                //  XMLManager.Instance.strConfigDir["WrongBreakerInfoLog"]);
                FXTPMainManager.Instance.SetMouseClock(true);
                MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["WrongBreakerInfoLog"], 350, MyLogger.TextAlign.Center,
                    delegate {
                        FXTPMainManager.Instance.SetMouseClock(false);
                    }
                    );
            }
        }
        //Vector3 target = (transform.position - other.transform.position) * 0.2f + transform.position;
        //target.y = transform.position.y;
        //transform.DOMove(target, 0.5f);
        isDrag = false;
        Cursor.visible = true;
    }

    private void OnTriggerStay(Collider other)
    {
        //Vector3 target = (transform.position - other.transform.position) * 0.2f + transform.position;
        //target.y = transform.position.y;
        //transform.DOMove(target, SingleModel.escapeTime);
        Vector3 target = (transform.position - other.transform.position).normalized * 0.02f + transform.position;
        target.y = transform.position.y;
        // transform.DOMove(target, SingleModel.escapeTime);
        transform.position = target;
    }
}

