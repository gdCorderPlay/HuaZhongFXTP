using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJ
{
	public class StepPanelBase : MonoBehaviour
	{
        protected RectTransform _compareTarget;
        protected RectTransform _rt;

        void Awake()
        {
            _rt = GetComponent<RectTransform>();
        }

        public void ResetSize()
        {
            _rt.sizeDelta = new Vector2(_compareTarget.sizeDelta.x, _rt.sizeDelta.y);
        }
	}
}

