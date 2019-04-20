
using PEProtocol;
using System.Collections.Generic;

public class PowerSys
{
    private static PowerSys instance = null;
    private PowerSys() { }
    public static PowerSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PowerSys();
            }
            return instance;
        }
    }

    public void Init()
    {
        TimeSvc.Instance.AddTimeTask(CalcPowerAdd, PECommon.PowerAddSpace, PETimeUnit.Minute, 0);
        PECommon.Log("PowerSys Init Done.");
    }

    public void CalcPowerAdd(int tid)
    {
        //计算体力增长 TODO
        PECommon.Log("All Online Player Calc Power Increase");
        GameMsg msg = new GameMsg {
            cmd = (int)CMD.PshPower,
            pshPower = new PshPower(),
        };


        //所有在线玩家获得实时的体力增长推送数据
        Dictionary<ServerSession, PlayerData> onlineDic = CacheSvc.Instance.GetOnlineCache();

        List<ServerSession> serverlist = new List<ServerSession>(onlineDic.Keys);
        for (int i = 0; i < onlineDic.Count; i++)
        {
            PlayerData pd = onlineDic[serverlist[i]];
            ServerSession session = serverlist[i];

            int powerMax = PECommon.GetPowerLimit(pd.lv);
            if (pd.power >= powerMax)
            {
                continue;
            }
            else
            {
                pd.power += PECommon.PowerAddCount;
                pd.time = TimeSvc.Instance.GetNowTime();
                if (pd.power > powerMax)
                {
                    pd.power = powerMax;
                }
            }
            if (!CacheSvc.Instance.UpdatePlayerData(pd.id, pd, session))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.pshPower.power = pd.power;
            }
            session.SendMsg(msg);
        }

        //foreach (var item in onlineDic)
        //{
        //    PlayerData pd = item.Value;
        //    ServerSession session = item.Key;

        //    int powerMax = PECommon.GetPowerLimit(pd.lv);

        //    if (pd.power>=powerMax)
        //    {
        //        continue;
        //    }
        //    else
        //    {
        //        pd.power += PECommon.PowerAddCount;
        //        if (pd.power > powerMax)
        //        {
        //            pd.power = powerMax;
        //        }
        //    }
        //    if (!CacheSvc.Instance.UpdatePlayerData(pd.id, pd, session))
        //    {
        //        msg.err = (int)ErrorCode.UpdateDBError;
        //    }
        //    else
        //    {
        //        msg.pshPower.power = pd.power;
        //    }
        //    session.SendMsg(msg);
        //}
    }
}

