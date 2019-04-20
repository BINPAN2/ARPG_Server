/// <summary>
/// 网络服务
/// </summary>

using PEProtocol;
using PENet;
using System.Collections.Generic;

public class MsgPack
{
    public ServerSession session;
    public GameMsg msg;
    public MsgPack(ServerSession session,GameMsg msg)
    {
        this.session = session;
        this.msg = msg;
    }
}

public class Netsvc
{
    private static Netsvc instance = null;
    private Netsvc() { }
    public static Netsvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Netsvc();
            }
            return instance;
        }
    }
    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();
    public static readonly string obj = "lock";

    public void Init()
    {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();
        server.StartAsServer(IPCfg.srvIP, IPCfg.srvPort);
        PECommon.Log("NetSvc Init Done.");
    }

    public void AddMsgQue(MsgPack pack)
    {
        lock (obj)
        {
            msgPackQue.Enqueue(pack);
        }

    }

    public void Update()
    {
        if (msgPackQue.Count > 0)
        {
            lock (obj)
            {
                PECommon.Log("PackCount:" + msgPackQue.Count);
                MsgPack pack = msgPackQue.Dequeue();
                HandOutMsg(pack);
            }

        }
    }

    public void HandOutMsg(MsgPack pack)
    {
        switch ((CMD)pack.msg.cmd)
        {
            case CMD.ReqLogin:
                LoginSys.Instance.ReqLogin(pack);
                break;

            case CMD.ReqRename:
                LoginSys.Instance.ReqRename(pack);
                break;
            case CMD.ReqGuide:
                GuideSys.Instance.ReqGuide(pack);
                break;
            case CMD.ReqStrong:
                StrongSys.Instance.ReqStrong(pack);
                break;
            case CMD.SndChat:
                ChatSys.Instance.SndChat(pack);
                break;
            case CMD.ReqBuy:
                BuySys.Instance.ReqBuy(pack);
                break;
            case CMD.ReqTakeTaskReward:
                TaskSys.Instance.ReqTakeTaskReward(pack);
                break;
            case CMD.ReqFBFight:
                MissionSys.Instance.ReqFBFight(pack);
                break;
        }
    }
}

