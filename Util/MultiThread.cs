using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Beinet.cn.Tools.Util
{
    /// <summary>
    /// 多线程，对数组里的数据，进行处理
    /// </summary>
    public class MultiThread
    {
        /// <summary>
        /// 存储数据的队列
        /// </summary>
        private ConcurrentQueue<object> _queue = new ConcurrentQueue<object>();

        private SemaphoreSlim _sem;

        /// <summary>
        /// 处理数据的方法
        /// </summary>
        public WaitCallback Method { get; private set; }

        /// <summary>
        /// 任务是否完成
        /// </summary>
        public bool Completed => ProcessCount == AddCount;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; private set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; private set; }

        /// <summary>
        /// 添加的数据计数
        /// </summary>
        public int AddCount { get; private set; }

        private int _processCount;
        /// <summary>
        /// 处理的数据计数
        /// </summary>
        public int ProcessCount
        {
            get => _processCount;
        }
        /// <summary>
        /// 过程中的错误统计
        /// </summary>
        public Dictionary<string, Exception> Exceptions { get; private set; } = new Dictionary<string, Exception>();

        public MultiThread(WaitCallback method, int threadNum)
        {
            Method = method;
            // 参数1，启动时释放几个信号量；参数2:，最多允许释放几个信号量
            _sem = new SemaphoreSlim(threadNum, threadNum);
        }

        /// <summary>
        /// 添加数据到队列中
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Add(object obj)
        {
            if (obj == null)
                return _queue.Count;

            _queue.Enqueue(obj);
            AddCount++;
            return _queue.Count;
        }

        public void Start()
        {
            StartTime = DateTime.Now;
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                var waitSeconds = 0;
                while (true)
                {
                    // 1分钟内读取不到数据时，退出
                    if (!_queue.TryDequeue(out var data))
                    {
                        waitSeconds++;
                        if (waitSeconds > 60)
                        {
                            EndTime = DateTime.Now;
                            return;
                        }

                        Thread.Sleep(1000);
                        continue;
                    }

                    _sem.Wait();

                    ThreadPool.UnsafeQueueUserWorkItem(item =>
                    {
                        try
                        {
                            Method(item);
                        }
                        catch (Exception exp)
                        {
                            Exceptions.Add(item.ToString(), exp);
                        }
                        finally
                        {
                            var cnt = Interlocked.Increment(ref _processCount);
                            if (cnt >= AddCount)
                            {
                                EndTime = DateTime.Now;
                            }
                            _sem.Release();
                        }
                    }, data);
                }

            }, null);
        }
    }
}
