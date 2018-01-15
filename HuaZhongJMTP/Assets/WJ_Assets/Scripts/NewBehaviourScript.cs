using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WJ;
using DG.Tweening;

public class NewBehaviourScript : MonoBehaviour
{
    private IEnumerator Start()
    {
        GlobalUIManager.Instance.InitUIData(GlobalUIManager.Instance.tempStepData);
        yield return new WaitForSeconds(1);
		//transform.DOLookAt (Camera.main.transform.position, 0.5f, AxisConstraint.None, Vector3.up);
    }

    


    void Update()
	{
		if(Input.GetKeyDown(KeyCode.A))
		{
			InfoPanel.Instance.ShowInfo(GameObject.Find("+ Models/FXTP/InfoPoint").transform,"浮屠玄冷喝的声音，爆响而起，那之中蕴含的怒意，令得天地都是寂静下来，圣品之威，显露无疑。",delegate{
				print("信息面板已关闭");
			});
		}


		if(Input.GetKeyDown(KeyCode.S))
		{
			MyLogger.Instance.Log("浮屠玄冷喝的声音，爆响而起，那之中蕴含的怒意，令得天地都是寂静下来，圣品之威，显露无疑。",350,MyLogger.TextAlign.Center,delegate{
				print("Log窗口已关闭");
			});
		}
	}
}
