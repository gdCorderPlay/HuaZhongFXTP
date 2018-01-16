using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using WJ;
public class OnFaMa : I_MoveModle
{
    
    private bool targetFaMa = false; 
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
            //if (Input.GetAxis("Mouse X") < 2f || Input.GetAxis("Mouse Y") < 2f)
            //{
            //    transform.position = target + offsetPos;

            //}
            //else
            //{
            //    isDrag = false;

            //}
            //
            if (Vector3.Distance(target + offsetPos, transform.position) > SingleModel.maxDistance*0.8f)
            {
                Cursor.visible = true;
                isDrag = false;
                return;
            }
            transform.position = target + offsetPos;
            // transform.position += (target + offsetPos - transform.position) * Mathf.Clamp(Vector3.Distance(target + offsetPos, transform.position), 0, SingleModel.maxSpeed);

        }
    }

    private void OnMouseEnter()
    {
    }
    private void OnMouseExit()
    {
    }
    /// <summary>
    /// 碰到指定物体时 触发
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter (Collider other)
    {
        InfoPanel.Instance.HideInfo();
        if (other.name.Equals("Balance_FX"))
        {

            if (_CallBack != null)
            {
                Vector3 target = (transform.position - other.transform.position).normalized * 0.02f + transform.position;
                target.y = transform.position.y;
                // transform.DOMove(target, SingleModel.escapeTime);
                transform.position = target;
                _CallBack();
            }
            else
            {
                 FXTPMainManager.Instance.SetMouseClock(true);
                  MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["WrongWeightInfoLog"], 350, MyLogger.TextAlign.Center,delegate {
                    FXTPMainManager.Instance.SetMouseClock(false); });
                
               // InfoPanel.Instance.ShowInfo(GameObject.Find("Point").transform,
                   // XMLManager.Instance.strConfigDir["WrongWeightInfoLog"]);
            }
        }
        if (isDrag)
        {
            //Vector3 target = (transform.position - other.transform.position) * 0.2f + transform.position;
            //target.y = transform.position.y;
            //transform.DOMove(target, SingleModel.escapeTime);
            Vector3 target = (transform.position - other.transform.position).normalized * 0.02f + transform.position;
            target.y = transform.position.y;
            // transform.DOMove(target, SingleModel.escapeTime);
            transform.position = target;
        }
            isDrag = false;
        Cursor.visible = true;
        //Vector3 target = (transform.position - other.transform.position) * 0.2f + transform.position;
        //target.y = transform.position.y;
        //transform.DOMove(target, 0.5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (isDrag)
        {
            //Vector3 target = (transform.position - other.transform.position) * 0.2f + transform.position;
            //target.y = transform.position.y;
            //transform.DOMove(target, SingleModel.escapeTime);
            Vector3 target = (transform.position - other.transform.position).normalized * 0.01f + transform.position;
            target.y = transform.position.y;
            // transform.DOMove(target, SingleModel.escapeTime);
            transform.position = target;
        }
    }
}
