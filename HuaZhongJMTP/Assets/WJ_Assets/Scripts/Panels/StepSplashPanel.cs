using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace WJ
{
    /// <summary>
    /// 本脚本控制了 UI界面的切换和管理 点击前进和后退在本脚本中
    /// </summary>
    public class StepSplashPanel : MonoBehaviour
    {
        [SerializeField] private Text _label = null;
        [SerializeField] private GameObject _activeObj = null;
        [SerializeField] private Button menuBtn = null;
        [SerializeField] private int minWidth = 800;
        [SerializeField] private Button[] contrlllBtns;

        private StepPanel stepPanel;
        private RectTransform _rt;
        private CanvasGroup _alphaComp;

        private bool _isShow = false;
        public bool IsShow
        {
            get { return this._isShow; }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
            _alphaComp = _activeObj.GetComponent<CanvasGroup>();
            stepPanel = GetComponentInChildren<StepPanel>();

            _rt.anchoredPosition = new Vector2(-5,-100);
            _alphaComp.alpha = 0;

            menuBtn.onClick.AddListener(delegate
            {
                _rt.SetAsLastSibling();
                stepPanel.Show();
            });

            for (int i = 0; i < contrlllBtns.Length;i++)
            {
                Button _b = contrlllBtns[i];
                _b.onClick.AddListener(delegate {
                    ControllBtnClicked(_b.gameObject);
                });
            }
        }
        /// <summary>
        /// 点击button 按钮时会触发本方法
        /// </summary>
        /// <param name="B"></param>
        private void ControllBtnClicked(GameObject B)
        {
			if (GlobalUIManager.Instance.UIControllState == GlobalUIManager.UI_CONTROLL_STATE.LOCK)
				return;
            if (FXTPMainManager.Instance.currentState.mouseClock) return;
//			if (WholeDataManager.getInstance.cameraEnum == WholeDataManager.CameraEnum._详细)
//				return;
//			if (WholeDataManager.getInstance.isLMPlayeing)
//				return;
            int _step = 0;
            switch(B.name)
            {
                case "PrevBtn":
                    _step = -1; 
                    break;
                case "ResetBtn":
                    _step = 0;
                    break;
                case "NextBtn":
                    _step = 1;
                    break;
            }

            FXTPMainManager.Instance.ChangeNextState(_step, GlobalUIManager.STEP_CONTROLL_STATE.Click);

            //调用菜单显示面板
          //  GlobalUIManager.Instance.ChangeStepIndexValue(_step,GlobalUIManager.STEP_CONTROLL_STATE.Click);

        }

        public void InitStepItem()
        {
            _label.text = GlobalUIManager.Instance.AllStepData[GlobalUIManager.stepIndex].items[GlobalUIManager.stepSubIndex].title;
        }

        /// <summary>
        /// 设置步骤文本
        /// </summary>
        public void ChangeStepItemTitle()
        {
            StartCoroutine("StartChange");
        }

        private IEnumerator StartChange()
        {
            if(_isShow)
            {
                Hide();
                yield return new WaitForSeconds(0.3f);
            }
            _label.text = GlobalUIManager.Instance.AllStepData[GlobalUIManager.stepIndex].items[GlobalUIManager.stepSubIndex].title;
            int _width = (int)(_label.preferredWidth + 270);
            if (_width < minWidth)
                _width = minWidth;

            _rt.sizeDelta = new Vector2(_width, _rt.sizeDelta.y);
            Show();
        }

        /// <summary>
        /// 显示的方法
        /// </summary>
        public void Show()
        {
            _rt.DOPause();
            _alphaComp.DOPause();

            _rt.anchoredPosition = new Vector2(0,0);
            _rt.localScale = Vector3.one * 0.9f;

            _activeObj.SetActive(true);
            _rt.DOAnchorPosY(-5, 0.6f).SetEase(Ease.OutBack);
            _rt.DOScale(Vector3.one, 0.6f).SetEase(Ease.OutBack);
            _alphaComp.DOFade(1, 0.6f);
            _isShow = true;

            GetComponentInChildren<StepPanel>().SetStepIconColorState();
        }

        /// <summary>
        /// 隐藏的方法
        /// </summary>
        public void Hide()
        {
            _rt.DOPause();
            _alphaComp.DOPause();

            _alphaComp.DOFade(0, 0.3f);
            _rt.DOScale(Vector3.one * 0.9f, 0.3f).SetEase(Ease.InBack).OnComplete(delegate {
                _activeObj.SetActive(false);
                _isShow = false;
            });
        }
    }
}

