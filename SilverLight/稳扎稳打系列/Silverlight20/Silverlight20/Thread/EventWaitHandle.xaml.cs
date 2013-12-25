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
    public partial class EventWaitHandle : UserControl
    {
        // AutoResetEvent(bool state) - 通知其他线程是否可入的类，自动 Reset()
        //     bool state - 是否为终止状态，即是否禁止其他线程入内
        private System.Threading.AutoResetEvent autoResetEvent = 
            new System.Threading.AutoResetEvent(false);

        // ManualResetEvent(bool state) - 通知其他线程是否可入的类，手动 Reset()
        //     bool state - 是否为终止状态，即是否禁止其他线程入内
        private System.Threading.ManualResetEvent manualResetEvent = 
            new System.Threading.ManualResetEvent(false);

        private static int i;

        public EventWaitHandle()
        {
            InitializeComponent();

            // 演示 AutoResetEvent
            AutoResetEventDemo();

            // 演示 ManualResetEvent
            ManualResetEventDemo();
        }

        private void AutoResetEventDemo()
        {
            i = 0;

            for (int x = 0; x < 100; x++)
            {
                // 开 100 个线程去操作静态变量 i
                System.Threading.Thread thread =
                    new System.Threading.Thread(new System.Threading.ThreadStart(AutoResetEventDemoCallback));
                thread.Start();

                // 阻塞当前线程，直到 AutoResetEvent 发出 Set() 信号
                autoResetEvent.WaitOne();
            }

            System.Threading.Thread.Sleep(1000);
            // 1 秒后 100 个线程都应该执行完毕了，取得 i 的结果
            txtAutoResetEvent.Text = i.ToString();
        }

        private void AutoResetEventDemoCallback()
        {
            try
            {
                int j = i + 1;

                // 模拟多线程并发操作静态变量 i 的情况
                System.Threading.Thread.Sleep(5);

                i = j;
            }
            finally
            {
                // 发出 Set() 信号，以释放 AutoResetEvent 所阻塞的线程
                autoResetEvent.Set();
            }
        }


        private void ManualResetEventDemo()
        {
            i = 0;

            for (int x = 0; x < 100; x++)
            {
                // Reset() - 将 ManualResetEvent 变为非终止状态，即由此线程控制 ManualResetEvent，
                //     其他线程排队，直到 ManualResetEvent 发出 Set() 信号（AutoResetEvent 在 Set() 时会自动 Reset()）
                manualResetEvent.Reset();

                // 开 100 个线程去操作静态变量 i
                System.Threading.Thread thread =
                    new System.Threading.Thread(new System.Threading.ThreadStart(ManualResetEventDemoCallback));
                thread.Start();

                // 阻塞当前线程，直到 ManualResetEvent 发出 Set() 信号
                manualResetEvent.WaitOne();
            }

            System.Threading.Thread.Sleep(1000);
            // 1 秒后 100 个线程都应该执行完毕了，取得 i 的结果
            txtManualResetEvent.Text = i.ToString();
        }

        private void ManualResetEventDemoCallback()
        {
            try
            {
                int j = i + 1;

                // 模拟多线程并发操作静态变量 i 的情况
                System.Threading.Thread.Sleep(5);

                i = j;
            }
            finally
            {
                // 发出 Set() 信号，以释放 ManualResetEvent 所阻塞的线程，同时 ManualResetEvent 变为终止状态）
                manualResetEvent.Set();
            }
        }
    }
}
