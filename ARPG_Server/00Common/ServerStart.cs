/// <summary>
/// 启动服务器
/// </summary>

public class ServerStart
{
    static void  Main(string[] args)
    {
        ServerRoot.Instance.Init();

        while (true)
        {
            ServerRoot.Instance.Update();
        }
    }
}
