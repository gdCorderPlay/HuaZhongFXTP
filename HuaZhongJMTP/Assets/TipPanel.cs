using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace WJ
{
	public class TipPanel: MonoBehaviour
    {
        [SerializeField] private Text Label;
        [SerializeField] private GameObject _activeObj;
        private CanvasGroup _alphaComp;
        private RectTransform _rt;
        private bool _isShowed = false;
        public bool IsShowed 
        { 
            get { return this._isShowed; } 
        }

        private void Awake()
        {
            _rt = GetComponent<RectTransform>();
            _rt.localScale = Vector3.one * 0.6f;
            _alphaComp = GetComponent<CanvasGroup>();
            _alphaComp.alpha = 0;
        }

        public void ShowTip(string _str)
        {
            Label.text = _str;
            _rt.sizeDelta = new Vector2(Label.preferredWidth + 60, _rt.sizeDelta.y);
            Show();
        }

        private void Show()
        {
            _rt.SetAsLastSibling();
            _rt.DOPause();
            _alphaComp.DOPause();

            _activeObj.SetActive(true);
            _alphaComp.DOFade(1,0.3f);
            _rt.DOScale(Vector3.one,0.3f).SetEase(Ease.OutBack);
            _isShowed = true;
        }

        public void Hide()
        {
            _rt.DOPause();
            _alphaComp.DOPause();
            
            _rt.DOScale(Vector3.one * 0.6f, 0.2f).SetEase(Ease.InBack);
            _alphaComp.DOFade(0,0.2f).OnComplete(delegate
            {
                _activeObj.SetActive(false);
            });
            _isShowed = false;
        }

        public void Follow(Vector2 _pos)
        {
            _rt.anchoredPosition = _pos;
        }

		public bool IsActiveObj()
		{
			if (_activeObj.activeInHierarchy)
				return true;
			else
				return false;
		}
    }
}

