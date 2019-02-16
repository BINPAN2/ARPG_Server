using PEProtocol;
using PENet;
public class ServerSession : PESession<GameMsg>
{
    public int SessionID = 0;

    protected override void OnConnected()
    {
        SessionID = ServerRoot.Instance.GetSessionID();
        PECommon.Log("SessionID"+SessionID+"Client Connect");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("SessionID" + SessionID + "RcvPack CMD:" + ((CMD)msg.cmd).ToString());
        Netsvc.Instance.AddMsgQue(new MsgPack(this,msg));
    }

    protected override void OnDisConnected()
    {
        LoginSys.Instance.ClearOffline(this);
        PECommon.Log("SessionID" + SessionID + "Client Disconnect");
    }
}
