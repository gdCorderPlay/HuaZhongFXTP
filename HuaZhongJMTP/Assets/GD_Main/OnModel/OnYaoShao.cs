using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using WJ;
/// <summary>
/// 药勺的移动控制脚本
/// </summary>
public class OnYaoShao : I_MoveModle
{
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
            // transform.position += (target + offsetPos - transform.position) * Mathf.Clamp(Vector3.Distance(target + offsetPos, transform.position), 0, SingleModel.maxSpeed);
            if (Vector3.Distance(target + offsetPos, transform.position) > SingleModel.maxDistance*0.8f)
            {
                isDrag = false;
                Cursor.visible = true;
                return;
            }
            transform.position = target + offsetPos;
            //if (Mathf.Abs(Input.GetAxis("Mouse X")) < 2 && Mathf.Abs(Input.GetAxis("Mouse Y")) < 2)
            //{ transform.position = target + offsetPos; }
            //else
            //{
            //    isDrag = false;
            //   
            //}
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //InfoPanel.Instance.HideInfo();
        if (other.transform.name.Equals("QiongZhiTangGanFenGuan"))
        {
            if (_CallBack != null && isDrag)
            {
               
                // GetComponent<Collider>().enabled = false;
                Vector3 target = (transform.position - other.transform.position).normalized * 0.02f + transform.position;
                target.y = transform.position.y;
                // transform.DOMove(target, SingleModel.escapeTime);
                transform.position = target;
                _CallBack();
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
       
        Vector3 target = (transform.position - other.transform.position).normalized * 0.01f + transform.position;
        target.y = transform.position.y;
        // transform.DOMove(target, SingleModel.escapeTime);
        transform.position = target;
    }
}
