using PEProtocol;
using PENet;
public class ServerSession : PESession<GameMsg>
{
    protected override void OnConnected()
    {
        PECommon.Log("Client Connect");

    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("RcvPack CMD:"+ ((CMD)msg.cmd).ToString());
        Netsvc.Instance.AddMsgQue(new MsgPack(this,msg));
    }

    protected override void OnDisConnected()
    {
        PECommon.Log("Client Disconnect");
    }
}
