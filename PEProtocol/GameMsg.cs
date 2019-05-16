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

        public ReqBuy reqBuy;
        public RspBuy rspBuy;

        public PshPower pshPower;

        public ReqTakeTaskReward reqTakeTaskReward;
        public RspTakeTaskReward rspTakeTaskReward;

        public PshTaskPrgs pshTaskPrgs;

        public ReqFBFight reqFBFight;
        public RspFBFight rspFBFight;

        public ReqFBFightEnd reqFBFightEnd;
        public RspFBFightEnd rspFBFightEnd;
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
        public long time;
        public string[] taskArr;
        public int mission;
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

    #region Power
    [Serializable]
    public class PshPower
    {
        public int power;
    }

    #endregion

    #region Buy
    [Serializable]
    public class ReqBuy
    {
        public int buyType;
        public int cost;
    }

    [Serializable]
    public class RspBuy
    {
        public int buyType;
        public int diamond;
        public int coin;
        public int power;
    }
    #endregion

    #region TaskReward
    [Serializable]
    public class ReqTakeTaskReward
    {
        public int rid;
    }
    [Serializable]
    public class RspTakeTaskReward
    {
        public int coin;
        public int lv;
        public int exp;
        public string[] taskArr;
    }
    [Serializable]
    public class PshTaskPrgs
    {
        public string[] taskArr;
    }

    #endregion

    #region Fight
    [Serializable]
    public class ReqFBFight
    {
        public int fbid;
    }

    [Serializable]
    public class RspFBFight
    {
        public int fbid;
        public int power;
    }

    [Serializable]
    public class ReqFBFightEnd
    {
        public bool iswin;
        public int fbid;
        public int resthp;
        public int costtime;
    }

    [Serializable]
    public class RspFBFightEnd
    {
        public bool iswin;
        public int fbid;
        public int resthp;
        public int costtime;

        //副本奖励
        public int coin;
        public int lv;
        public int exp;
        public int crystal;
        public int fuben;
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
        ClientDataError,//客户端数据异常

        LackLevel,//等级不够
        LackCoin,//缺少金币
        LackCrystal,//缺少水晶
        LackDiamond,//缺少钻石
        LackPower,//体力不足
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

        //购买
        ReqBuy = 207,
        RspBuy = 208,

        //推送体力
        PshPower = 209,

        //领取任务奖励
        ReqTakeTaskReward = 210,
        RspTakeTaskReward = 211,

        //推送任务进度
        PshTaskPrgs = 212,

        //战斗
        ReqFBFight =301,
        RspFBFight = 302,

        ReqFBFightEnd = 303,
        RspFBFightEnd = 304,
    }

    public class IPCfg
    {
        public const string srvIP = "127.0.0.1";
        public const int srvPort = 17666;
    }
}
