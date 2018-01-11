using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace Beinet.cn.Tools
{
    /// <summary>
    /// 线程调度者
    /// </summary>
    public class ThreadScheduler
    {
        private readonly ConcurrentQueue<Tuple<WaitCallback, object>> _queue;
        private readonly int _maxQueueLength;
        private readonly Action<Exception> _exProc;
        private readonly int _expWaitSecond;
        private DateTime _expWaitToTime = DateTime.MinValue;
        private bool _startWork;

        /// <summary>
        /// 队列中的任务数
        /// </summary>
        public int QueueLength
        {
            get { return _queue.Count; }
        }

        /// <summary>
        /// 线程调度
        /// </summary>
        /// <param name="maxQueueLength">队列中的最大任务数</param>
        /// <param name="exProc">异常委托函数</param>
        /// <param name="expWaitSecond">
        /// 出现异常时，等待多久进行下一次消息出队处理，0表示不等待.
        /// 此参数让所有后台工作进程都进入等待，而exProc只能让一个工作进程进入等待
        /// </param>
        public ThreadScheduler(int maxQueueLength = 10000, 
            Action<Exception> exProc = null, int expWaitSecond = 0)
        {
            _maxQueueLength = maxQueueLength;
            _exProc = exProc;
            _queue = new ConcurrentQueue<Tuple<WaitCallback, object>>();
            _expWaitSecond = expWaitSecond;
        }

        /// <summary>
        /// 处理推荐回收消息
        /// 注: 当任务队列满后, 返回false
        /// </summary>
        public bool QueueUserWorkItem(WaitCallback callBack, object state)
        {
            if (_queue.Count >= _maxQueueLength)
                return false;

            // 入队
            _queue.Enqueue(new Tuple<WaitCallback, object>(callBack, state));
            return true;
        }

        /// <summary>
        /// 后台工作
        /// </summary>
        public void StartWork(int workerNum)
        {
            _startWork = true;
            for (int i = 0; i < workerNum; i++)
            {
                var thread = new Thread(() =>
                {
                    while (_startWork)
                    {
                        Sleep();

                        // 判断队列中是否存在任务
                        Tuple<WaitCallback, object> task;
                        if (_queue.TryDequeue(out task))
                        {
                            if (task != null) // 取到任务
                            {
                                Sleep();
                                try
                                {
                                    task.Item1(task.Item2);
                                    continue; // 任务执行完成, 立即请求下一个任务
                                }
                                catch (Exception ex)
                                {
                                    // 此处不用考虑并发
                                    if (_expWaitSecond > 0)
                                        _expWaitToTime = DateTime.Now.AddSeconds(_expWaitSecond);

                                    if (_exProc != null)
                                    {
                                        // 套上try，避免异常处理函数自己抛出异常，导致线程异常，从而影响站点运行
                                        try
                                        {
                                            _exProc(ex);
                                        }
                                        // ReSharper disable once EmptyGeneralCatchClause
                                        catch { }
                                    }
                                }
                            }
                        }
                        // 队列为空时, 休眠一段时间, 避免CPU消耗
                        Thread.Sleep(10);
                    }
                    // ReSharper disable once FunctionNeverReturns
                });

                // 避免主线程退出时，子线程无法关闭
                thread.IsBackground = true;
                thread.Start();
            }
        }

        public void StopWork()
        {
            _startWork = false;
        }

        void Sleep()
        {
            // 出现异常，并设置了异常等待时间时，等待一段时间后，再出队数据处理
            if (_expWaitSecond > 0)
            {
                while (_expWaitToTime >= DateTime.Now)
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
