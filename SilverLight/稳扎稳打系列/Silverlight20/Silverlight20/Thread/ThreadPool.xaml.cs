using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Silverlight20.Thread
{
    public partial class ThreadPool : UserControl
    {
        public ThreadPool()
        {
            InitializeComponent();
        }

        private void txtMsgQueueUserWorkItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // ThreadPool - 线程池的管理类

            // QueueUserWorkItem(WaitCallback callBack, Object state) - 将指定方法加入线程池队列
            //     WaitCallback callBack - 需要在新开线程里执行的方法
            //     Object state - 传递给指定方法的参数
            System.Threading.ThreadPool.QueueUserWorkItem(DoWork, DateTime.Now);
        }

        private void DoWork(object state)
        {
            // 作为线程管理策略的一部分，线程池在创建线程前会有一定的延迟
            // 也就是说线程入队列的时间和线程启动的时间之间有一定的间隔

            DateTime dtJoin = (DateTime)state;
            DateTime dtStart = DateTime.Now;
            System.Threading.Thread.Sleep(3000);
            DateTime dtEnd = DateTime.Now;

            // Dispatcher.BeginInvoke() - 在与 Dispatcher 相关联的线程上执行指定的操作。自动线程同步
            this.Dispatcher.BeginInvoke(() =>
            {
                txtMsgQueueUserWorkItem.Text += string.Format("\r\n入队列时间{0} 启动时间{1} 完成时间{2}",
                    dtJoin.ToString(), dtStart.ToString(), dtEnd.ToString());
            });
        }


        private void txtRegisterWaitForSingleObject_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Threading.AutoResetEvent done = new System.Threading.AutoResetEvent(false);

            // 为了传递 RegisteredWaitHandle 对象，要将其做一个封装
            RegisteredWaitHandlePacket packet = new RegisteredWaitHandlePacket();

            // RegisterWaitForSingleObject - 注册一个 WaitHandle 。在超时或发信号的情况下对指定的回调方法做调用
            // 第一个参数：需要注册的 WaitHandle
            // 第二个参数：需要回调的方法（此处省略掉了委托类型）
            // 第三个参数：传递给回调方法的参数
            // 第四个参数：超时时间（到超时时间则调用指定的方法）
            // 第五个参数：是否为一次调用（是到超时时间一次性调用指定的方法，还是每次超时时间后都调用指定的方法）
            packet.Handle = System.Threading.ThreadPool.RegisterWaitForSingleObject
                (
                    done,
                    WaitOrTimer,
                    packet,
                    100,
                    false
                );

            System.Threading.Thread.Sleep(555);
            done.Set(); // 发出信号，调用 RegisterWaitForSingleObject 所指定的方法
        }

        public void WaitOrTimer(object state, bool timedOut)
        {
            RegisteredWaitHandlePacket packet = state as RegisteredWaitHandlePacket;

            // bool timedOut - 是否是因为超时而执行到这里
            if (!timedOut) 
            {
                // 如果不是因为超时而执行到这里（即因为 AutoResetEvent 发出了信号而执行到这里），则注销指定的 RegisteredWaitHandle
                packet.Handle.Unregister(null);
            }

            this.Dispatcher.BeginInvoke(() =>
            {
                txtRegisterWaitForSingleObject.Text +=
                    String.Format("\r\n是否收到信号：{0}", (!timedOut).ToString());
            });
        }
    }

    /// <summary>
    /// 封装了 RegisteredWaitHandle 的类
    /// </summary>
    public class RegisteredWaitHandlePacket
    {
        public System.Threading.RegisteredWaitHandle Handle { get; set; }
    }
}
