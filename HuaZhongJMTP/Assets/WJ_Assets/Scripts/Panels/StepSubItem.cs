using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WJ
{
    /// <summary>
    /// 子菜单按钮  点击菜单跳转在本脚本中
    /// </summary>
	public class StepSubItem : MonoBehaviour
	{
        private Button btn;

        private void Start()
        {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(delegate {
                OnClick();
//				if (WholeDataManager.getInstance.isLMPlayeing)
//					return;
//                StepItem _si = transform.GetComponentInParent<StepItem>();
//                int _index = int.Parse(_si.gameObject.name.Split(new char[] { '_' })[1]);
//                int _subIndex = int.Parse(gameObject.name.Split(new char[] { '_' })[1]);
////				if(!WholeDataManager.getInstance.isTest)
////				{
////				if (WholeDataManager.getInstance.goStepEnum == WholeDataManager.GoStepEnum._正在准备操作) 
////				{
////						MyLogger.Instance.Log (TextConfigUtility.getInstance.PrepareingInfoLog, delegate {
////						
////					});
////					return;
////				}
////
////				if (WholeDataManager.getInstance.goStepEnum == WholeDataManager.GoStepEnum._已完成准备操作) {
////					if (_index == 0) {
////
////							MyLogger.Instance.Log (TextConfigUtility.getInstance.NoRepeatPrepareInfoLog, delegate {
////							
////						});
////						return;
////					}
////
////				}
////				}
//                GlobalUIManager.stepIndex = _index;
//                GlobalUIManager.stepSubIndex = _subIndex;
//               // GlobalUIManager.Instance.StepChangeEvent();
//              // FXTPMainManager.Instance.
//                GetComponentInParent<StepPanel>().Hide();
            });
        }
        /// <summary>
        /// 当子菜单被点击时触发
        /// </summary>
        void OnClick()
        {
            if (FXTPMainManager.Instance.currentState.mouseClock) return;
            StepItem _si = transform.GetComponentInParent<StepItem>();
            int _index = int.Parse(_si.gameObject.name.Split(new char[] { '_' })[1]);
            int _subIndex = int.Parse(gameObject.name.Split(new char[] { '_' })[1]);
            if (GlobalUIManager.stepIndex==_index&& GlobalUIManager.stepSubIndex== _subIndex)
            {//点选的是当前步骤 跳过
                GetComponentInParent<StepPanel>().Hide();
                return;
            }
            if (GlobalUIManager.stepIndex == 0)
            {
                //当前是准备阶段的步骤无法跳过
                FXTPMainManager.Instance.SetMouseClock(true);
                MyLogger.Instance.Log(XMLManager.Instance.strConfigDir["PrepareingInfoLog"], 350, MyLogger.TextAlign.Center,delegate
                {
                    FXTPMainManager.Instance.SetMouseClock(false);
                });
                GetComponentInParent<StepPanel>().Hide();
                return;
            }
            if(GlobalUIManager.stepIndex == _index && GlobalUIManager.stepSubIndex != _subIndex)
            {
                int step = _subIndex - GlobalUIManager.stepSubIndex;
                GlobalUIManager.stepIndex = _index;
                GlobalUIManager.stepSubIndex = _subIndex;
                // 跳步
                FXTPMainManager.Instance.ChangeNextState(step, GlobalUIManager.STEP_CONTROLL_STATE.Click);
                GetComponentInParent<StepPanel>().Hide();
            }
           
            // GlobalUIManager.Instance.StepChangeEvent();
            // FXTPMainManager.Instance.
           
        }
    }
}

