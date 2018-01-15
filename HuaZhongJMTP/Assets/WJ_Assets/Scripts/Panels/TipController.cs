using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace WJ
{
	public class TipController : MonoBehaviour
	{
        [SerializeField]private CanvasGroup _alphaComp;
        [HideInInspector] public bool IsShowed = false;
        /// <summary>
        /// 记录切换前的照相机切换数据
        /// </summary>
      public  class cameraData
        {
            public Vector3 target;
            public bool isAuto;
            public float currentDistance;
            public float XDeg;
            public float YDeg;
            public bool mouseClock;
        }

        private cameraData lastCameraData=new cameraData();
        private void Awake()
        {
            for (int i = 0; i < _alphaComp.transform.childCount; i++)
            {
                Transform _tf = _alphaComp.transform.GetChild(i);
                _tf.localScale = Vector3.zero;
            }
            _alphaComp.alpha = 0;
            _alphaComp.gameObject.SetActive(false);
        }


		public void Show(bool isFXTP)
        {
            _alphaComp.DOPause();
            _alphaComp.gameObject.SetActive(true);
            _alphaComp.DOFade(1,1);

            for (int i = 0; i < _alphaComp.transform.childCount;i++)
            {
                Transform _tf = _alphaComp.transform.GetChild(i);
                _tf.DOPause();
                _tf.localScale = Vector3.zero;
                _tf.DOScale(Vector3.one, 0.7f).SetDelay(i * 0.05f).SetEase(Ease.OutBack);
            }

            CameraViewer _cv = GlobalUIManager.Instance.MainCam.GetComponent<CameraViewer>();
			//Transform _target = _cv.target;
            if(_cv != null)
            {
//                _cv.enabled = false;
//               // _cv.xDeg = 0;
//               // _cv.yDeg = 0;
//				_cv.transform.DOLocalMove(new Vector3(0.1507869f, 0.07485636f, -0.3218957f),1);
//				_cv.transform.DOLocalRotate(new Vector3(4, -25.1f,0),1).OnComplete(delegate {
//                    _cv.enabled = true;
//                });
				if(!isFXTP)
				{
                    lastCameraData.isAuto = _cv.IsAutoChangeView;
                    lastCameraData.target = _cv.target.position;
                    lastCameraData.XDeg = _cv.xDeg;
                    lastCameraData.YDeg = _cv.yDeg;
                    lastCameraData.currentDistance = _cv.currentDistance;
                    // lastCameraData.mouseClock = FXTPMainManager.Instance.currentState.mouseClock;
                    //_cv.ChangeCameraView (-35.2f, 4f, 0.447f, 0.5f, delegate {
                    //	IsShowed = true;
                    //	return true;
                    FXTPMainManager.Instance.isShowDes = true;
                    //  _cv.ChangeCameraView(SingleModel.Model_FXTP.position+Vector3.up*0.12f,0, 27f, 0.6f, 0.5f, delegate //之前的角度
                    _cv.ChangeCameraView(SingleModel.Model_FXTP.position + Vector3.up * 0.04f, -15, 13f, 0.35f, 0.5f, delegate
                    {
                       
                        IsShowed = true;
                        return true;

                    });
//				_cv.ChangeCameraView (ModelConfigUtility.getInstance.testTarget,-51.2f, 4f, 0.6f, 1f, delegate {
//					return true;
//					IsShowed = true;
//				});
				}
				//else
//					_cv.ChangeCameraView (ModelConfigUtility.getInstance.FXTPTarget,-35.2f, 4f, 0.447f, 0.5f, delegate {
//						IsShowed = true;
//						return true;
//
//					});
            }

            
        }


		public void Hide(bool isFXTP)
        {
			CameraViewer _cv = GlobalUIManager.Instance.MainCam.GetComponent<CameraViewer>();
            _alphaComp.DOPause();
            for (int i = 0; i < _alphaComp.transform.childCount; i++)
            {
                Transform _tf = _alphaComp.transform.GetChild(i);
                _tf.DOPause();
                _tf.DOScale(Vector3.one * 0.5f, 0.5f).SetDelay(i * 0.05f).SetEase(Ease.InBack);
            }

            _alphaComp.DOFade(0,1).OnComplete(delegate
            {
                HideOverHandler();
            });
//			if(isFXTP)
//				_cv.ChangeCameraView (ModelConfigUtility.getInstance.JMTPTarget,-33.6f, 4f, 0.39f, 0.5f, delegate {
//					IsShowed = true;
//					return true;
//
//				});
        }


        private void HideOverHandler()
        {
            IsShowed = false;
            _alphaComp.gameObject.SetActive(false);
            if (lastCameraData != null)
                CameraViewer.Instance.ChangeCameraView(lastCameraData.target, lastCameraData.XDeg, lastCameraData.YDeg, 
                    lastCameraData.currentDistance, 0.5f, delegate {
                       FXTPMainManager.Instance.isShowDes = false;
                        return !lastCameraData.isAuto;
                    }
                    );
        }
	}
}

