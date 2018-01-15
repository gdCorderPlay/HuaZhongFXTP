using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
namespace WJ
{
    public class ModelTipManager : SingletonMono<ModelTipManager>
    {
        [SerializeField] private string tipMessage;
        [SerializeField] private float waitTime = 1.0f;
        [SerializeField] private bool _canShow = true;
       private bool Showing = false;
        TipPanel _tp = null;
       
        
        public void ShowTip()
        {
            _canShow = true;
                if (_tp == null)
                {
                GameObject _go = Instantiate(Resources.Load("TipPanel")) as GameObject;
                _go.name = "TipPanel";

                RectTransform _tip = _go.GetComponent<RectTransform>();
                _tip.SetParent(GameObject.Find("Canvas/GroupPanel").transform);

                _tip.localScale = Vector3.zero;
                _tip.anchoredPosition = Vector2.zero;
                _tp = _tip.GetComponent<TipPanel>();
            }
            //Invoke("WaitShow", waitTime);
            _tp.ShowTip(tipMessage);
           
            _tp.Follow(Camera.main.WorldToScreenPoint(transform.position));

        }
    
    //private void OnDisable()
    //{
    //    if (allShow)
    //    {
    //        if (_tp != null)
    //        {
    //           // CancelInvoke("WaitShow");
    //            _tp.Hide();
    //        }
    //    }
    //}
    private void OnMouseEnter()
        {
            //if (allShow) return;
			_tp = FindObjectOfType<TipPanel> ();
            // 查找Tip对象
            // 没有就创建
			if(_tp == null)
            {
                GameObject _go = Instantiate(Resources.Load("TipPanel")) as GameObject;
                _go.name = "TipPanel";

                RectTransform _tip = _go.GetComponent<RectTransform>();
                _tip.SetParent(GameObject.Find("Canvas/GroupPanel").transform);

                _tip.localScale = Vector3.zero;
                _tip.anchoredPosition = Vector2.zero;
                _tp = _tip.GetComponent<TipPanel>();
            }

			if (_canShow)
			{
				
				Invoke ("WaitShow", waitTime);
				//StartCoroutine(WaitShow(waitTime));
			}
        }

		private void WaitShow()
        {
            // 赋值并显示
            Showing = true;
            _tp.ShowTip (tipMessage);
           
				
        }

        private void OnMouseExit()
        {
            //  if (allShow) return;
              Showing = false;
				HideThisUI ();
        }

		#region 放出去 在鼠标移动到ui上关闭提示
		public void HideThisUI()
		{
			if (_tp != null)
			{
				if (!ActiveObj ())
					return;
				CancelInvoke ("WaitShow");
				_tp.Hide ();
			}
		}

		public bool ActiveObj()
		{
			
				return _tp.IsActiveObj();
			
		}
		#endregion

        private void Update()
        {
            //if ()
            //{
            //   // _tp.ShowTip(tipMessage);
            //    _tp.Follow(Camera.main.WorldToScreenPoint(transform.position)); return;
            //}
            if(_tp && Showing)
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    if (_tp.IsShowed)
                        _tp.Hide();
                    Showing = false;
                    _canShow = false;
                }

                if (_tp.IsShowed)
                {
                    _tp.Follow(Input.mousePosition);
                }

//				if (IToolsLM.getInstance.IsOnUI)
//					HideThisUI ();
            }

            if (Input.GetMouseButtonDown(0))
            {
                CancelInvoke("WaitShow");
                _canShow = false;

            }

            if (Input.GetMouseButtonUp(0))
                _canShow = true;
        }
    }
}

