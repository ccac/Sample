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

namespace YYPiano.Controls.Parts
{
    public partial class ScalePlayer : UserControl
    {
        // MediaElement 控件总数
        private int _count = 32;

        // MediaElement 控件集合的索引
        private int _index = 0;

        public ScalePlayer()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Player_Loaded);
        }

        void Player_Loaded(object sender, RoutedEventArgs e)
        {
            // 在 Canvas 上添加指定数量的 MediaElement 控件
            for (int i = 0; i < _count; i++)
            {
                var element = new MediaElement();
                element.Volume = 1d;

                root.Children.Add(element);
            }
        }

        /// <summary>
        /// 播放音阶
        /// A 键对应 Scale 文件夹内的 A.mp3，以此类推
        /// A 键对应 C 大调的低音 dou，以此类推
        /// </summary>
        /// <param name="key">键值</param>
        public void Play(Key key)
        {
            if (key >= Key.A && key <= Key.Z)
            {
                // 循环使用 MediaElement 控件集合中的控件
                if (_index > _count - 1)
                    _index = 0;

                // 设置 MediaElement 的 Source 并播放
                var element = root.Children[_index] as MediaElement;
                element.Source = new Uri("/YYPiano;component/Scale/" + key.ToString() + ".mp3", UriKind.Relative);
                element.Stop();
                element.Play();

                _index++;
            }
        }
    }
}
