using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

namespace WJ
{
	public class InputerPanel : MonoBehaviour
	{
		private static InputerPanel _instance;
		public static InputerPanel Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = FindObjectOfType<InputerPanel>();
					if(_instance == null)
					{
						GameObject _go = Instantiate(Resources.Load("InputerPanel")) as GameObject;
						_go.name = "InputerPanel";
						RectTransform _RT = _go.GetComponent<RectTransform>();

						_RT.SetParent(GameObject.Find("Canvas/GroupPanel").transform);
						_RT.localEulerAngles = Vector3.zero;
						_RT.anchoredPosition3D = Vector3.zero;
						_instance = _go.GetComponent<InputerPanel>();
					}
				}
				return _instance;
			}
		}
		

		[SerializeField] private InputField _text = null;
		[SerializeField] private GameObject _activeObj = null;
		[SerializeField] private CanvasGroup _alphaComp = null;
		[SerializeField] private Button _enterBtn = null;
		private System.Action<float> _callback = null;
		private RectTransform _rt = null;

        private string lastStr;

		void Awake()
		{
			if(_rt == null)
				_rt = GetComponent<RectTransform>();
			_rt.localScale = Vector3.one * 0.5f;
            //_text.onEndEdit.AddListener ; //测验
         // _text.onValidateInput += test;
           _text.onValueChanged.AddListener(onValueChanged) ;
            _enterBtn.onClick.AddListener(delegate{
				Hide();
			});
		}
        string[] inputs;
        private void onValueChanged(string inputText)
        {

            // if( inputText.Contains("."))
            // {
           // string _str = "";
           // string[] _tmp = inputText.Split('.');

          //  _str += _tmp[0];
           // if(_tmp[1].Length > 2)
          //  {
                
          //  }
            //inputText
            // MatchCollection mc =;
            //  Debug.Log(inputText);
            inputs = inputText.Split('.');
            if (inputs.Length > 2)
            {
                _text.text = lastStr;
                return;
            }
            if (inputs.Length == 2)
            {
                if (inputs[1].Length > 3)
                {
                    _text.text = lastStr;
                    return;
                }
            }
            lastStr = inputText;
            //if(Regex.IsMatch(inputText, "^[0-9]([.][0-9]{0,3})?$")) {
            //    lastStr = inputText;
            //}

            //else
            //{
            //    _text.text = lastStr;
            //}
        }

       
        char test(string text,int index,char lastcar) //测验
        {
            //  int pointIndex = _text. text.IndexOf('.');
            int pointIndex = text.Split('.').Length;
            int count =_text. text.Length;
            if (count > 8)//限制长度
                return '\0';
            if (lastcar == '.') //小数点个数限制
            {
                if(pointIndex>=2) //已经存在小数点
                    return '\0';
                if (count - index > 4) //不能插在倒数第三位之后

                    return '\0';

                return lastcar;
            }
           if((int)lastcar>=48&& (int)lastcar <= 57) //数字
            {
                if (pointIndex >= 2)
                {
                   // int pointIndex = text.IndexOf('.');

                    if(index>=pointIndex&&count-pointIndex>=4)//统计小数点后已经有的个数
                    {
                        return '\0';
                    }
                }
                    return lastcar;
            }
            return '\0';
        }
       
        void EndEdit() //测验
        {

            //Debug.Log("ceshi ");
        }

		public void ShowInputer(System.Action<float> _callback)
		{
			if(_rt == null)
				_rt = GetComponent<RectTransform>();
			this._callback = _callback;
			Show();
           
        }

		private void Show()
		{
			_rt.DOPause();
			_alphaComp.DOPause();
            _text.text = "";
           

            _activeObj.SetActive(true);
			_rt.DOScale(Vector3.one,0.6f).SetEase(Ease.OutBack);
			_alphaComp.DOFade(1,0.6f);
		}

		private void Hide()
		{
            
            _rt.DOPause();
			_alphaComp.DOPause();

			_alphaComp.DOFade(0,0.3f);
			_rt.DOScale(Vector3.one * 0.5f,0.3f).SetEase(Ease.InBack).OnComplete(delegate{
				_activeObj.SetActive(false);
               // decimal.Round(decimal.Parse("0.3333333"), 2);
                float inputNum;
                // float.TryParse(_text.text, out inputNum);
                if (string.IsNullOrEmpty(_text.text))
                {
                    _callback(0);
                }
                else
                {
                    inputNum = (float)decimal.Round(decimal.Parse(_text.text), 3);

                    _callback(inputNum);
                }
			});
		}
	}
}
