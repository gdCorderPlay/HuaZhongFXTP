using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using WJ;
//using UnityEditor;
using System.Xml.Linq;
/// <summary>
/// xml 管理
/// </summary>
public class XMLManager:MonoBehaviour  {

    public static XMLManager Instance;
    private void Awake()
    {
        Instance = this;
        ReadXML();
        ParseStringConfig();
        ParseClickStringConfig();
    }

    
    public  List<StepData> tempStepData =new List<StepData>();
    public Dictionary<string, string> strConfigDir=new Dictionary<string, string>();
    public Dictionary<string, string> clickConfigDir=new Dictionary<string, string>();
  /// <summary>
  /// 解析xml 生成一个步骤的列表
  /// </summary>
    void ReadXML()
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.dataPath + "/FXTPStep.xml");
        XmlNodeList list = xmlDoc.SelectSingleNode("StepDatas").ChildNodes;
        foreach (XmlNode xmlNode in list)
        {
            //创建一个列表
            List<StepItemData> items = new List<StepItemData>();
            XmlNodeList nodeList = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodeList)
            {
                XmlElement xmld = (XmlElement)xn;
                items.Add(new StepItemData(xmld.GetAttribute("StepDesc")));
            }
            XmlElement xmled = (XmlElement)xmlNode;
            tempStepData.Add(new StepData(xmled.GetAttribute("OperationDesc"), items));
        }
    }

    void ParseStringConfig()
    {
        XmlDocument xmlDoc = new XmlDocument();
         XmlReaderSettings settings = new XmlReaderSettings();
         settings.IgnoreComments = true;//忽略文档里面的注释
         XmlReader reader = XmlReader.Create(Application.dataPath + "/FXTPStringConfig.xml", settings);
         xmlDoc.Load(reader);
        
        XmlNodeList list = xmlDoc.SelectSingleNode("StringConfig").ChildNodes;
        foreach (XmlNode xmlNode in list)
        {
            XmlElement temp =(XmlElement) xmlNode  ;
            strConfigDir.Add(xmlNode.Name, temp.GetAttribute("value"));
          
        }
        reader.Close();
    }
    void ParseClickStringConfig()
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreComments = true;//忽略文档里面的注释
        XmlReader reader = XmlReader.Create(Application.dataPath + "/FXTPStringClick.xml", settings);
        xmlDoc.Load(reader);
        XmlElement temp;
        XmlNodeList list = xmlDoc.SelectSingleNode("ClickInfo").ChildNodes;
        foreach (XmlNode xmlNode in list)
        {
            temp = (XmlElement)xmlNode;
            clickConfigDir.Add(xmlNode.Name, temp.GetAttribute("value"));
        }
        reader.Close();
    }
    /// <summary>
    /// 步骤 xml 表
    /// </summary>
    
    void CreatXML1()
    {


       
        XmlDocument xmlDoc = new XmlDocument();

        XmlElement root = xmlDoc.CreateElement("StepDatas");
        root.SetAttribute("ExperimentName", "分析天平");



        XmlElement Operation1 = xmlDoc.CreateElement("Operation");
        Operation1.SetAttribute("OperationIndex", "0");
        Operation1.SetAttribute("OperationDesc", "准备工作");

        XmlElement Step0 = xmlDoc.CreateElement("Step");
        Step0.SetAttribute("StepIndex", "0");
        Step0.SetAttribute("StepDesc", "将交流电源适配器的插头插入台面上的插座孔处，接通电源。");


        XmlElement Step1 = xmlDoc.CreateElement("Step");
        Step1.SetAttribute("StepIndex", "1");
        Step1.SetAttribute("StepDesc", "旋转水平调节螺丝，将气泡调至中央，以弥补台面不平对称量结果造成的影响。");

        
        XmlElement Step2 = xmlDoc.CreateElement("Step");
        Step2.SetAttribute("StepIndex", "2");
        Step2.SetAttribute("StepDesc", "空载情况下单击《开/关》键，启动天平。");

        XmlElement Step3 = xmlDoc.CreateElement("Step");
        Step3.SetAttribute("StepIndex", "3");
        Step3.SetAttribute("StepDesc", "取出干燥包,按天平《去皮》键，使天平显示0.000g。");

        XmlElement Step4 = xmlDoc.CreateElement("Step");
        Step4.SetAttribute("StepIndex", "4");
        Step4.SetAttribute("StepDesc", "按住《Cal》键不放，直到显示屏出现“CAL”字样后松开。");

        XmlElement Step5 = xmlDoc.CreateElement("Step");
        Step5.SetAttribute("StepIndex", "5");
        Step5.SetAttribute("StepDesc", "拖拽符合外部校准重量的砝码到分析天平处，对天平进行校正。");

       



        XmlElement Operation2 = xmlDoc.CreateElement("Operation");
        Operation2.SetAttribute("OperationIndex", "1");
        Operation2.SetAttribute("OperationDesc", "正式测量");


        XmlElement Step6 = xmlDoc.CreateElement("Step");
        Step6.SetAttribute("StepIndex", "6");
        Step6.SetAttribute("StepDesc", "拖拽烧杯到分析天平处，将称量纸放在天平的秤盘上。");


        XmlElement Step7 = xmlDoc.CreateElement("Step");
        Step7.SetAttribute("StepIndex", "7");
        Step7.SetAttribute("StepDesc", "拖拽药勺到琼脂糖干粉处，挖取一定重量并作为称量样品。");

        xmlDoc.AppendChild(root);
        root.AppendChild(Operation1);
        root.AppendChild(Operation2);
        Operation1.AppendChild(Step0);
        Operation1.AppendChild(Step1);
        Operation1.AppendChild(Step2);
        Operation1.AppendChild(Step3);
        Operation1.AppendChild(Step4);
        Operation1.AppendChild(Step5);
        Operation2.AppendChild(Step6);
        Operation2.AppendChild(Step7);
        xmlDoc.Save(Application.dataPath + "/FXTPStep.xml");
      //  AssetDatabase.Refresh();
    }
    /// <summary>
    /// 提示信息表
    /// </summary>
    void CreatXml2()
    {
        XmlDocument xmlDoc = new XmlDocument();

        XmlElement strInfo = xmlDoc.CreateElement("StringConfig");


        XmlElement info1 = xmlDoc.CreateElement("PowerInfoLog");
        info1.SetAttribute("value", "已接通电源");
        XmlElement info2 = xmlDoc.CreateElement("NoPowerInfoLog");
        info2.SetAttribute("value", "实验开始，准备接通电源");

        XmlElement info3 = xmlDoc.CreateElement("BalanceInitInfoLog");
        info3.SetAttribute("value", "点击三次，调平");


        XmlElement info4 = xmlDoc.CreateElement("OnInitInfoLog");
        info4.SetAttribute("value", "点击电源开关，启动天平");

        XmlElement info5= xmlDoc.CreateElement("RemoveGanZaoBaoInfoLog");
        info5.SetAttribute("value", "需要移出干燥包！");

        XmlElement info6 = xmlDoc.CreateElement("RemoveInitInfoLog");
        info6.SetAttribute("value", "等待去皮。");


        XmlElement info7 = xmlDoc.CreateElement("CheckInitInfoLog");
        info7.SetAttribute("value", "点击校正按钮，开始校正");
       

        XmlElement info8 = xmlDoc.CreateElement("LayUpWeightInitInfoLog ");
        info8.SetAttribute("value", "外部砝码校准重量。");

        XmlElement info9 = xmlDoc.CreateElement("LayUpWeightCompleteInfoLog");
        info9.SetAttribute("value", "天平校准过程结束，移去砝码。");

        XmlElement info10 = xmlDoc.CreateElement("PanPaperInitInfoLog");
        info10.SetAttribute("value", "拖拽烧杯到天平处，准备称量。");

        XmlElement info11 = xmlDoc.CreateElement("WeighUpSolidInitInfoLog");
        info11.SetAttribute("value", "拖拽药勺到药盒处，准备称量。");

        XmlElement info12 = xmlDoc.CreateElement("PanPaperInitInfoLog");
        info12.SetAttribute("value", "拖拽烧杯到天平处，准备称量。");

    }

}
