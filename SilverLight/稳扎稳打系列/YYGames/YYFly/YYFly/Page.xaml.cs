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

using YYFly.Model;
using YYFly.View;
using YYFly.Presenter;
using System.Windows.Threading;

namespace YYFly
{
    public partial class Page : UserControl
    {
        DispatcherTimer timer; // 生成苍蝇的计时器
        List<Fly> flies; // 苍蝇集合
        int flyIndex; // 当前苍蝇的索引

        public Page()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.None;

            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);

            Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);

            // 初始化苍蝇。32只苍蝇的集合
            flies = new List<Fly>();
            for (int i = 0; i < 32; i++)
            {
                var flyView = new Fly();
                flyView.InitPosition();
                var flyModel = new FlyModel();
                FlyPresenter presenter = new FlyPresenter(flyView, flyModel);
                flies.Add(flyView);
                flyContainer.Children.Add(flyView);
            }
            flyIndex = 0;

            panelScorer.DataContext = Singleton<Scorer>.Instance;

            gameTip.Start += new EventHandler(gameTip_Start);
            myTimer.Stop += new EventHandler(myTimer_Stop);
        }

        void myTimer_Stop(object sender, EventArgs e)
        {
            gameTip.Visibility = Visibility.Visible;
            gameTip.ButtonText = "游戏结束。重新开始。";
            timer.Stop();
        }

        /// <summary>
        /// 开始游戏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gameTip_Start(object sender, EventArgs e)
        {
            foreach (var fly in flies)
            {
                fly.OnStop();
            }

            Singleton<Scorer>.Instance.Score = 0;
            Singleton<Scorer>.Instance.Level = 1;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            gameTip.Visibility = Visibility.Collapsed;
            myTimer.Start();
            timer.Start();
        }

        /// <summary>
        /// 每次计时器到此时启动下一只苍蝇
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            flies[flyIndex].OnStart();

            int minisecond = 1000 - (Singleton<Scorer>.Instance.Level - 1) * 100;
            if (minisecond < 100)
                minisecond = 100;
            timer.Interval = new TimeSpan(0, 0, 0, 0, minisecond);

            flyIndex += 1;
            if (flyIndex > 31)
                flyIndex = 0;
        }

        /// <summary>
        /// 全屏逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Content_FullScreenChanged(object sender, EventArgs e)
        {
            if (Application.Current.Host.Content.IsFullScreen)
            {
                st.ScaleX = Application.Current.Host.Content.ActualWidth / 640;
                st.ScaleY = Application.Current.Host.Content.ActualHeight / 480;
            }
            else
            {
                st.ScaleX = 1;
                st.ScaleY = 1;
            }
        }

        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
        }

        /// <summary>
        /// 移动瞄准镜
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(sender as FrameworkElement);
            gun.SetValue(Canvas.LeftProperty, p.X / st.ScaleX - gun.ActualWidth / 2);
            gun.SetValue(Canvas.TopProperty, p.Y / st.ScaleY - gun.ActualHeight / 2);
        }

        /// <summary>
        /// 开火
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gun.Fire();
            sound.Fire();
        }

        /// <summary>
        /// 复位瞄准镜
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            gun.Reset();
        }
    }
}
