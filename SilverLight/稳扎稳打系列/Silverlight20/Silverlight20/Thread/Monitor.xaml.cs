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
    public partial class Monitor : UserControl
    {
        private static readonly object objLock = new object();
        private static int i;
        
        public Monitor()
        {
            InitializeComponent();

            i = 0;

            for (int x = 0; x < 100; x++)
            {
                // 开 100 个线程去操作静态变量 i
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(DoWork));
                thread.Start();
            }

            System.Threading.Thread.Sleep(1000);
            // 1 秒后 100 个线程都应该执行完毕了，取得 i 的结果
            txtMsg.Text = i.ToString();
        }

        private void DoWork()
        {
            try
            {
                // Monitor - 提供同步访问对象的机制

                // Enter() - 在指定对象上获取排他锁
                System.Threading.Monitor.Enter(objLock);

                int j = i + 1;

                // 模拟多线程并发操作静态变量 i 的情况
                System.Threading.Thread.Sleep(5);

                i = j;

                // Exit() - 释放指定对象上的排他锁
                System.Threading.Monitor.Exit(objLock);
            }
            finally
            {
                // code
            }
        }
    }
}