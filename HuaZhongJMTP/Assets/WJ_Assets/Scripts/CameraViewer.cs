using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// 摄像机控制脚本
/// </summary>
public class CameraViewer : MonoBehaviour
{
	public static CameraViewer Instance;

	public Transform target;                // 注视的目标点
	public Vector3 targetOffset;            // 目标点的偏移量

	public float distance = 5.0f;           // 距离值 5.0
	public float maxDistance = 5;          // 最大距离 20
	public float minDistance = 0.3f;        // 最小距离 0.6

	public float xSpeed = 200.0f;           // 旋转速度 200
	public float ySpeed = 200.0f;           // 旋转速度 200
                                            //限制范围  
    public int yMinAngleLimit = -80;          //-80
	public int yMaxAngleLimit = 80;         //80
	public int xMinAngleLimit = 0;        //-80
	public int xMaxAngleLimit = 80;         //80
	public int zoomRate = 40;               // 缩放比率40
	public float zoomDampening = 5.0f;      // 缩放阻尼5


	public float xDeg = 0.0f;

	public float yDeg = 0.0f;
    /// <summary>
    /// 当前的距离
    /// </summary>
	public float currentDistance;
    /// <summary>
    /// 目标距离
    /// </summary>
	[HideInInspector]
	public float desiredDistance;
	public Quaternion rotation;
	private Vector3 position;
    /// <summary>
    /// 摄像机锁
    /// </summary>
	public static  bool ModelCameraLocked = false;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;

	private bool isRePosition;

	//摄像机是否自动变化视口
	private bool isAutoChangeView;
    /// <summary>
    /// 设置摄像机的模式
    /// </summary>
    public bool IsAutoChangeView
    {
        set { isAutoChangeView = value; }
        get { return isAutoChangeView; }
    }
	void Awake() { Init(); }

	[HideInInspector]
	public Vector3 startPos,startAngle;

	IEnumerator Start()
	{
		yield return new WaitForSeconds (0.1f);
		startPos = transform.position;
		startAngle = transform.eulerAngles;
	}

	void Init()
	{
        //如果为空 获取自身
		if(Instance == null)Instance = GetComponent<CameraViewer>();
        //防止目标点丢失

        if (!target)
        {
            GameObject go = new GameObject("Cam Target");
            go.transform.position = transform.position + (transform.forward * distance);
            target = go.transform;
        }

        distance = Vector3.Distance(transform.position, target.position);
		currentDistance = distance;
		desiredDistance = distance;

		position = transform.position;
		rotation = transform.rotation;

		xDeg = Vector3.Angle (Vector3.right, transform.right);
		yDeg = Vector3.Angle (Vector3.up, transform.up);

        isAutoChangeView = true;    
	}
		

	void LateUpdate()
	{
		//MouseDragUtility.getInstance.currDragTrans != null || 
		//如果有拖拽物体，则锁定视角
//		if (IToolsLM.getInstance.isCollisionForCameraViewer)
//			return;

		// 鼠标滚轮 ———— 缩放远近
		if(!isAutoChangeView)
			desiredDistance -= Input.GetAxis ("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs (desiredDistance);

		// 鼠标左键 ———— 旋转
		if (Input.GetMouseButton (0) && !isAutoChangeView) 
		{
			xDeg += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
			yDeg -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
			yDeg = ClampAngle (yDeg, yMinAngleLimit, yMaxAngleLimit);
			xDeg = ClampAngle (xDeg, xMinAngleLimit, xMaxAngleLimit);
		}
			
            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
            currentRotation = transform.rotation;
			rotation = Quaternion.Lerp (currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
			transform.rotation = rotation;
		
		desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
		currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);
		position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
		transform.position = position;
	}

	// 角度限定
	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360)  angle += 360;
		if (angle > 360) angle -= 360;
		return Mathf.Clamp(angle, min, max);
	}
	#region 摄像机动画方法
	//摄像机动画方法(插值自己在运行状态下鼠标移动到需要的位置，在脚本Editor模式下查看xDeg,yDeg和CurrentDistance的数值,最后个参数(true)为可继续操作摄像机视角)
	public void ChangeCameraView(float _xDeg,float _yDeg,float _Distance,float _changeTime,System.Func<bool> _viewBack = null)
	{
		isAutoChangeView = true;
		DOTween.To(() => xDeg, x => xDeg = x, _xDeg, _changeTime);
		DOTween.To(() => yDeg, x => yDeg = x, _yDeg, _changeTime);
		DOTween.To(() => desiredDistance, x => desiredDistance = x, _Distance, _changeTime).OnComplete(delegate{
			if(_viewBack != null)
			{
				isAutoChangeView = !_viewBack();
			}
			
		});
	}

    public void ChangeCameraView(Vector3 _target, float _xDeg, float _yDeg, float _Distance, float _changeTime, System.Func<bool> _viewBack = null)
    {
        target.DOMove(_target, _changeTime);
        isAutoChangeView = true;
        DOTween.To(() => xDeg, x => xDeg = x, _xDeg, _changeTime);
        DOTween.To(() => yDeg, x => yDeg = x, _yDeg, _changeTime);
        DOTween.To(() => desiredDistance, x => desiredDistance = x, _Distance, _changeTime).OnComplete(delegate {
            //target = _target;
            if (_viewBack != null)
                isAutoChangeView = !_viewBack();

        });
    }
    public void ChangeCameraView(Transform _target,float _xDeg,float _yDeg,float _Distance,float _changeTime,System.Func<bool> _viewBack = null)
	{
		target.DOMove (_target.position, _changeTime);
		isAutoChangeView = true;
		DOTween.To(() => xDeg, x => xDeg = x, _xDeg, _changeTime);
		DOTween.To(() => yDeg, x => yDeg = x, _yDeg, _changeTime);
		DOTween.To(() => desiredDistance, x => desiredDistance = x, _Distance, _changeTime).OnComplete(delegate{
			//target = _target;
			if(_viewBack != null)
				isAutoChangeView = !_viewBack();

		});
	}
    public void ChangeCameraView(Transform _target, float _xDeg, float _yDeg, float _Distance, float _changeTime,System.Action _CallBack, System.Func<bool> _viewBack = null)
    {
        target.DOMove(_target.position, _changeTime);
        isAutoChangeView = true;
        DOTween.To(() => xDeg, x => xDeg = x, _xDeg, _changeTime);
        DOTween.To(() => yDeg, x => yDeg = x, _yDeg, _changeTime);
        DOTween.To(() => desiredDistance, x => desiredDistance = x, _Distance, _changeTime).OnComplete(delegate {
            //target = _target;
            if (_viewBack != null)
                isAutoChangeView = !_viewBack();

            if (_CallBack != null)
            {
                _CallBack();
            }
        });
    }
    public void ChangeCameraView(Transform _target, float _xDeg, float _yDeg, float _Distance, float _changeTime, bool isAutoChangeView)
    {
        target.DOMove(_target.position, _changeTime);
        this.isAutoChangeView = true;
        DOTween.To(() => xDeg, x => xDeg = x, _xDeg, _changeTime);
        DOTween.To(() => yDeg, x => yDeg = x, _yDeg, _changeTime);
        DOTween.To(() => desiredDistance, x => desiredDistance = x, _Distance, _changeTime).OnComplete(delegate {
            //target = _target;
            this.isAutoChangeView = isAutoChangeView;
           
        });
    }
    public void ChangeCameraView( float _xDeg, float _yDeg, float _Distance, float _changeTime, bool isAutoChangeView)
    {
       // target.DOMove(_target.position, _changeTime);
        this.isAutoChangeView = true;
        DOTween.To(() => xDeg, x => xDeg = x, _xDeg, _changeTime);
        DOTween.To(() => yDeg, x => yDeg = x, _yDeg, _changeTime);
        DOTween.To(() => desiredDistance, x => desiredDistance = x, _Distance, _changeTime).OnComplete(delegate {
            //target = _target;
            this.isAutoChangeView = isAutoChangeView;

        });
    }
    #endregion
}
