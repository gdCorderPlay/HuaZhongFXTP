using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WJ
{
    /// <summary>
    /// 菜单信息
    /// </summary>
    public class StepItem : MonoBehaviour
    {
        /// <summary>
        /// 子条目的父对象
        /// </summary>
        public RectTransform subItemParent;

        /// <summary>
        /// 选择状态的对象
        /// </summary>
        private Image stater;
        public Image Stater
        {
            get { return this.stater; }
        }

        private Button popBtn;
        private RectTransform _rt;
        private StepData _currentData;


        /// <summary>
        /// 开关的状态
        /// </summary>
        private enum PopState { Open,Close }
        private PopState _popState = PopState.Close;

        /// <summary>
        /// 状态图标的数组
        /// </summary>
        [HideInInspector]public List<Image> gouList = new List<Image>();

        /// <summary>
        /// 步骤前颜色方块的数组
        /// </summary>
        [HideInInspector]public List<Image> stepColorIcon = new List<Image>();

        [HideInInspector]public List<Button> stepItemButtons = new List<Button>();

        [SerializeField] private Transform progbar;
        [SerializeField] private Text progLabel;

        [HideInInspector] public int doneLength = 0;


        /// <summary>
        /// 初始化
        /// </summary>
        private void Start()
        {
            _rt = GetComponent<RectTransform>();
            subItemParent.gameObject.SetActive(false);
            stater = transform.Find("MainTitle/Stater").GetComponent<Image>();
            progbar.localScale = new Vector3(0, 1, 1);
            popBtn = transform.Find("MainTitle/PopBtn").GetComponent<Button>();
            popBtn.onClick.AddListener(delegate
            {
                PopBtnClicked();
            });
        }


        /// <summary>
        /// 状态按钮的点击事件
        /// </summary>
        private void PopBtnClicked()
        {
            switch(_popState)
            {
                case PopState.Open: GoCloseState(); break;
                case PopState.Close: GoOpenState(); break;
                default:break;
            }

            // 重置所有子对象的位置
            GetComponentInParent<StepPanel>().ResetAllChildPos();
        }
        /// <summary>
        /// 更新显示数据
        /// </summary>
        public void UpdateProgress()
        {
            doneLength++;
            float _prog = (float)doneLength / (float)gouList.Count;
            _prog = Mathf.Clamp(_prog, 0, 1f);
            progbar.localScale = new Vector3(_prog, 1, 1);
            float _tmp = _prog * 100;
            progLabel.text = _tmp.ToString("0.0") + "%";
        }


        /// <summary>
        /// 进入展开状态
        /// </summary>
        public void GoOpenState()
        {
            _popState = PopState.Open;
            subItemParent.gameObject.SetActive(true);
            _rt.sizeDelta = new Vector2(_rt.sizeDelta.x, _currentData.items.Count * 40 + 50);
        }

        /// <summary>
        /// 进入合起状态
        /// </summary>
        public void GoCloseState()
        {
            _popState = PopState.Close;
            subItemParent.gameObject.SetActive(false);
            _rt.sizeDelta = new Vector2(_rt.sizeDelta.x, 50);
        }

        /// <summary>
        /// 具体的创建方法
        /// </summary>
        /// <returns>The creat.</returns>
        /// <param name="_data">Data.</param>
        public void Creat(StepData _data)
        {
            // 暂存本步骤的数据
            this._currentData = _data;
            
            // 设定主标题
            Text _mainTitle = this.transform.Find("MainTitle/TitleLabel").GetComponent<Text>();
            _mainTitle.text = _data.title;

            // 创建所有子对象
            for (int i = 0; i < _data.items.Count; ++i)
            {
                GameObject _item = Instantiate(Resources.Load("StepSubItem")) as GameObject;
                _item.name = "StepSubItem_" + i;

                RectTransform _childRT = _item.GetComponent<RectTransform>();
                _childRT.SetParent(subItemParent);
                _childRT.localScale = Vector3.one;

                Text _label = _childRT.Find("Center/Text").GetComponent<Text>();
                _label.text = _data.items[i].title;

                Image _gou = _childRT.Find("Right/Icon").GetComponent<Image>();
                gouList.Add(_gou);

                Image _icon = _childRT.Find("Left/Rect").GetComponent<Image>();
                stepColorIcon.Add(_icon);

                stepItemButtons.Add(_item.GetComponent<Button>());
            }
        }
    }
}