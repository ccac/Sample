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
    public partial class Timer : UserControl
    {
        System.Threading.SynchronizationContext _syncContext;
        // Timer - 用于以指定的时间间隔执行指定的方法的类
        System.Threading.Timer _timer;
        private int _flag = 0;

        public Timer()
        {
            InitializeComponent();

            // UI 线程
            _syncContext = System.Threading.SynchronizationContext.Current;

            Demo();
        }

        void Demo()
        {
            // 输出当前时间
            txtMsg.Text = DateTime.Now.ToString() + "\r\n";

            // 第一个参数：定时器需要调用的方法
            // 第二个参数：传给需要调用的方法的参数
            // 第三个参数：此时间后启动定时器
            // 第四个参数：调用指定方法的间隔时间（System.Threading.Timeout.Infinite 为无穷大）
            _timer = new System.Threading.Timer(MyTimerCallback, "webabcd", 3000, 1000);
        }

        private void MyTimerCallback(object state)
        {
            string result = string.Format("{0} - {1}\r\n", DateTime.Now.ToString(), (string)state);

            // 调用 UI 线程。不会做自动线程同步
            _syncContext.Post(delegate { txtMsg.Text += result; }, null); 

            _flag++;
            if (_flag == 5)
                _timer.Change(5000, 500); // 执行5次后，计时器重置为5秒后启动，每5毫秒的间隔时间执行一次指定的方法
            else if (_flag == 10)
                _timer.Dispose(); // 执行10次后，释放计时器所使用的全部资源
        }
    }
}
