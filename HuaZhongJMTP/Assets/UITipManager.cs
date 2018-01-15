using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WJ
{
    public class UITipManager : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
	{
        [SerializeField] private string tipMessage = "";
        [SerializeField] private float waitTime = 1;
        private UITipPanel uiTipPanel = null;


        public void OnPointerEnter(PointerEventData _ed)
        {
            Invoke("WaitShow",waitTime);
        }

        private void WaitShow()
        {
            if (uiTipPanel == null) uiTipPanel = TipPanel;
            uiTipPanel.ShowUITip(tipMessage);
        }

        public void OnPointerExit(PointerEventData _ed)
        {
            CancelInvoke("WaitShow");

            if (uiTipPanel == null) uiTipPanel = TipPanel;
            uiTipPanel.Hide();
        }

        void Update()
        {
            if(uiTipPanel && uiTipPanel.IsShow)
                uiTipPanel.Follow(Input.mousePosition);

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                CancelInvoke("WaitShow");
        }


        private UITipPanel TipPanel
        {
            get
            {
                if (uiTipPanel == null)
                {
                    uiTipPanel = FindObjectOfType<UITipPanel>();
                    if (uiTipPanel == null)
                    {
                        GameObject _go = Instantiate(Resources.Load("UITipPanel")) as GameObject;
                        _go.name = "UITipPanel";
                        RectTransform _rt = _go.GetComponent<RectTransform>();
                        _rt.SetParent(GameObject.Find("Canvas/GroupPanel").transform);
                        uiTipPanel = _go.GetComponent<UITipPanel>();
                    }
                }

                return uiTipPanel;
            }
        }
	}
}

