using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace WJ
{
	public class UITipPanel : MonoBehaviour
	{
        [SerializeField] private Text label = null;
        [SerializeField] private GameObject _activeObj;
        private RectTransform _rt;
        private CanvasGroup _alphaComp = null;
        private bool _isShow = true;
        public bool IsShow
        {
            get { return this._isShow; }
        }

        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
            _alphaComp = _activeObj.GetComponent<CanvasGroup>();
        }

        public void ShowUITip(string _str)
        {
            label.text = _str;
            _rt.sizeDelta = new Vector2(label.preferredWidth + 30, _rt.sizeDelta.y);
            Show();
        }

        public void Follow(Vector2 _pos)
        {
            _rt.anchoredPosition = new Vector2(_pos.x,_pos.y + 10);
        }

        private void Show()
        {
            _rt.SetAsLastSibling();
            _rt.DOPause();
            _alphaComp.DOPause();

            _activeObj.SetActive(true);
            _rt.DOScale(Vector3.one,0.4f).SetEase(Ease.OutBack);
            _alphaComp.DOFade(1,0.4f);
            _isShow = true;
        }

        public void Hide()
        {
            _rt.DOPause();
            _alphaComp.DOPause();

            _rt.DOScale(Vector3.one * 0.5f,0.1f).SetEase(Ease.InBack);
            _alphaComp.DOFade(0,0.1f).OnComplete(delegate {
                _isShow = false;
                _activeObj.SetActive(false);
            });
        }

	}
}

