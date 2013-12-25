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

namespace YYFly.Controls
{
    public partial class Sound : UserControl
    {
        public Sound()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 开火，用于播放开枪的声音
        /// </summary>
        public void Fire()
        {
            bomb.Position = TimeSpan.FromMilliseconds(0);
            bomb.Play();
        }

        /// <summary>
        /// 用于循环播放背景音乐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void background_MediaEnded(object sender, RoutedEventArgs e)
        {
            background.Position = TimeSpan.FromMilliseconds(0);
            background.Play();
        }
    }
}
