using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace WJ
{
    public class StepPanel : MonoBehaviour
    {
        /// <summary>
        /// 操作步骤主条目的父对象
        /// </summary>
        [SerializeField] private RectTransform _itemParent = null;
        public RectTransform ItemParent
        {
            get { return this._itemParent; }
        }
        /// <summary>
        /// 动画对象
        /// </summary>
        [SerializeField] private GameObject _activeObj = null;
        /// <summary>
        /// 透明组件
        /// </summary>
        private CanvasGroup _alphaComp = null;
        private RectTransform _rt = null;
        [SerializeField] private Button closeBtn = null;

        [SerializeField] private List<StepItem> childStaters = new List<StepItem>();
        [SerializeField] private Sprite gouSelectIcon = null;
        [SerializeField] private Color32 staterNormalColor = new Color32(75, 80, 90,255);
        [SerializeField] private Color32 staterSelectColor = new Color32(0, 150, 255, 255);
        [SerializeField] private Color32 stepIconNormalColor = new Color32(200, 220, 200, 255);
        [SerializeField] private Color32 stepIconSelectColor = new Color32(255, 140, 0, 255);

        private void Start()
        {
            _rt = GetComponent<RectTransform>();
            _alphaComp = _activeObj.GetComponent<CanvasGroup>();
            _alphaComp.alpha = 0;
            _rt.localScale = Vector3.zero;

            closeBtn.onClick.AddListener(delegate
            {
                Hide();
            });
        }

        /// <summary>
        /// 设定打钩的状态
        /// </summary>
        public void SetStaterState()
        {
            StepItem _si = childStaters[GlobalUIManager.stepIndex];

            if (_si.gouList[GlobalUIManager.stepSubIndex].sprite != gouSelectIcon)
            {
                _si.gouList[GlobalUIManager.stepSubIndex].sprite = gouSelectIcon;
                _si.UpdateProgress();
            }
        }

        /// <summary>
        /// 设定步骤前色块的状态
        /// </summary>
        public void SetStepIconColorState()
        {
            StepItem _si = childStaters[GlobalUIManager.stepIndex];
            for (int i = 0; i < _si.stepColorIcon.Count; i++)
            {
                Image _img = _si.stepColorIcon[i];
                if (i == GlobalUIManager.stepSubIndex)
                {
                    _img.color = stepIconSelectColor;
                    _img.rectTransform.sizeDelta = new Vector2(9, 9);
                    _si.stepItemButtons[i].interactable = false;
                }
                else
                {
                    _img.color = stepIconNormalColor;
                    _img.rectTransform.sizeDelta = new Vector2(5, 5);
                    _si.stepItemButtons[i].interactable = true;
                }
            }
        }


        /// <summary>
        /// 显示的方法
        /// </summary>
        public void Show()
        {
            GlobalUIManager.Instance.UIControllState = GlobalUIManager.UI_CONTROLL_STATE.LOCK;
            _alphaComp.DOPause();
            _rt.DOPause();

            _rt.localScale = new Vector3(1, 0.9f,1);
            _alphaComp.DOFade(1, 0.6f);
            _rt.DOScaleY(1, 0.6f).SetEase(Ease.OutBack);
            _activeObj.SetActive(true);

            StepItem[] _Items = GetComponentsInChildren<StepItem>();
            for (int i = 0; i < _Items.Length; i++)
            {
                _Items[i].Stater.color = staterNormalColor;
                _Items[i].GoCloseState();
            }

            _Items[GlobalUIManager.stepIndex].Stater.color = staterSelectColor;
            _Items[GlobalUIManager.stepIndex].GoOpenState();
            ResetAllChildPos();

            Resize();
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            _alphaComp.DOFade(0, 0.3f);
            _rt.DOScaleY(0.9f, 0.3f).SetEase(Ease.InBack).OnComplete(delegate
            {
                _activeObj.SetActive(false);
                GlobalUIManager.Instance.UIControllState = GlobalUIManager.UI_CONTROLL_STATE.FREE;
            });
        }

        /// <summary>
        /// 创建所有的主步骤数据
        /// </summary>
        /// <returns>The creat.</returns>
        /// <param name="_datas">Datas.</param>
        public void Creat(List<StepData> _datas)
        {
            for (int i = 0; i < _datas.Count;i++)
            {
                GameObject _item = Instantiate(Resources.Load("StepItem")) as GameObject;
                _item.name = "StepItem_" + i;

                RectTransform _child = _item.GetComponent<RectTransform>();
                _child.SetParent(_itemParent);
                _child.localScale = Vector3.one;
                _child.anchoredPosition = new Vector2(0, i * -50);
                _child.sizeDelta = new Vector2(_itemParent.rect.width, 50);

                StepItem _si = _child.GetComponent<StepItem>();
                _si.Creat(_datas[i]);

                childStaters.Add(_si);
            }

            // 创建完成后，开始定位
            ResetAllChildPos();
        }

        /// <summary>
        /// 设定所有的子对象位置
        /// </summary>
        /// <returns>The all child position.</returns>
        public void ResetAllChildPos()
        {
            // 为下一个对象存储当前对象的位置和坐标
            int _pos = 0;
            for (int i = 0; i < _itemParent.childCount;i++)
            {
                RectTransform _child = _itemParent.GetChild(i).GetComponent<RectTransform>();
                _child.anchoredPosition = new Vector2(0, -_pos);
                _pos = (int)(_child.sizeDelta.y + Mathf.Abs(_child.anchoredPosition.y));
            }

            RectTransform _last = _itemParent.GetChild(_itemParent.childCount - 1).GetComponent<RectTransform>();
            int _height = (int)(Mathf.Abs(_last.anchoredPosition.y) + _last.sizeDelta.y);
            _itemParent.DOSizeDelta(new Vector2(_itemParent.sizeDelta.x, _height), 0.4f);

            // 设定子菜单对象的大小
            int _current = _height + 86;
            int _maxHeight = Screen.height - 150;
            if (_current > _maxHeight)
                _current = _maxHeight;
            GetComponent<RectTransform>().DOSizeDelta(new Vector2(GetComponent<RectTransform>().sizeDelta.x, _current), 0.4f);
        }

        /// <summary>
        /// 重置大小
        /// </summary>
        public void Resize()
        {
            for (int i = 0; i < _itemParent.childCount; i++)
            {
                RectTransform _childRT = _itemParent.GetChild(i).GetComponent<RectTransform>();
                _childRT.sizeDelta = new Vector2(_itemParent.rect.width, _childRT.sizeDelta.y);

                StepItem _si = _childRT.GetComponent<StepItem>();
                for (int j = 0; j < _si.subItemParent.childCount; j++)
                {
                    RectTransform _child = _si.subItemParent.GetChild(j).GetComponent<RectTransform>();
                    _child.sizeDelta = new Vector2(_childRT.rect.width, _child.sizeDelta.y);
                }
            }
        }
    }
}

