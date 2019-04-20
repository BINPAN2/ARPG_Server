/// <summary>
/// 客户端服务器端共用工具类
/// </summary>

using PENet;
using PEProtocol;
public enum LogType
{
    Log = 0,
    Warn = 1,
    Error = 2,
    Info = 3,
}
public class PECommon
{
    public static void Log(string msg = "", LogType tp = LogType.Log)
    {
        LogLevel lv = (LogLevel)tp;
        PETool.LogMsg(msg, lv);
    }


    public static int GetFightByProps(PlayerData pd)
    {
        return pd.lv * 100 + pd.ad + pd.ap + pd.addef + pd.apdef;
    }

    public static int GetPowerLimit(int lv)
    {
        return ((lv - 1) / 10) * 150 + 150;
    }

    public static int GetExpUpValByLv(int lv)
    {
        return 100 * lv * lv;
    }

    public const int PowerAddSpace = 5;//体力增加间隔 分钟
    public const int PowerAddCount = 2;

    public static void CalcExp(PlayerData pd, int addExp)
    {
        int curLv = pd.lv;
        int curExp = pd.exp;
        int addRestExp = addExp;
        while (true)
        {
            int upNeedExp = PECommon.GetExpUpValByLv(curLv) - curExp;
            if (addRestExp >= upNeedExp)
            {
                curLv++;
                curExp = 0;
                addRestExp -= upNeedExp;
            }
            else
            {
                pd.lv = curLv;
                pd.exp = curExp + addRestExp;
                break;
            }
        }
    }


}

