/// <summary>
/// 登录业务系统
/// </summary>
using PEProtocol;
public class LoginSys
{
    private static LoginSys instance = null;
    private LoginSys() { }
    public static LoginSys Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LoginSys();
            }
            return instance;
        }
    }

    public void Init()
    {
        PECommon.Log("LoginSys Init Done.");
    }

    public void ReqLogin(MsgPack pack)
    {
        ReqLogin data = pack.msg.reqLogin;
        //当前账号是否已经上线
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspLogin,
        };
        if (CacheSvc.Instance.IsAcctOnline(data.acct))
        {
            //已上线：返回错误信息
            msg.err = (int)ErrorCode.AcctIsOnLine;
        }
        else
        {
            //未上线：
            PlayerData pd = CacheSvc.Instance.GetPlayerData(data.acct, data.pass);
            if (pd == null)
            {
                //存在：密码错误
                msg.err = (int)ErrorCode.WrongPass;
            }
            else
            {
                msg.rspLogin = new RspLogin
                {
                    playerData = pd
                };

                CacheSvc.Instance.AcctOnline(data.acct, pack.session, pd);
            }


        }


        //回应客户端

        pack.session.SendMsg(msg);

    }

    public void ReqRename(MsgPack pack)
    {
        ReqRename data = pack.msg.reqRename;
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspRename
        };

        if (CacheSvc.Instance.IsNameExist(data.name))
        {
            //名字是否已经存在
            //存在：返回错误码
            msg.err =(int) ErrorCode.NameIsExist;
        }
        else
        {
            //不存在：更新缓存，数据库，在返回给客户端
            PlayerData playerData = CacheSvc.Instance.GetPlayerDataBySession(pack.session);
            playerData.name = data.name;

            if (!CacheSvc.Instance.UpdatePlayerData(playerData.id,playerData))
            {
                msg.err = (int)ErrorCode.UpdateDBError;
            }
            else
            {
                msg.rspRename = new RspRename
                {
                    name = data.name
                };
            }
        }

        pack.session.SendMsg(msg);
    }

    public void ClearOffline(ServerSession session)
    {
        CacheSvc.Instance.AcctOffline(session);
    }
}

