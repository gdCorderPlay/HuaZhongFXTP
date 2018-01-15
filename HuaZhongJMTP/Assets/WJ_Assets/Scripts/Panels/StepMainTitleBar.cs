using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace WJ {
	public class StepMainTitleBar : MonoBehaviour
	{
		[SerializeField]private GameObject activeObj;
		[SerializeField]private Text label;
		private bool _isShowed = false;
        public bool IsShowed
        {
            get { return this._isShowed; }
        }
		private RectTransform _rt;

		void Awake()
		{
			activeObj.SetActive(false);
			_rt = GetComponent<RectTransform>();

			_rt.localScale = Vector3.one * 0.6f;
			_isShowed = false;
		}


		public void SetTitle(string _title)
		{
			StartCoroutine("StartChange",_title);
		}

		private IEnumerator StartChange(string _title)
		{
			if(_isShowed)
			{
				Hide();
				yield return new WaitForSeconds(0.5f);
            }

            label.text = _title;
            int _w = (int)(label.preferredWidth + 160);
            if (_w < 250)
                _w = 250;
            _rt.sizeDelta = new Vector2(_w, _rt.sizeDelta.y);

			Show();
		}

        public void Show()
		{
            activeObj.SetActive(true);
			CanvasGroup _cg = activeObj.GetComponent<CanvasGroup>();
			activeObj.transform.DOPause();
			_cg.DOPause();
            _rt.DOPause();

			_cg.DOFade(1,0.8f);
			_rt.DOScale(Vector3.one,0.8f).SetEase(Ease.OutBack);
			_isShowed = true;
		}

		public void Hide()
		{
            CanvasGroup _cg = activeObj.GetComponent<CanvasGroup>();
            activeObj.transform.DOPause();
            _rt.DOPause();

            _cg.DOFade(0,0.3f);
			_rt.DOScale(Vector3.one * 0.6f,0.3f).SetEase(Ease.InBack).OnComplete(delegate{
				activeObj.SetActive(false);
			});
			_isShowed = false;
		}
	}
}