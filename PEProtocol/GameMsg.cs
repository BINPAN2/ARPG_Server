using PENet;
using System;
namespace PEProtocol
{
    [Serializable]
    public class GameMsg:PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;

        public ReqRename reqRename;
        public RspRename rspRename;

        public ReqGuide reqGuide;
        public RspGuide rspGuide;

        public ReqStrong reqStrong;
        public RspStrong rspStrong;

        public SndChat sndChat;
        public PshChat pshChat;
    }

    #region Login
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
        public int crystal;

        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int dodge;//闪避概率
        public int pierce;//穿透比率
        public int critical;//暴击概率

        public int guideID;
        public int[] strongArr;//索引号代表位置，值代表星级
    } 

    [Serializable]
    public class ReqRename
    {
        public string name;
    }

    [Serializable]
    public  class RspRename
    {
        public string name;
    }

    #endregion

    #region Guide

    [Serializable]
    public class ReqGuide
    {
        public int guideID;
    }

    [Serializable]
    public class RspGuide
    {
        public int guideID;
        public int lv;
        public int coin;
        public int exp;
    }

    #endregion

    #region Strong
    [Serializable]
    public class ReqStrong
    {
        public int pos;
    }
    [Serializable]
    public class RspStrong
    {
        public int coin;
        public int crystal;
        public int hp;
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int[] strongArr;
    }
    #endregion

    #region Chat
    [Serializable]
    public class SndChat
    {
        public string Chat;
    }
    [Serializable]
    public class PshChat
    {
        public string name;
        public string Chat;
    }
    #endregion

    public enum ErrorCode
    {
        None = 0,
        ServerDataError,//服务器与客户端数据不一致
        AcctIsOnLine,//账号已上线
        WrongPass,//密码错误
        NameIsExist,//名字已经存在
        UpdateDBError,//更新数据库错误


        LackLevel,//等级不够
        LackCoin,//缺少金币
        LackCrystal,//缺少水晶
    }

    public enum CMD
    {
        None = 0,
        //登陆相关
        ReqLogin=101,
        RspLogin=102,
        ReqRename =103,
        RspRename = 104,

        //主城相关
        ReqGuide = 201,
        RspGuide = 202,

        ReqStrong = 203,
        RspStrong = 204,

        //聊天
        SndChat = 205,
        PshChat = 206,
    }

    public class IPCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}
