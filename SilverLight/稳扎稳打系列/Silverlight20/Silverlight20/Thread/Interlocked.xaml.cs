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
    public partial class Interlocked : UserControl
    {
        private static int i;

        public Interlocked()
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
                // Interlocked - 为多个线程共享的变量提供原子级的操作（避免并发问题）

                // i 加 1
                System.Threading.Interlocked.Increment(ref i);

                // i 减 1
                System.Threading.Interlocked.Decrement(ref i);

                // i 加 1
                System.Threading.Interlocked.Add(ref i, 1);

                // 如果 i 等于 100 ，则将 i 赋值为 101
                System.Threading.Interlocked.CompareExchange(ref i, 101, 100); 

                // 将 i 赋值为 1000
                // System.Threading.Interlocked.Exchange(ref i, 1000);
            }
            finally
            {
                // code
            }
        }
    }
}
