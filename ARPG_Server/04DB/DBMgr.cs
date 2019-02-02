using MySql.Data.MySqlClient;
using PEProtocol;
/// <summary>
/// 数据库管理类
/// </summary>
public class DBMgr
{
    private static DBMgr instance = null;
    private DBMgr() { }
    public static DBMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DBMgr();
            }
            return instance;
        }
    }

    private MySqlConnection conn = null;

    public void Init()
    {
        conn = new MySqlConnection("server = localhost; User Id =root;password = root;database = arpg;Charset = utf8");
        PECommon.Log("NetSvc Init Done.");
    }

    public PlayerData QueryPlayerData(string acct,string pass)
    {
        PlayerData playerData = null;
        //TODO
        return playerData;
    }
}

