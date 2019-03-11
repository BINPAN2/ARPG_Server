using System.Collections.Generic;
using PEProtocol;
/// <summary>
/// 数据缓存层
/// </summary>
public class CacheSvc
{
    private static CacheSvc instance = null;
    private CacheSvc() { }
    public static CacheSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CacheSvc();
            }
            return instance;
        }
    }

    private Dictionary<string, ServerSession> onLineAcctDic = new Dictionary<string, ServerSession>();
    private Dictionary<ServerSession, PlayerData> onLineSessionDic = new Dictionary<ServerSession, PlayerData>();

    public void Init()
    {
        PECommon.Log("CacheSvc Init Done.");
    }

    public bool IsAcctOnline(string acct)
    {
        return onLineAcctDic.ContainsKey(acct);
    }

    /// <summary>
    /// 根据账号密码返回相应账号数据，密码错误返回null，账号不存在创建默认账号
    /// </summary>
    public PlayerData GetPlayerData(string acct, string pass)
    {
        
        return DBMgr.Instance.QueryPlayerData(acct, pass);
    }

    /// <summary>
    /// 账号上线，缓存数据
    /// </summary>
    public void AcctOnline(string acct , ServerSession session , PlayerData playerData)
    {
        onLineAcctDic.Add(acct, session);
        onLineSessionDic.Add(session, playerData);
    }

    public List<ServerSession> GetOnlineServerSession()
    {
        List<ServerSession> list = new List<ServerSession>();
        foreach (var item in onLineSessionDic)
        {
            list.Add(item.Key);
        }
        return list;
    }

    public bool IsNameExist(string name)
    {
        return DBMgr.Instance.QueryNameData(name);
    }

    public PlayerData GetPlayerDataBySession(ServerSession session)
    {
        if (onLineSessionDic.TryGetValue(session, out PlayerData playerData))
        {
            return playerData;
        }

        else
        {
            return null;
        }

    }

    public bool UpdatePlayerData(int id ,PlayerData playerData,ServerSession session)
    {
        if (DBMgr.Instance.UpdatePlayerData(id, playerData))
        {
            //没有更新缓存
            onLineSessionDic[session] = playerData;
            return true;
        }
        return false;
    }

    public void AcctOffline(ServerSession session)
    {
        foreach (var item in onLineAcctDic)
        {
            if (item.Value == session)
            {
                onLineAcctDic.Remove(item.Key);
            }
        }

        bool ret = onLineSessionDic.Remove(session);
        PECommon.Log("Offline Result:" + ret + "SessionID:" + session.SessionID);
    }
}

