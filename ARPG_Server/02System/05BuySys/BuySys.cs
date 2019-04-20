using PEProtocol;
public class BuySys
{
    private static BuySys instance = null;
    private BuySys() { }
    public static BuySys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new BuySys();
            }
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("BuySys Init Done.");
    }

    public void ReqBuy(MsgPack pack)
    {
        ReqBuy data = pack.msg.reqBuy;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspBuy,
        };

        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.session);

        if (pd.diamond<data.cost)
        {
            msg.err = (int)ErrorCode.LackDiamond;
        }
        else
        {
            pd.diamond -= data.cost;
            switch (data.buyType)
            {
                case 0://体力
                    pd.power += 100;
                    TaskSys.Instance.CalcTaskPrgs(pd, 4);//更新任务进度数据
                    break;
                case 1://金币
                    pd.coin += 1000;
                    TaskSys.Instance.CalcTaskPrgs(pd, 5);//更新任务进度数据
                    break;
            }

            if (!CacheSvc.Instance.UpdatePlayerData(pd.id, pd, pack.session))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }

            else
            {
                msg.rspBuy = new RspBuy
                {
                    diamond = pd.diamond,
                    coin = pd.coin,
                    power = pd.power,
                    buyType = data.buyType,
                };
            }
        }

        pack.session.SendMsg(msg);

    }
}

