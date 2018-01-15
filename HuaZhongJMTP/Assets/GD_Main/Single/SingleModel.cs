using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 项目中会用到的一些模型
/// </summary>
public static class SingleModel  {
    /// <summary>
    /// 默认的字体颜色
    /// </summary>
    public static Color defaltColor = new Color32(50, 50, 50, 255);
    private static Transform model_FXTP;
    /// <summary>
    /// 分析天平
    /// </summary>
    public static Transform Model_FXTP
    {
        get
        {
            if (model_FXTP == null)
            {
                model_FXTP = GameObject.Find("+ Models/FXTP/Balance_FX/").transform;
            }
            return model_FXTP;
        }
    }

    private static Transform model_Door;
    /// <summary>
    /// 天平的门
    /// </summary>
    public static Transform Model_Door
    {
        get { if (model_Door == null)
            {
                model_Door= GameObject.Find("+ Models/FXTP/Balance_FX/Balance_FX_door_right").transform;
            }
            return model_Door;
        }
    }

    private static Transform model_DoorTop;
    /// <summary>
    /// 天平的门 上
    /// </summary>
    public static Transform Model_DoorTop
    {
        get
        {
            if (model_DoorTop == null)
            {
                model_DoorTop = GameObject.Find("+ Models/FXTP/Balance_FX/Balance_FX_door_top").transform;
            }
            return model_DoorTop;
        }
    }
    private static Image ui_LoadingCal;
    /// <summary>
    /// 加载图片
    /// </summary>
    public static Image UI_LoadingCal
    {
        get
        {
            if (ui_LoadingCal == null)
            {
                ui_LoadingCal = GameObject.Find("TipCanvas/Button/_Cal").GetComponent<Image>();
            }
            return ui_LoadingCal;
        }
    }

    private static Image ui_LoadingPower;
    /// <summary>
    /// 电源显示图片
    /// </summary>
    public static Image UI_LoadingPower
    {
        get
        {
            if (ui_LoadingPower == null)
            {
                ui_LoadingPower = GameObject.Find("TipCanvas/Button/_Power").GetComponent<Image>();
            }
            return ui_LoadingPower;
        }
    }

    private static Text ui_TextInfo;
    /// <summary>
    /// 显示屏显示的信息
    /// </summary>
    public static Text UI_TextInfo
    {
        get
        {
            if (ui_TextInfo == null)
            {
                ui_TextInfo = GameObject.Find("TipCanvas/viewTip").GetComponent<Text>();
            }
            return ui_TextInfo;
        }
    }
    
    private static Text ui_TextDanWeiInfo;
    /// <summary>
    /// 显示屏显示的单位信息
    /// </summary>
    public static Text UI_TextDanWeiInfo
    {
        get
        {
            if (ui_TextDanWeiInfo == null)
            {
                ui_TextDanWeiInfo = GameObject.Find("TipCanvas/danWei").GetComponent<Text>();
            }
            return ui_TextDanWeiInfo;
        }
    }
    private static Transform ui_showInfoPoint;
    /// <summary>
    /// 显示提示信息的坐标点
    /// </summary>
    public static Transform UI_ShowInfoPoint
    {
        get
        {
            if (ui_showInfoPoint == null)
            {
                ui_showInfoPoint = GameObject.Find("+ Models/FXTP/InfoPoint/_TipPoint").transform;
            }
            return ui_showInfoPoint;
        }
    }

    private static GameObject model_GanZaoBao;
    /// <summary>
    /// 干燥包
    /// </summary>
    public static GameObject Model_GanZaoBao
    {
        get
        {
            if (model_GanZaoBao == null)
            {
                model_GanZaoBao = GameObject.Find("+ Models/ganzaobap");
            }
            return model_GanZaoBao;
        }
    }

    private static Transform model_DianXian;
    /// <summary>
    /// 电线
    /// </summary>
    public static Transform Model_DianXian
    {
        get
        {
            if (model_DianXian == null)
            {
                model_DianXian = GameObject.Find("+ Models/DianXian").transform;
            }
            return model_DianXian;
        }
    }
    /// <summary>
    /// 克
    /// </summary>
    public const string danWei = "g";
    /// <summary>
    /// 0.000 
    /// </summary>
    public const string zero = "0.000";
    /// <summary>
    /// 当关闭的时候设置为""空的
    /// </summary>
    public const string Close = "";
    /// <summary>
    /// 拖拽时限制的最大移动距离
    /// </summary>
    public const float maxDistance = 0.03f;
    /// <summary>
    /// 动画的播放速度控制
    /// </summary>
    public const float playSpeed = 1f;
    /// <summary>
    /// 相撞后离开的时间
    /// </summary>
    public const float escapeTime = 0.15f;
}
