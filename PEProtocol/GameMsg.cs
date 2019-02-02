using PENet;
using System;
namespace PEProtocol
{
    [Serializable]
    public class GameMsg:PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;
    }

    [Serializable]
    public class ReqLogin
    {
        public string acct;
        public string pass;
    }

    [Serializable]
    public class RspLogin
    {
        public PlayerData playerData;
    }

    [Serializable]
    public class PlayerData
    {
        public int id;
        public string name;
        public int lv;
        public int exp;
        public int power;
        public int coin;
        public int diamond;
    }

    public enum ErrorCode
    {
        None = 0,
        AcctIsOnLine,//账号已上线
        WrongPass,//密码错误
    }

    public enum CMD
    {
        None = 0,
        //登陆相关
        ReqLogin=101,
        RspLogin=102,
    }

    public class IPCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}
