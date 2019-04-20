using PENet;
using PEProtocol;
using System.Collections.Generic;
/// <summary>
/// 聊天业务系统
/// </summary>
public class ChatSys
{
    private static ChatSys instance = null;
    private ChatSys() { }
    public static ChatSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ChatSys();
            }
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("ChatSys Init Done.");
    }

    public void SndChat(MsgPack pack)
    {
        SndChat data = pack.msg.sndChat;
        PlayerData pd = CacheSvc.Instance.GetPlayerDataBySession(pack.session);

        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.PshChat,
            pshChat = new PshChat
            {
                name = pd.name,
                Chat = data.Chat
            }
        };

        //广播
        List<ServerSession> list = CacheSvc.Instance.GetOnlineServerSession();
        byte[] bytes = PENet.PETool.PackNetMsg(msg);
        for (int i = 0; i < list.Count; i++)
        {
            list[i].SendMsg(bytes);
        }

        TaskSys.Instance.CalcTaskPrgs(pd, 6);//更新任务进度数据
    }
}
