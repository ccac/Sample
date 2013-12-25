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
    public partial class SoundEffect : UserControl
    {
        /// <summary>
        /// 爆炸效果音效
        /// </summary>
        private string _bombAddress = "/Resource/bomb.mp3";
        /// <summary>
        /// 开火效果音效
        /// </summary>
        private string _fireAddress = "/Resource/fire.mp3";

        // MediaElement 控件总数
        private int _count = 32;

        // MediaElement 控件集合的索引
        private int _index = 0;

        public SoundEffect()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(SoundEffect_Loaded);
        }

        void SoundEffect_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _count; i++)
            {
                var element = new MediaElement();
                element.Volume = 0.5d;

                root.Children.Add(element);
            }
        }

        private void Play(string address)
        {
            // 循环使用 MediaElement 控件集合中的控件
            if (_index > _count - 1)
                _index = 0;

            // 设置 MediaElement 的 Source 并播放
            var element = root.Children[_index] as MediaElement;
            element.Source = new Uri(address, UriKind.Relative);
            element.Stop();
            element.Play();

            _index++;
        }

        /// <summary>
        /// 播放爆炸音效
        /// </summary>
        public void PlayBomb()
        {

            Play(_bombAddress);
        }

        /// <summary>
        /// 播放开火音效
        /// </summary>
        public void PlayFire()
        {
            Play(_fireAddress);
        }

        private void bg_MediaEnded(object sender, RoutedEventArgs e)
        {
            bg.Position = TimeSpan.FromMilliseconds(0);
            bg.Play();
        }
    }
}
