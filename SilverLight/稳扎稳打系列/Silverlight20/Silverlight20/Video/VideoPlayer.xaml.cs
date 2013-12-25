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

namespace Silverlight20.Video
{
    public partial class VideoPlayer : UserControl
    {
        // 媒体的时长
        private TimeSpan _duration;

        private DispatcherTimer _timer = new DispatcherTimer();

        public VideoPlayer()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(VideoPlayer_Loaded);

            /*
             * MediaOpened - 当媒体被成功地打开时所触发的事件
             * MediaFailed - 当媒体未能被成功地打开时所触发的事件
             * CurrentStateChanged - 播放状态（CurrentState）发生改变时所触发的事件
             * DownloadProgressChanged - 下载进度（DownloadProgress）发生变化时所触发的事件（当下载增加量大于等于 0.05 或下载进度增加到 1 时会触发此事件）
             * MediaEnded - 当媒体播放到末尾时所触发的事件
             * BufferingProgressChanged - 缓冲进度（BufferingProgress）发生变化时所触发的事件（当缓冲增加量大于等于 0.05 或缓冲进度增加到 1 时会触发此事件）
             */

            mediaElement.MediaOpened += new RoutedEventHandler(mediaElement_MediaOpened);
            mediaElement.CurrentStateChanged += new RoutedEventHandler(mediaElement_CurrentStateChanged);
            mediaElement.DownloadProgressChanged += new RoutedEventHandler(mediaElement_DownloadProgressChanged);
            mediaElement.MediaEnded += new RoutedEventHandler(mediaElement_MediaEnded);
            mediaElement.BufferingProgressChanged += new RoutedEventHandler(mediaElement_BufferingProgressChanged);
        }

        void VideoPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            // 每 500 毫秒调用一次指定的方法
            _timer.Interval = TimeSpan.FromMilliseconds(500);
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            // CurrentState - 播放状态 [System.Windows.Media.MediaElementState枚举]
            // Position - 媒体的位置（单位：秒）
            if (mediaElement.CurrentState == MediaElementState.Playing)
            {
                lblPlayTime.Text = string.Format(
                    "{0}{1:00}:{2:00}:{3:00}",
                    "播放进度：",
                    mediaElement.Position.Hours,
                    mediaElement.Position.Minutes,
                    mediaElement.Position.Seconds);
            }

            // DroppedFramesPerSecond - 媒体每秒正在丢弃的帧数
            lblDroppedFramesPerSecond.Text = "每秒正在丢弃的帧数：" + mediaElement.DroppedFramesPerSecond.ToString();
        }

        void mediaElement_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            // BufferingProgress - 缓冲进度（0 - 1 之间）
            lblBufferingProgress.Text = string.Format(
                "缓冲进度：{0:##%}",
                mediaElement.BufferingProgress);
        }

        void mediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
        }

        void mediaElement_DownloadProgressChanged(object sender, RoutedEventArgs e)
        {
            // DownloadProgress - 下载进度（0 - 1 之间）
            lblDownloadProgress.Text = string.Format(
                "下载进度：{0:##%}",
                mediaElement.DownloadProgress);
        }
        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            /*
             * NaturalVideoWidth - 媒体文件的宽
             * NaturalVideoHeight - 媒体文件的高
             * HasTimeSpan - 是否可取得媒体文件的时长
             * NaturalDuration - 媒体文件的时长
             * Volume - 音量大小（0 - 1 之间）
             * Balance - 音量平衡（-1 - 1 之间）
             * BufferingTime - 需要缓冲的时间的长度
             */

            lblWidth.Text = "媒体文件的宽：" + mediaElement.NaturalVideoWidth.ToString();
            lblHeight.Text = "媒体文件的高：" + mediaElement.NaturalVideoHeight.ToString();

            _duration = mediaElement.NaturalDuration.HasTimeSpan ? mediaElement.NaturalDuration.TimeSpan : TimeSpan.FromMilliseconds(0);

            lblTotalTime.Text = string.Format(
                "{0}{1:00}:{2:00}:{3:00}", "时长：",
                _duration.Hours,
                _duration.Minutes,
                _duration.Seconds);

            mediaElement.Volume = 0.8;
            volumeSlider.Value = 0.8;
            lblVolume.Text = "音量大小：80%";

            mediaElement.Balance = 0;
            balanceSlider.Value = 0;
            lblBalance.Text = "音量平衡：0%";

            mediaElement.BufferingTime = TimeSpan.FromSeconds(30);
            lblBufferingTime.Text = "缓冲长度：30秒";
        }

        private void mediaElement_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            /*
             * CurrentState - 播放状态 [System.Windows.Media.MediaElementState枚举]
             *     MediaElementState.Closed - 无可用媒体
             *     MediaElementState.Opening - 尝试打开媒体（此时Play(),Pause(),Stop()命令会被排进队列，等到媒体被成功打开后再依次执行）
             *     MediaElementState.Buffering - 缓冲中
             *     MediaElementState.Playing - 播放中
             *     MediaElementState.Paused - 被暂停（显示当前帧）
             *     MediaElementState.Stopped - 被停止（显示第一帧）
             */

            lblState.Text = "播放状态：" + mediaElement.CurrentState.ToString();
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
            // Play() - 播放媒体（在当前 Position 处播放）
            mediaElement.Play();
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            // CanPause - 媒体是否可暂停
            // Pause() - 暂停媒体的播放
            if (mediaElement.CanPause)
                mediaElement.Pause();
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            // Stop() - 停止媒体的播放
            mediaElement.Stop();
        }

        void mute_Click(object sender, RoutedEventArgs e)
        {
            // IsMuted - 是否静音
            if (mediaElement.IsMuted == true)
            {
                mute.Content = "静音";
                mediaElement.IsMuted = false;
            }
            else
            {
                mute.Content = "有声";
                mediaElement.IsMuted = true;
            }
        }

        private void playSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // CanSeek - 是否可以通过设置 Position 来重新定位媒体
            // Position - 媒体的位置（单位：秒）
            if (mediaElement.CanSeek)
            {
                mediaElement.Pause();
                mediaElement.Position = TimeSpan.FromSeconds(_duration.TotalSeconds * playSlider.Value);
                mediaElement.Play();
            }
        }

        private void volumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Volume - 音量大小（0 - 1 之间）
            mediaElement.Volume = volumeSlider.Value;
            lblVolume.Text = string.Format(
                "音量大小：{0:##%}",
                volumeSlider.Value);
        }

        private void balanceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Balance - 音量平衡（-1 - 1 之间）
            mediaElement.Balance = balanceSlider.Value;
            lblBalance.Text = string.Format(
                "音量平衡：{0:##%}",
                balanceSlider.Value);
        }
    }
}
