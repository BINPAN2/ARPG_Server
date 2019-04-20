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
}
