using System;
using System.Collections.Generic;
using System.Xml;
/// <summary>
/// 配置服务
/// </summary>
public class CfgSvc
{
    private static CfgSvc instance = null;
    private CfgSvc() { }
    public static CfgSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CfgSvc();
            }
            return instance;
        }
    }

    public void Init()
    {
        InitAutoGuideCfg();
        InitStrongCfg();
        InitTaskRewardCfg();
        InitMapCfg();
        PECommon.Log("CfgSvc Init Done.");
    }



    #region AutoGuideCfg

    private Dictionary<int, AutoGuideCfg> autoGuideCfgDic = new Dictionary<int, AutoGuideCfg>();

    public void InitAutoGuideCfg()
    {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"G:\Unity Projects\ARPG\Assets\Resources\ResCfgs\guide.xml");

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                AutoGuideCfg autoGuideCfg = new AutoGuideCfg
                {
                    ID = ID,

                };
                foreach (XmlElement item in ele.ChildNodes)
                {
                    switch (item.Name)
                    {
                        case "coin":
                            autoGuideCfg.coin = int.Parse(item.InnerText);
                            break;
                        case "exp":
                            autoGuideCfg.exp = int.Parse(item.InnerText);
                            break;
                    }
                }
                autoGuideCfgDic.Add(ID, autoGuideCfg);
            }
        PECommon.Log("GuideCfg Init Done");
    }

    public AutoGuideCfg GetAutoGuideCfg(int ID)
    {
        AutoGuideCfg autoGuideCfg = null;
        if (autoGuideCfgDic.TryGetValue(ID, out autoGuideCfg))
        {
            return autoGuideCfg;
        }
        return null;
    }



    #endregion

    #region Strong

    private Dictionary<int, Dictionary<int, StrongCfg>> strongCfgDic = new Dictionary<int, Dictionary<int, StrongCfg>>();

    public void InitStrongCfg()
    {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"G:\Unity Projects\ARPG\Assets\Resources\ResCfgs\strong.xml");

            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement ele = nodeList[i] as XmlElement;
                if (ele.GetAttributeNode("ID") == null)
                {
                    continue;
                }
                int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
                StrongCfg strongCfg = new StrongCfg
                {
                    ID = ID,

                };
                foreach (XmlElement item in ele.ChildNodes)
                {
                    switch (item.Name)
                    {
                        case "pos":
                            strongCfg.pos = int.Parse(item.InnerText);
                            break;
                        case "starlv":
                            strongCfg.startlv = int.Parse(item.InnerText);
                            break;
                        case "addhp":
                            strongCfg.addhp = int.Parse(item.InnerText);
                            break;
                        case "adddef":
                            strongCfg.adddef = int.Parse(item.InnerText);
                            break;
                        case "addhurt":
                            strongCfg.addhurt = int.Parse(item.InnerText);
                            break;
                        case "minlv":
                            strongCfg.minlv = int.Parse(item.InnerText);
                            break;
                        case "coin":
                            strongCfg.coin = int.Parse(item.InnerText);
                            break;
                        case "crystal":
                            strongCfg.crystal = int.Parse(item.InnerText);
                            break;
                    }
                }
                Dictionary<int, StrongCfg> dic = null;
                if (strongCfgDic.TryGetValue(strongCfg.pos, out dic))
                {
                    dic.Add(strongCfg.startlv, strongCfg);
                }
                else
                {
                    dic = new Dictionary<int, StrongCfg>();
                    dic.Add(strongCfg.startlv, strongCfg);
                    strongCfgDic.Add(strongCfg.pos, dic);
                }
            }
        PECommon.Log("StrongCfg Init Done");
    }

    public StrongCfg GetStrongCfg(int pos, int startlv)
    {
        StrongCfg sd = null;
        Dictionary<int, StrongCfg> dic = null;
        if (strongCfgDic.TryGetValue(pos, out dic))
        {
            if (dic.ContainsKey(startlv))
            {
                sd = dic[startlv];
            }
        }
        return sd;
    }

    #endregion


    #region TaskRewardCfg

    private Dictionary<int, TaskRewardCfg> taskRewardCfgDic = new Dictionary<int, TaskRewardCfg>();

    public void InitTaskRewardCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"G:\Unity Projects\ARPG\Assets\Resources\ResCfgs\taskreward.xml");

        XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            if (ele.GetAttributeNode("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            TaskRewardCfg taskRewardCfg = new TaskRewardCfg
            {
                ID = ID,
            };
            foreach (XmlElement item in ele.ChildNodes)
            {
                switch (item.Name)
                {
                    case "count":
                        taskRewardCfg.count = int.Parse(item.InnerText);
                        break;
                    case "coin":
                        taskRewardCfg.coin = int.Parse(item.InnerText);
                        break;
                    case "exp":
                        taskRewardCfg.exp = int.Parse(item.InnerText);
                        break;
                }
            }
            taskRewardCfgDic.Add(ID, taskRewardCfg);
        }
        PECommon.Log("TaskRewardCfg Init Done");
    }

    public TaskRewardCfg GetTaskRewardCfg(int ID)
    {
        TaskRewardCfg taskRewardCfg = null;
        if (taskRewardCfgDic.TryGetValue(ID, out taskRewardCfg))
        {
            return taskRewardCfg;
        }
        return null;
    }



    #endregion

    #region MapCfg

    private Dictionary<int, MapCfg> mapCfgDic = new Dictionary<int, MapCfg>();

    public void InitMapCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"G:\Unity Projects\ARPG\Assets\Resources\ResCfgs\map.xml");

        XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement ele = nodeList[i] as XmlElement;
            if (ele.GetAttributeNode("ID") == null)
            {
                continue;
            }
            int ID = Convert.ToInt32(ele.GetAttributeNode("ID").InnerText);
            MapCfg mapCfg = new MapCfg
            {
                ID = ID,
            };
            foreach (XmlElement item in ele.ChildNodes)
            {
                switch (item.Name)
                {
                    case "power":
                        mapCfg.power = int.Parse(item.InnerText);
                        break;
                }
            }
            mapCfgDic.Add(ID, mapCfg);
        }
        PECommon.Log("MapCfg Init Done");
    }

    public MapCfg GetMapCfg(int ID)
    {
        MapCfg mapCfg = null;
        if (mapCfgDic.TryGetValue(ID, out mapCfg))
        {
            return mapCfg;
        }
        return null;
    }



    #endregion

}
public class BaseData<T>
{
    public int ID;
}


public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
    public int coin;
    public int exp;
}

public class StrongCfg : BaseData<StrongCfg>
{
    public int pos;
    public int startlv;
    public int addhp;
    public int addhurt;
    public int adddef;
    public int minlv;
    public int coin;
    public int crystal;
}

public class TaskRewardCfg : BaseData<TaskRewardCfg>
{
    public int count;
    public int exp;
    public int coin;
}

public class TaskRewardData : BaseData<TaskRewardData>
{
    public int prgs;
    public bool taked;
}

public class MapCfg: BaseData<MapCfg>
{
    public int power;
}
