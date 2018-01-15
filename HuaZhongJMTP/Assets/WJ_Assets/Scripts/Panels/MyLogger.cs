using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace WJ
{
    /// <summary>
    /// Log管理
    /// </summary>
	public class MyLogger : MonoBehaviour 
	{
		private static MyLogger _instance;
		public static MyLogger Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = FindObjectOfType<MyLogger>();
					if(_instance == null)
					{
						GameObject _log = Instantiate(Resources.Load("LogPanel")) as GameObject;
						_log.name = "LogPanel";
						RectTransform _logRT = _log.GetComponent<RectTransform>();

						_logRT.SetParent(GameObject.Find("Canvas/GroupPanel").transform);
						_logRT.localEulerAngles = Vector3.zero;
						// _logRT.localScale = Vector3.one;
						_logRT.anchoredPosition3D = Vector3.zero;
						_instance = _log.GetComponent<MyLogger>();
					}
				}
				return _instance;
			}
		}


		private RectTransform _rt;
		[SerializeField] private GameObject _activeObj;
		[SerializeField] private Text _label;
		private CanvasGroup _alphaComp;
		private Action _callback = null;
		private bool _isShow = false;

		private bool isInit;

		[SerializeField]private Button enterBtn;

		/// <summary>
		/// 文本的对齐方式
		/// 默认为居中对齐
		/// </summary>
		public enum TextAlign { Left, Center }

		/// <summary>
		/// 目标宽度大小，默认值为290
		/// </summary>
		private int _targetWidth = 290;


		void Awake()
		{
			_rt = GetComponent<RectTransform>();
			_alphaComp = _activeObj.GetComponent<CanvasGroup>();

			enterBtn.onClick.AddListener(delegate{
				EnterBtnClicked();
			});

			Init();
		}

		private void Init()
		{
			_rt.localScale = Vector3.one * 0.5f;
			_alphaComp.alpha = 0;
			_activeObj.SetActive(false);
		}

		private void EnterBtnClicked()
		{
			if(_callback != null)
				_callback();
				
			Hide();
		}


		/// <summary>
		/// Log的具体方法，重载
		/// </summary>
		/// <param name="_message">消息内容</param>
		/// <param name="_callback">回调</param>
		public void Log(string _message,Action _callback = null)
		{
			SetLog(_message, TextAlign.Center, _targetWidth, _callback);
		}

		/// <summary>
		/// Log的具体方法，重载
		/// </summary>
		/// <param name="_message">消息内容</param>
		/// <param name="_width">设定宽度(高度自动计算)</param>
		/// <param name="_callback">回调</param>
		public void Log(string _message,int _width, Action _callback = null)
		{
			SetLog(_message,TextAlign.Center,_width,_callback);
		}


		/// <summary>
		/// Log的具体方法，重载
		/// </summary>
		/// <param name="_message">消息内容</param>
		/// <param name="_align">文本的对齐方式：Center，Left</param>
		/// <param name="_callback">回调</param>
		public void Log(string _message,TextAlign _align,Action _callback = null)
		{
			SetLog(_message, _align, _targetWidth, _callback);
		}


		/// <summary>
		/// Log的具体方法，重载
		/// </summary>
		/// <param name="_message">消息内容</param>
		/// <param name="_width">设定宽度(高度自动计算)</param>
		/// <param name="_align">文本的对齐方式：Center，Left</param>
		/// <param name="_callback">回调</param>
		public void Log(string _message,int _width,TextAlign _align,Action _callback = null)
		{
			SetLog(_message,_align,_width,_callback);
		}

        public void HideLog()
        {
            Hide();
        }

		private void SetLog(string _message,TextAlign _align,int _width, Action _callback)
		{
			_rt.SetAsLastSibling();
			this._callback = _callback;
			StartCoroutine(PlayChangeAnim(_message,_align,_width));
		}


		private IEnumerator PlayChangeAnim(string _message,TextAlign _align,int _width)
		{
			if(_isShow)
			{
				Hide();
				yield return new WaitForSeconds(0.3f);
			}

			_label.text = _message;
			switch(_align)
			{
			case TextAlign.Center:
				_label.alignment = TextAnchor.UpperCenter;
				break;

			case TextAlign.Left:
				_label.alignment = TextAnchor.UpperLeft;
				break;
			}
			_rt.sizeDelta = new Vector2(_width,_rt.sizeDelta.y);
			_rt.sizeDelta = new Vector2(_rt.sizeDelta.x,_label.preferredHeight + 110);
			_activeObj.SetActive(true);


			Show();
		}

		private void Show()
		{
            GlobalUIManager.Instance.UIControllState = GlobalUIManager.UI_CONTROLL_STATE.LOCK;
			_rt.DOPause();
			_activeObj.GetComponent<CanvasGroup>().DOPause();
			_rt.DOScale(Vector3.one,0.6f).SetEase(Ease.OutBack);
			_activeObj.GetComponent<CanvasGroup>().DOFade(1,0.6f);
			_isShow = true;
		}

		private void Hide()
		{
			_rt.DOPause();
			_activeObj.GetComponent<CanvasGroup>().DOPause();
			_rt.DOScale(Vector3.one * 0.6f,0.3f).SetEase(Ease.InBack);
			_activeObj.GetComponent<CanvasGroup>().DOFade(0,0.3f).OnComplete(delegate{
				_activeObj.SetActive(false);
                GlobalUIManager.Instance.UIControllState = GlobalUIManager.UI_CONTROLL_STATE.FREE;
			});
			_isShow = false;
		}
	}
}