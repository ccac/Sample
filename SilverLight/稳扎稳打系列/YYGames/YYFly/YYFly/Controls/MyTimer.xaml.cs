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

using System.Windows.Threading;

namespace YYFly.Controls
{
    public partial class MyTimer : UserControl
    {
        DispatcherTimer timer;

        public MyTimer()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MyTimer_Loaded);
        }

        void MyTimer_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(timer_Tick);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            time.Text = (int.Parse(time.Text) - 1).ToString();

            if (time.Text == "0")
            {
                OnStop();
                timer.Stop();
            }
        }

        /// <summary>
        /// 启动定时器。用于 60 秒倒计时
        /// </summary>
        public void Start()
        {
            time.Text = "60";
            timer.Start();
        }

        /// <summary>
        /// 游戏停止事件
        /// </summary>
        public event EventHandler Stop;
        public void OnStop()
        {
            if (Stop != null)
                Stop(this, EventArgs.Empty);
        }
    }
}
