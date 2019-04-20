
using PEProtocol;

public class TaskSys
{
    private static TaskSys instance = null;
    private TaskSys() { }
    public static TaskSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TaskSys();
            }
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("TaskSys Init Done.");
    }

    public void ReqTakeTaskReward(MsgPack pack)
    {
        ReqTakeTaskReward data = pack.msg.reqTakeTaskReward;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspTakeTaskReward,
        };
        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.session);
        TaskRewardCfg trcfg = CfgSvc.Instance.GetTaskRewardCfg(data.rid);
        TaskRewardData trd = GetTaskRewardData(pd, data.rid);

        if (trd.prgs==trcfg.count&&!trd.taked)
        {
            pd.coin += trcfg.coin;
            PECommon.CalcExp(pd, trcfg.exp);
            trd.taked = true;
            CalcTaskArr(pd, trd);

            if (!CacheSvc.Instance.UpdatePlayerData(pd.id, pd, pack.session))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspTakeTaskReward = new RspTakeTaskReward
                {
                    coin = pd.coin,
                    exp = pd.exp,
                    lv = pd.lv,
                    taskArr = pd.taskArr,
                };
            }
        }
        else
        {
            msg.err = (int)ErrorCode.ClientDataError;
        }
        pack.session.SendMsg(msg);
    }

    public TaskRewardData GetTaskRewardData(PlayerData pd,int rid)
    {
        TaskRewardData trd = null;
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            if (int.Parse(taskInfo[0])==rid)
            {
                trd = new TaskRewardData
                {
                    ID = rid,
                    prgs = int.Parse(taskInfo[1]),
                    taked = taskInfo[2].Equals("1"),
                };
                break;
            }
        }
        return trd;
    }

    public void CalcTaskArr(PlayerData pd, TaskRewardData trd)
    {
        string result = trd.ID + "|" + trd.prgs + "|" + (trd.taked ? 1 : 0);
        int index = -1;
        for (int i = 0; i < pd.taskArr.Length; i++)
        {
            string[] taskInfo = pd.taskArr[i].Split('|');
            if (int.Parse(taskInfo[0])==trd.ID)
            {
                index = i;
                break;
            }
        }
        pd.taskArr[index] = result;
    }

    public void CalcTaskPrgs(PlayerData pd,int rid)
    {
        TaskRewardData trd = TaskSys.instance.GetTaskRewardData(pd, rid);
        TaskRewardCfg trcfg = CfgSvc.Instance.GetTaskRewardCfg(rid);

        if (trd.prgs<trcfg.count)
        {
            trd.prgs += 1;
            CalcTaskArr(pd, trd);

            ServerSession session = CacheSvc.Instance.GetServerSessionByID(pd.id);
            if (session != null)
            {
                GameMsg msg = new GameMsg
                {
                    cmd = (int)CMD.PshTaskPrgs,
                    pshTaskPrgs = new PshTaskPrgs
                    {
                        taskArr = pd.taskArr
                    }
                };

                session.SendMsg(msg);
            }
        }
    }
}

