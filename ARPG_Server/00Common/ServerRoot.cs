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

        //服务层
        Netsvc.Instance.Init();
        CacheSvc.Instance.Init();

        //业务系统曾
        LoginSys.Instance.Init();
    }

    public void Update()
    {
        Netsvc.Instance.Update();
    }
}

