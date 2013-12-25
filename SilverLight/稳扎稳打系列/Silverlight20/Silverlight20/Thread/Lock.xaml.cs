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
    public partial class Lock : UserControl
    {
        // 需要被 lock 的静态变量
        private static readonly object objLock = new object();

        private static int i;

        public Lock()
        {
            InitializeComponent();

            i = 0;

            for (int x = 0; x < 100; x++)
            {
                // 开 100 个线程去操作静态变量 i
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(DoWork));
                thread.Start();
            }

            System.Threading.Thread.Sleep(3000);
            // 3 秒后 100 个线程都应该执行完毕了，取得 i 的结果
            // 做了并发处理的结果为 100 ，去掉 lock 可得到不做并发处理的结果
            txtMsg.Text = i.ToString();
        }

        private void DoWork()
        {
            try
            {
                // lock() - 确保代码块完成运行，而不会被其他线程中断。其参数必须为一个引用类型的对象
                lock (objLock)
                {
                    int j = i + 1;

                    // 模拟多线程并发操作静态变量 i 的情况
                    System.Threading.Thread.Sleep(10);

                    i = j;
                }
            }
            finally
            {
                // code
            }
        }
    }
}
