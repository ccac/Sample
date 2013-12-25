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

namespace YYArena.Controls
{
    public partial class Fireball : UserControl
    {
        public Fireball()
        {
            InitializeComponent();
        }

        private void aniShow_Completed(object sender, EventArgs e)
        {
            aniDisappear.Begin();
        }

        private void aniDisappear_Completed(object sender, EventArgs e)
        {
            Completed(this, EventArgs.Empty);
        }

        /// <summary>
        /// 开始火球爆炸的动画
        /// </summary>
        public void Begin()
        {
            aniShow.Begin();
        }

        /// <summary>
        /// 指定的时间间隔后开始动画
        /// </summary>
        public void Begin(TimeSpan ts)
        {
            aniShow.BeginTime = ts;
            aniShow.Begin();
        }

        private double _radius = 10d;
        /// <summary>
        /// 火球半径
        /// </summary>
        public double Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;

                ball.Width = value * 2;
                ball.Height = value * 2;
            }
        }

        /// <summary>
        /// 火球动画完成后所执行的事件
        /// </summary>
        public event EventHandler<EventArgs> Completed;
    }
}
