using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace YYFly.View
{
    public partial class Fly : UserControl, IFlyView
    {
        public Fly()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Fly_Loaded);
        }

        void Fly_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// 更新苍蝇的位置
        /// </summary>
        /// <param name="x">X 轴坐标</param>
        /// <param name="y">Y 轴坐标</param>
        /// <param name="z">Z 轴方向上的缩放比例</param>
        public void Update(double x, double y, double z)
        {
            fly.SetValue(Canvas.LeftProperty, x);
            fly.SetValue(Canvas.TopProperty, y);
            st.ScaleX = st.ScaleY = z;
        }

        /// <summary>
        /// 打到苍蝇后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fly_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (aniWing.GetCurrentState() != ClockState.Stopped)
            {
                aniFlyDisappear.Begin();

                OnScore();
            }
        }

        /// <summary>
        /// 苍蝇消失的动画结束后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aniFlyDisappear_Completed(object sender, EventArgs e)
        {
            OnStop();
        }

        /// <summary>
        /// 初始化苍蝇的位置
        /// </summary>
        public void InitPosition()
        {
            fly.SetValue(Canvas.LeftProperty, -300d);
        }

        public event EventHandler Start;
        public void OnStart()
        {
            InitPosition();

            aniFlyDisappear.Stop();

            if (Start != null)
                Start(this, EventArgs.Empty);
        }

        public event EventHandler Stop;
        public void OnStop()
        {
            InitPosition();

            if (Stop != null)
                Stop(this, EventArgs.Empty);
        }

        public event EventHandler Score;
        public void OnScore()
        {
            if (Score != null)
                Score(this, EventArgs.Empty);
        }
    }
}