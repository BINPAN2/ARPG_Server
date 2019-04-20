/// <summary>
/// 服务器初始化
/// </summary>
public class ServerRoot
{
    private static ServerRoot instance = null;
    private ServerRoot() { }
    public static ServerRoot Instance
    {
        get
        {
            if (instance==null)
            {
                instance = new ServerRoot();
            }
            return instance;
        }
    }

    public void Init()
    {
        //数据层 
        DBMgr.Instance.Init();
        //服务层
        CfgSvc.Instance.Init();
        Netsvc.Instance.Init();
        CacheSvc.Instance.Init();
        TimeSvc.Instance.Init();
        //业务系统层
        LoginSys.Instance.Init();
        GuideSys.Instance.Init();
        ChatSys.Instance.Init();
        StrongSys.Instance.Init();
        BuySys.Instance.Init();
        PowerSys.Instance.Init();
        TaskSys.Instance.Init();

    }

    public void Update()
    {
        Netsvc.Instance.Update();
        TimeSvc.Instance.Update();
    }

    private int SessionID = 0;
    public int GetSessionID()
    {
        if (SessionID == int.MaxValue)
        {
            SessionID = 0;
        }
        return SessionID+=1;
    }
}

