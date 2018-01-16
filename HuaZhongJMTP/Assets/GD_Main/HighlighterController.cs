using UnityEngine;
using HighlightingSystem;
//using ExperimentFrame;
using UnityEngine.EventSystems;
    public class HighlighterController : MonoBehaviour
    {
        protected Highlighter highlighter;
        //private bool isDown = false;

		protected void Awake()
		{
			highlighter = GetComponent<Highlighter>();
			if (highlighter == null)
			{
				highlighter = gameObject.AddComponent<Highlighter>();
			}
		}

        protected void OnEnable()
        {
			gameObject.GetComponent<Highlighter> ().enabled = true;
        }

		protected void OnDisable()
		{
			gameObject.GetComponent<Highlighter> ().enabled = false;
		}

        private void OnMouseOver()
        {
		if (IsOnUI)
                return;
            highlighter.On();
        }
   
    public bool IsOnUI
    {
        get
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {

                return true;
            }
            else
            {
                return false;
            }
        }
    }

    }
