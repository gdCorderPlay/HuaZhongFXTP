using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace WJ
{
    /// <summary>
    /// 面板信息
    /// </summary>
	public class InfoPanel : MonoBehaviour
	{
		private static InfoPanel _instance;
        /// <summary>
        /// 面板信息管理的单例
        /// </summary>
        public static InfoPanel Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = FindObjectOfType<InfoPanel>();
					if(_instance == null)
					{
						GameObject _go = Instantiate(Resources.Load("InfoPanel")) as GameObject;
						_go.name = "InfoPanel";
						RectTransform _RT = _go.GetComponent<RectTransform>();

						_RT.SetParent(GameObject.Find("Canvas/GroupPanel").transform);
						_RT.localEulerAngles = Vector3.zero;
						_RT.anchoredPosition3D = Vector3.zero;
						_instance = _go.GetComponent<InfoPanel>();
					}
				}
				return _instance;
			}
		}


		/// <summary>
		/// 空间参考点
		/// </summary>
		private Transform ComparePoint;
		/// <summary>
		/// 显示隐藏的对象
		/// 为了规避在实例化对象的时候，就在其下创建了一个整体的隐藏显示的对象
		/// 以保持当前的顶级对象始终保持显示状态
		/// </summary>
		[SerializeField]private GameObject _activeObj = null;
		[SerializeField]private Text _label = null;
        [SerializeField]private Button _closeBtn = null;

        private RectTransform _rt = null;
		public RectTransform RT
		{
			get { return this._rt; }
		}

		/// <summary>
		/// 显示的状态
		/// </summary>
		public bool _isShowed = false;
		/// <summary>
		/// 透明组件
		/// </summary>
        private CanvasGroup _alphaComp = null;
		/// <summary>
		/// 对齐方式，意味着显示什么部位的箭头
		/// </summary>
		public enum AlignMent { Down,Left }
		/// <summary>
		/// 回调事件 添加在关闭按钮上
		/// </summary>
		private Action _callback = null;
		/// <summary>
		/// 默认的宽度，高度会自动进行计算
		/// </summary>
		private int _targetWidth = 340;


		
		/// <summary>
		/// 初始化
		/// </summary>
		void Awake()
		{
			this._rt = GetComponent<RectTransform>();
			_alphaComp = _activeObj.GetComponent<CanvasGroup>();
			_closeBtn.onClick.AddListener(delegate{
				if(_callback != null)_callback();
				Hide();
			});
			Init();
		}

		private void Init()
		{
			_rt.localScale = Vector3.one * 0.5f;
			_activeObj.SetActive(false);
			_alphaComp.alpha = 0;
		}
        /// <summary>
        /// 方法重载
        /// </summary>
       
		public void ShowInfo(Transform _target, string _message, Action _callback = null)
		{
            ShowInfo(_target,_message,_targetWidth,_callback);
		}
        /// <summary>
        /// 显示面板 
        /// </summary>
        /// <param name="_target">空间参考点</param>
        /// <param name="_message">显示信息</param>
        /// <param name="_width">显示的尺寸</param>
        /// <param name="_callback">毁掉的方法</param>
		public void ShowInfo(Transform _target, string _message,int _width,Action _callback = null)
		{
            this._callback = _callback;
            this.ComparePoint = _target;
            _label.text = _message;
            _rt.sizeDelta = new Vector2(_width, _rt.sizeDelta.y);
            _rt.sizeDelta = new Vector2(_rt.sizeDelta.x, _label.preferredHeight + 50);
            StartCoroutine("PlayShowAnim");
		}
        /// <summary>
        /// 关闭显示面板
        /// </summary>
        public void HideInfo()
        {
            if (_isShowed)
            {
                Hide();
            }
        }
		private IEnumerator PlayShowAnim()
		{
			if(_isShowed)
			{
				Hide();
				yield return new WaitForSeconds(0.3f);
			}
			_activeObj.SetActive(true);
			Show();
            //关闭协程
			StopCoroutine("PlayShowAnim");
		}


		/// <summary>
		/// 显示的方法
		/// </summary>
		private void Show()
		{
			_rt.DOPause();
			_alphaComp.DOPause();

			_rt.DOScale(Vector3.one,0.6f).SetEase(Ease.OutBack);
			_alphaComp.DOFade(1,0.6f);

			this._isShowed = true;
		}

		/// <summary>
		/// 隐藏的方法
		/// </summary>
		public void Hide()
		{
			_rt.DOPause();
			_alphaComp.DOPause();

			_alphaComp.DOFade(0,0.3f);
			_rt.DOScale(Vector3.one * 0.5f,0.3f).SetEase(Ease.InBack).OnComplete(delegate{
				_activeObj.SetActive(false);
			});
			this._isShowed = false;
		}

		void Update()
		{
            //确定显示的位置
			if(_isShowed)
			{
				Vector2 _pos = WorldPositionToScreenAnchorPosition(ComparePoint);
				_rt.anchoredPosition = new Vector2(_pos.x - 26,_pos.y);
			}
		}


        /// <summary>
        /// 三维坐标点转换成屏幕坐标点
        /// </summary>
        /// <returns>The position to screen anchor position.</returns>
        /// <param name="_targetPoint">Target point.</param>
        /// <param name="_cam">Cam.</param>
        public Vector2 WorldPositionToScreenAnchorPosition(Transform _targetPoint, Camera _cam = null)
        {
            if (_cam == null)
                _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            Vector3 _pos = Vector3.zero;
            _pos = _cam.WorldToScreenPoint(_targetPoint.position);
            Vector2 _anchorPos = new Vector2(_pos.x, _pos.y);
            return _anchorPos;
        }
        public Vector2 WorldPositionToScreenAnchorPosition(Vector3 _targetPoint, Camera _cam = null)
        {
            if (_cam == null)
                _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            Vector3 _pos = Vector3.zero;
            _pos = _cam.WorldToScreenPoint(_targetPoint);
            Vector2 _anchorPos = new Vector2(_pos.x, _pos.y);
            return _anchorPos;
        }
    }
}