using PEProtocol;

public class MissionSys
{
    private static MissionSys instance = null;
    private MissionSys() { }
    public static MissionSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MissionSys();
            }
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("MissionSys Init Done.");
    }

    public void ReqFBFight(MsgPack pack)
    {
        ReqFBFight data = pack.msg.reqFBFight;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspFBFight,
        };
        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.session);
        MapCfg mapcfg = CfgSvc.Instance.GetMapCfg(data.fbid);

        if (data.fbid>pd.mission)
        {
            msg.err = (int)ErrorCode.ClientDataError;
        }
        else if (pd.power < mapcfg.power)
        {
            msg.err = (int)ErrorCode.LackPower;
        }
        else
        {
            pd.power -= mapcfg.power;
            if (!CacheSvc.Instance.UpdatePlayerData(pd.id, pd, pack.session))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                RspFBFight rspFBFight = new RspFBFight
                {
                    fbid = data.fbid,
                    power = pd.power,
                };
                msg.rspFBFight = rspFBFight;
            }
        }
        pack.session.SendMsg(msg);
    }

    public void ReqFBFightEnd(MsgPack pack)
    {
        ReqFBFightEnd data = pack.msg.reqFBFightEnd;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspFBFightEnd,
        };
        //校验战斗结果是否合法
        if (data.iswin)
        {
            if (data.costtime>10&&data.resthp>0)
            {
                //根据副本ID获取奖励数据
                MapCfg mapcfg = CfgSvc.Instance.GetMapCfg(data.fbid);
                PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.session);

                pd.coin += mapcfg.coin;
                pd.crystal += mapcfg.crystal;
                PECommon.CalcExp(pd, mapcfg.exp);

                if (pd.mission == data.fbid)
                {
                    pd.mission++;
                }

                if (!CacheSvc.Instance.UpdatePlayerData(pd.id, pd, pack.session))
                {
                    msg.err = (int)ErrorCode.UpdateDBError;
                }
                else
                {
                    RspFBFightEnd rspFBFightEnd = new RspFBFightEnd
                    {
                        iswin = data.iswin,
                        fbid = data.fbid,
                        resthp = data.resthp,
                        costtime = data.costtime,

                        coin = pd.coin,
                        lv = pd.lv,
                        exp = pd.exp,
                        crystal = pd.crystal,
                        fuben = pd.mission,
                    };

                    msg.rspFBFightEnd = rspFBFightEnd;
                    //更新任务进度数据
                    TaskSys.Instance.CalcTaskPrgs(pd, 2);
                }
            }
            else
            {
                msg.err = (int)ErrorCode.ClientDataError;
            }
        }
        else
        {
            msg.err = (int)ErrorCode.ClientDataError;
        }

        pack.session.SendMsg(msg);
    }
}
