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

namespace Silverlight20.Control
{
    public partial class MediaElement : UserControl
    {
        public MediaElement()
        {
            InitializeComponent();
        }

        void play_Click(object sender, RoutedEventArgs e)
        {
            var tb = sender as System.Windows.Controls.Primitives.ToggleButton;
            if (tb.IsChecked == true)
            {
                tb.Content = "暂停";

                // MediaElement.Play() - 播放视频
                this.mediaElement.Play();
            }
            else
            {
                tb.Content = "播放";

                // MediaElement.Pause() - 暂停视频
                // MediaElement.Stop() - 停止视频
                this.mediaElement.Pause();
            }
        }

        void mute_Click(object sender, RoutedEventArgs e)
        {
            var tb = sender as System.Windows.Controls.Primitives.ToggleButton;
            if (tb.IsChecked == true)
            {
                tb.Content = "有声";

                // MediaElement.IsMuted - 是否静音
                // MediaElement.Volume - 声音大小（0 - 1）
                this.mediaElement.IsMuted = true;
            }
            else
            {
                tb.Content = "静音";
                this.mediaElement.IsMuted = false;
            }
        }
    }
}
