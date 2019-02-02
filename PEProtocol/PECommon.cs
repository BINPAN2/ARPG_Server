/// <summary>
/// 客户端服务器端共用工具类
/// </summary>

using PENet;
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
}

