using PEProtocol;

/// <summary>
/// 引导业务系统
/// </summary>

public class GuideSys
{
    private static GuideSys instance = null;
    private GuideSys() { }
    public static GuideSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GuideSys();
            }
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("GuideSys Init Done.");
    }

    public void ReqGuide(MsgPack pack)
    {
        ReqGuide data = pack.msg.reqGuide;

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspGuide,
        };

        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.session);
        AutoGuideCfg gc = CfgSvc.Instance.GetAutoGuideCfg(data.guideID);


        //更新引导任务ID
        if (pd.guideID == data.guideID)
        {
            //检测是否为智者点播任务
            if (pd.guideID == 1001)
            {
                //推送任务完成，更新任务进度
                TaskSys.Instance.CalcTaskPrgs(pd, 1);
            }
            pd.guideID += 1;
            //更新玩家数据
            pd.coin += gc.coin;
            PECommon.CalcExp(pd, gc.exp);

            if (!CacheSvc.Instance.UpdatePlayerData(pd.id,pd,pack.session))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspGuide = new RspGuide
                {
                    guideID = pd.guideID,
                    coin = pd.coin,
                    exp = pd.exp,
                    lv = pd.lv
                };
            }

        }
        else
        {
            //服务器与客户端数据不一致
            msg.err = (int)ErrorCode.ServerDataError;
        }
        pack.session.SendMsg(msg);
    }

}

