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

namespace Silverlight20.Animation
{
    public partial class Programmatically : UserControl
    {
        public Programmatically()
        {
            InitializeComponent();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 鼠标相对与canvas的坐标
            double newX = e.GetPosition(canvas).X;
            double newY = e.GetPosition(canvas).Y;
            Point myPoint = new Point(newX, newY);

            // 将动画的结束值设置为鼠标的当前坐标
            pointAnimation.To = myPoint;

            // 播放动画
            storyboard.Begin();
        }
    }
}
