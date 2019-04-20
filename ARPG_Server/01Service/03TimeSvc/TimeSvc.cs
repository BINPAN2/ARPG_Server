using System;
using System.Collections.Generic;

public class TaskPack
{
    public int tid;
    public Action<int> cb;
    public TaskPack(Action<int>cb,int tid)
    {
        this.tid = tid;
        this.cb = cb;
    }
}

public class TimeSvc
{

    private static TimeSvc instance = null;
    private TimeSvc() { }
    public static TimeSvc Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TimeSvc();
            }
            return instance;
        }
    }

    private PETimer pt = null;
    private Queue<TaskPack> tpQue = new Queue<TaskPack>();
    public static readonly string tpQueLock = "tpQueLock";
    public void Init()
    {
        pt = new PETimer(100);
        tpQue.Clear();
        //设置日志输出
        pt.SetLog((string info) =>
        {
            PECommon.Log(info);
        });

        pt.SetHandle((Action<int> cb,int tid) =>{
            if (cb!=null)
            {
                lock (tpQueLock)
                {
                    tpQue.Enqueue(new TaskPack(cb, tid));
                }
            }
        });

        PECommon.Log("TimeSvc Init Done.");
    }


    public void Update()
    {
        while (tpQue.Count>0)
        {
            TaskPack tp = null;
            lock (tpQueLock)
            {
                tp = tpQue.Dequeue();
            }

            if (tp!=null)
            {
                tp.cb(tp.tid);
            }
        }
    }

    public int AddTimeTask(Action<int> callback, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        return pt.AddTimeTask(callback, delay, timeUnit, count);
    }

    public long GetNowTime()
    {
        return (long)pt.GetMillisecondsTime();
    }
}

