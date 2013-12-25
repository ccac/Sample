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
    public partial class PointAnimation : UserControl
    {
        public PointAnimation()
        {
            InitializeComponent();
        }

        private void Animation_Begin(object sender, RoutedEventArgs e)
        {
            // 播放
            storyboard.Begin();
        }

        private void Animation_Pause(object sender, RoutedEventArgs e)
        {
            // 暂停
            storyboard.Pause();
        }

        private void Animation_Resume(object sender, RoutedEventArgs e)
        {
            // 继续
            storyboard.Resume();
        }

        private void Animation_Stop(object sender, RoutedEventArgs e)
        {
            // 停止
            storyboard.Stop();
        }
    }
}
