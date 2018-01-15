using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;

namespace WJ
{
    /// <summary>
    /// UI管理类
    /// </summary>
    public class GlobalUIManager : MonoBehaviour
    {
        #region 单例
        private static GlobalUIManager _instance;
        public static GlobalUIManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GlobalUIManager>();
                    if (_instance == null)
                    {
                        GameObject _go = new GameObject();
                        _go.name = "+ Singleton_GlobalUIManager";
                        _instance = _go.AddComponent<GlobalUIManager>();
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region 变量

        /// <summary>
        /// 主步骤索引
        /// </summary>
        public static int stepIndex = 0;

        /// <summary>
        /// 子步骤索引
        /// </summary>
        public static int stepSubIndex = 0;

        /// <summary>
        /// 临时的步骤数据
        /// </summary>
        public List<StepData> tempStepData = new List<StepData>()
        {
            new StepData("天平使用前的准备工作", new List<StepItemData>()
            {
                new StepItemData("接通电源"),
                new StepItemData("调整天平保持水平"),
                new StepItemData("启动天平"),
                new StepItemData("去皮，准备外部校正"),
                new StepItemData("准备校准砝码数据(校正键)"),
                new StepItemData("放置校准砝码")
            }),
            new StepData("开始测量",new List<StepItemData>()
            {
                new StepItemData("接通电源1"),
                new StepItemData("调整天平保持水平1"),
                new StepItemData("启动天平1"),
                new StepItemData("去皮，准备外部校正1"),
                new StepItemData("准备校准砝码数据(校正键)1"),
                new StepItemData("放置校准砝码1")
            }),
            new StepData("开始测量2",new List<StepItemData>()
            {
                new StepItemData("接通电源2"),
                new StepItemData("调整天平保持水平2"),
                new StepItemData("启动天平2"),
                new StepItemData("去皮，准备外部校正2"),
                new StepItemData("准备校准砝码数据(校正键)2"),
                new StepItemData("放置校准砝码222222")
            })
        };


        /// <summary>
        /// 所有的步骤数据
        /// </summary>
        private List<StepData> _allStepData;
        public List<StepData> AllStepData
        {
            get { return this._allStepData; }
        }

        /// <summary>
        /// UI的控制状态
        /// </summary>
        public enum UI_CONTROLL_STATE {
            /// <summary>
            /// 可移动状态
            /// </summary>
            FREE,
            /// <summary>
            /// 锁定状态
            /// </summary>
            LOCK }
        private UI_CONTROLL_STATE _uiControllState = UI_CONTROLL_STATE.FREE;
        public UI_CONTROLL_STATE UIControllState
        {
            get { return this._uiControllState; }
            set { this._uiControllState = value; }
        }


        /// <summary>
        /// 步骤执行状态 记录最新的状态
        /// </summary>
        public enum STEP_CONTROLL_STATE { Auto, Click }
        private STEP_CONTROLL_STATE _stepControllState = STEP_CONTROLL_STATE.Auto;
        public STEP_CONTROLL_STATE StepControllState
        {
            get { return this._stepControllState; }
            set { this._stepControllState = value; }
        }

        /// <summary>
        /// 更新目标步骤数据 事件 用来负责步骤数据的更新
        /// 参数 1 大步骤计数 
        /// 参数 2 小步骤计数
        /// 参数 3 步骤控制状态
        /// </summary>
        public static Action<int, int, STEP_CONTROLL_STATE> StepDataChange;

        /// <summary>
        /// 主相机
        /// </summary>
        [SerializeField] private Camera mainCam;
        public Camera MainCam
        {
            get { return this.mainCam; }
        }

        /// <summary>
        /// 初始时的整体遮罩，初始化完成后自动删除
        /// </summary>
        [SerializeField] private Image startMasker;

        #endregion

        #region 初始化步骤数据

        /// <summary>
        /// 初始化步骤数据
        /// </summary>
        /// <param name="_datas">Datas.</param>
        public void InitUIData(List<StepData> _datas, Action _callback = null)
        {
            this._allStepData = _datas;
            StepSplashPanel _ssp = FindObjectOfType<StepSplashPanel>();
            if (_ssp == null)
            {
                GameObject _panel = Instantiate(Resources.Load("StepSplashPanel")) as GameObject;
                _panel.name = "StepSplashPanel";
                RectTransform _rt = _panel.GetComponent<RectTransform>();
                _rt.SetParent(GameObject.Find("Canvas/GroupPanel").transform);
                _rt.localScale = Vector3.one;
                _rt.anchoredPosition = new Vector2(0, -100);
                _ssp = _panel.GetComponent<StepSplashPanel>();
                _ssp.InitStepItem();
            }

            // 创建步骤数据
            StepPanel _sp = _ssp.gameObject.GetComponentInChildren<StepPanel>();
            _sp.Creat(_datas);

//            // 初始化完成后的回调
//            if (_callback != null)
//                _callback();

            // 初始化完成后执行初始显示动画
			StartCoroutine(ShowNormalUIPanel(_ssp,_callback));
        }



        #endregion

        #region 步骤完成相关
        /// <summary>
        /// 步骤切换时，返回步骤索引
        /// </summary>
		public	int _prevIndex = int.MaxValue;
		public	int _prevSubIndex = int.MaxValue;

        /// <summary>
        /// 变化菜单栏的方法
        /// </summary>
        /// <param name="_step">准备切换到的菜单</param>
        /// <param name="_changeState"></param>
        public void ChangeStepIndexValue(int _step, STEP_CONTROLL_STATE _changeState)
        {
			if (_changeState == STEP_CONTROLL_STATE.Click && _step != 0) 
			{
				// 暂存当前的步骤索引
				_prevIndex = stepIndex;
				_prevSubIndex = stepSubIndex;

				// 计算
				stepSubIndex += _step;

				CompareStepIndex ();

//				if (WholeDataManager.getInstance.goStepEnum == WholeDataManager.GoStepEnum._正在准备操作) {
//					MyLogger.Instance.Log (TextConfigUtility.getInstance.PrepareingInfoLog, delegate {
//						stepIndex = _prevIndex;
//						stepSubIndex = _prevSubIndex;
//
//					});
//					return;
//				}
//
//				if (WholeDataManager.getInstance.goStepEnum == WholeDataManager.GoStepEnum._已完成准备操作)
//				{
//					if (stepIndex == 0) {
//					
//						MyLogger.Instance.Log (TextConfigUtility.getInstance.NoRepeatPrepareInfoLog, delegate {
//							stepIndex = _prevIndex;
//							stepSubIndex = _prevSubIndex;
//
//						});
//						return;
//					}
////					else
////						StepChangeEvent ();
//
//						
//
//				}

			}
			else 
			{
				this.StepControllState = _changeState;

				// 设定打钩事件
				if (StepControllState == STEP_CONTROLL_STATE.Auto) {
					StepPanel sp = FindObjectOfType<StepPanel> ();
					if (sp != null)
						sp.SetStaterState ();
				}
          
				// 暂存当前的步骤索引
				_prevIndex = stepIndex;
				_prevSubIndex = stepSubIndex;
				// 计算
				stepSubIndex += _step;
				CompareStepIndex ();
			}
			//步骤切换完成后的事件
			StepChangeEvent ();
        }

        
        /// <summary>
        /// 计算步骤索引 封装了计算下一个
        /// </summary>
		private void CompareStepIndex()
        {
            //改子步骤为当前大步骤的最后一个步骤
            if (stepSubIndex > AllStepData[stepIndex].items.Count - 1)
            {
                //不是最后一个大步骤 大步骤的计数递增至下一个大步骤 子步骤变为下一个的第一步骤
                if (stepIndex < AllStepData.Count - 1)
                {
                    stepIndex++;
                    stepSubIndex = 0;
                }
                else //表示当前为最后一步的最后一个小步骤 无法再向后面递进
                {
                    stepIndex = AllStepData.Count - 1;
                    stepSubIndex = AllStepData[stepIndex].items.Count - 1;
                }
            }
            //在返回的过程中小于零表示 从当前的大步骤的第一步开始返回
            if (stepSubIndex < 0)
            {
                //如果当前的大步骤不是第一个步骤 那么返回上一个步骤的最后一步
                if (stepIndex > 0)
                {
                    stepIndex--;
                    stepSubIndex = AllStepData[stepIndex].items.Count - 1;
                }
                else//表示当前是第一个大步骤的第一步 无法再次返回
                {
                    stepIndex = 0;
                    stepSubIndex = 0;
                }
            }
//            // 步骤切换完成后的事件
//            StepChangeEvent();
        }

        /// <summary>
        /// 步骤切换的事件
        /// </summary>
        public void StepChangeEvent()
        {
			InfoPanel infoPanel = GameObject.FindObjectOfType<InfoPanel> ();
			if (infoPanel != null)
			{
				if (infoPanel._isShowed) {
					infoPanel.Hide ();
				}
			}
            // 大步骤切换后变换大步骤标题
            if (_prevIndex != stepIndex)
                ChangeStepMainTitle(AllStepData[stepIndex].title);

            // 切换步骤文本的显示
	            if (_prevIndex != stepIndex || _prevSubIndex != stepSubIndex)
                FindObjectOfType<StepSplashPanel>().ChangeStepItemTitle();

            // 发送消息
            if (StepDataChange != null )
                StepDataChange(stepIndex, stepSubIndex, StepControllState);

            // 重置步骤跳转状态
            StepControllState = STEP_CONTROLL_STATE.Auto;

			// 暂存当前的步骤索引
			_prevIndex = stepIndex;
			_prevSubIndex = stepSubIndex;
        }

        #endregion

        #region 一般方法
        /// <summary>
        /// 设置主步骤的标题内容
        /// </summary>
        /// <param name="_title"></param>
        private void ChangeStepMainTitle(string _title)
        {
            StepMainTitleBar _smtb = FindObjectOfType<StepMainTitleBar>();
            if(_smtb == null){
                GameObject _go = Instantiate(Resources.Load("StepMainTitleBar")) as GameObject;
                _go.name = "StepMainTitleBar";

                RectTransform _rt = _go.GetComponent<RectTransform>();
                _rt.SetParent(GameObject.Find("Canvas/GroupPanel").transform);
                _rt.anchoredPosition = new Vector2(0, -90);
                _smtb = _go.GetComponent<StepMainTitleBar>();
            }
            _smtb.SetTitle(_title);
        }
        /// <summary>
        /// 外部设置主步骤标题的方法
        /// </summary>
        /// <param name="_title"></param>
        public void SetMainStepTitle(float _title)
        {
            
            ChangeStepMainTitle(string.Format("{0}(目标重量{1}g)", AllStepData[stepIndex].title, _title));
        }
        /// <summary>
        /// 刷新主步骤显示
        /// </summary>
        public void SetMainStepTitle()
        {
            ChangeStepMainTitle(AllStepData[stepIndex].title);
        }
		private IEnumerator ShowNormalUIPanel(StepSplashPanel _ssp,Action _callBack)
        {
            mainCam.DOFieldOfView(40, 2).SetDelay(0.2f);
            startMasker.gameObject.SetActive(true);
            startMasker.DOFade(0,3).SetDelay(0.5f).OnComplete(delegate {
                Destroy(startMasker.gameObject);
            });
            // 显示大步骤标题
            yield return new WaitForSeconds(2);
            ChangeStepMainTitle(AllStepData[stepIndex].title);
            // 显示步骤面板
            _ssp.Show();
			if (_callBack != null)
				_callBack ();
        }
        #endregion

    }
}
