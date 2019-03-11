using PEProtocol;
/// <summary>
/// 强化系统
/// </summary>
public class StrongSys
{
    private static StrongSys instance = null;
    private StrongSys() { }
    public static StrongSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new StrongSys();
            }
            return instance;
        }
    }
    public void Init()
    {
        PECommon.Log("StrongSys Init Done.");
    }

    public void ReqStrong(MsgPack pack)
    {
        ReqStrong data = pack.msg.reqStrong;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspStrong,
        };

        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.session);
        int curStarlv = pd.strongArr[data.pos];
        StrongCfg nextsd = CfgSvc.Instance.GetStrongCfg(data.pos, curStarlv + 1);
        //判断资源是否足够
        if (pd.lv<nextsd.minlv)
        {
            msg.err = (int)ErrorCode.LackLevel;
        }
        else if (pd.coin<nextsd.coin)
        {
            msg.err = (int)ErrorCode.LackCoin;
        }
        else if (pd.crystal<nextsd.crystal)
        {
            msg.err = (int)ErrorCode.LackCrystal;
        }
        else
        {
            //资源扣除
            pd.coin -= nextsd.coin;
            pd.crystal -= nextsd.crystal;
            //增加属性
            pd.strongArr[data.pos] += 1;
            pd.hp += nextsd.addhp;
            pd.ad += nextsd.addhurt;
            pd.ap += nextsd.addhurt;
            pd.addef += nextsd.adddef;
            pd.apdef += nextsd.adddef;

            if (!CacheSvc.Instance.UpdatePlayerData(pd.id,pd,pack.session))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspStrong = new RspStrong
                {
                    coin = pd.coin,
                    crystal = pd.crystal,
                    hp = pd.hp,
                    ad = pd.ad,
                    ap = pd.ap,
                    addef = pd.addef,
                    apdef = pd.apdef,
                    strongArr = pd.strongArr
                };
            }

        }


        pack.session.SendMsg(msg);
    }
}

