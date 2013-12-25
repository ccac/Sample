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
    public partial class ThreadStaticAttribute : UserControl
    {
        // ThreadStatic - 所指定的静态变量对每个线程都是唯一的
        [System.ThreadStatic]
        private static int value;

        // 一般的静态变量，对每个线程都是共用的
        private static int value2;

        public ThreadStaticAttribute()
        {
            InitializeComponent();

            Demo();
        }

        void Demo()
        {
            System.Threading.Thread thread = new System.Threading.Thread(DoWork);
            thread.Name = "线程1";
            thread.Start();

            System.Threading.Thread.Sleep(100);

            System.Threading.Thread thread2 = new System.Threading.Thread(DoWork2);
            thread2.Name = "线程2";
            thread2.Start();

        }

        void DoWork()
        {
            for (int i = 0; i < 10; i++)
            {
                // 线程1对静态变量的操作
                value++;
                value2++;
            }

            string s = value.ToString(); // value - 本线程独有的静态变量
            string s2 = value2.ToString(); // value2 - 所有线程共用的静态变量

            this.Dispatcher.BeginInvoke(delegate { txtMsg.Text = s + " - " + s2; });
            // this.Dispatcher.BeginInvoke(delegate { txtMsg.Text = value + " - " + value2; }); // 在UI线程上调用，所以value值为UI线程上的value值，即 0 
        }

        void DoWork2()
        {
            for (int i = 0; i < 10; i++)
            {
                // 线程2对静态变量的操作
                value++;
                value2++;
            }

            string s = value.ToString(); // value - 本线程独有的静态变量
            string s2 = value2.ToString(); // value2 - 所有线程共用的静态变量

            this.Dispatcher.BeginInvoke(delegate { txtMsg2.Text = s + " - " + s2; });
            // this.Dispatcher.BeginInvoke(delegate { txtMsg2.Text = value + " - " + value2; }); // 在UI线程上调用，所以value值为UI线程上的value值，即 0 
        }
    }
}
