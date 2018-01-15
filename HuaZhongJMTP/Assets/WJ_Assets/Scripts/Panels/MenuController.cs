using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace WJ
{
    /// <summary>
    /// 菜单管理类
    /// </summary>
    public class MenuController : MonoBehaviour
    {
        /// <summary>
        /// 所有的菜单按钮的数组
        /// </summary>
        public Button[] menuBtns;

        /// <summary>
        /// 初始化
        /// </summary>
        private void Start()
        {
            for (int i = 0; i < menuBtns.Length;i++)
            {
                Button _btn = menuBtns[i];
                _btn.onClick.AddListener(delegate {
                    MenuBtnClicked(_btn);
                });
            }
        }

        /// <summary>
        /// 菜单按钮的点击事件
        /// </summary>
        /// <param name="_btn">Button.</param>
        private void MenuBtnClicked(Button _btn)
        {
            if (FXTPMainManager.Instance.currentState.mouseClock) 
                return;
            if (_btn.gameObject.name == "MenuBtn_2")
                return;
            if (GlobalUIManager.Instance.UIControllState == GlobalUIManager.UI_CONTROLL_STATE.LOCK)
                return;
            
            for (int i = 0; i < menuBtns.Length; i++)
            {
                Button _b = menuBtns[i];
                _b.interactable = true;
            }

            switch(_btn.gameObject.name)
            {
                case "MenuBtn_0": ShowDoc(false); break;
				case "MenuBtn_0_FXTP": ShowDoc(true); break;
                case "MenuBtn_1": ShowStepPanel(false)
                        
                        ; break;
				case "MenuBtn_1_FXTP": ShowStepPanel(true); break;
               // case "MenuBtn_2": ShowSetterPanel(); break;
                default:break;
            }

            _btn.interactable = false;
        }

        /// <summary>
        /// 显示天平说明
        /// </summary>
		private void ShowDoc(bool _bool)
        {
           
            //			if (WholeDataManager.getInstance.stepEnum == WholeDataManager.StepEnum._调平) 
            //			{
            //				ModelConfigUtility.getInstance.BalanceLeft.raycastTarget = false;
            //				ModelConfigUtility.getInstance.BalanceRight.raycastTarget = false;
            //				ModelConfigUtility.getInstance.BalancePanel.DOFade (0, 0f);
            //				ModelConfigUtility.getInstance.BalancePanel.transform.GetChild (0).DOScale (Vector3.zero, 0.5f);
            //			}
            //			WholeDataManager.getInstance.cameraEnum = WholeDataManager.CameraEnum._详细;	
            StepSplashPanel _ssp = FindObjectOfType<StepSplashPanel>();
            if (_ssp != null && _ssp.IsShow)
                _ssp.Hide();

            StepMainTitleBar _smtb = FindObjectOfType<StepMainTitleBar>();
            if (_smtb != null && _smtb.IsShowed)
                _smtb.Hide();
            
            TipController _tc = FindObjectOfType<TipController>();
            if (_tc != null && !_tc.IsShowed)
				_tc.Show(_bool);
        }

        /// <summary>
        /// 显示步骤
        /// </summary>
		private void ShowStepPanel(bool _bool)
        {
//			if (WholeDataManager.getInstance.stepEnum == WholeDataManager.StepEnum._调平) 
//			{
//				StartCoroutine (JMTP_StepInit_CompleteManager.getInstance.Step_Balance_Init (false));
//			}
//
//			WholeDataManager.getInstance.cameraEnum = WholeDataManager.CameraEnum._操作;	
            TipController _tc = FindObjectOfType<TipController>();
            if (_tc != null && _tc.IsShowed)
            {
                //if (FXTPMainManager.Instance.currentStateType == FXTPMainManager.StateType.调整水平)
                //{
                //    FXTPMainManager.Instance.ChangeNextState(0, GlobalUIManager.STEP_CONTROLL_STATE.Click);
                //}
				_tc.Hide(_bool);
                StartCoroutine(ShowStepPanel(1));
            }
            else
            {
                StartCoroutine(ShowStepPanel(0));
                StepMainTitleBar _smtb = FindObjectOfType<StepMainTitleBar>();
                if (_smtb != null && !_smtb.IsShowed)
                    _smtb.Show();
            }
        }

        private IEnumerator ShowStepPanel(float _yieldTime)
        {
            yield return new WaitForSeconds(_yieldTime);

            StepSplashPanel _sp = FindObjectOfType<StepSplashPanel>();
            if (_sp != null)
            {
                if (!_sp.IsShow)
                    _sp.Show();
            }

            StepMainTitleBar _smtb = FindObjectOfType<StepMainTitleBar>();
            if (_smtb != null && !_smtb.IsShowed)
                _smtb.Show();
        }

        /// <summary>
        /// 显示设置面板
        /// </summary>
        private void ShowSetterPanel()
        {
            StepSplashPanel _ssp = FindObjectOfType<StepSplashPanel>();
            if (_ssp != null && _ssp.IsShow)
                _ssp.Hide();

            StepMainTitleBar _smtb = FindObjectOfType<StepMainTitleBar>();
            if (_smtb != null && _smtb.IsShowed)
                _smtb.Hide();

            TipController _tc = FindObjectOfType<TipController>();
            //if (_tc != null && _tc.IsShowed)
                //_tc.Hide();
        }

    }
}