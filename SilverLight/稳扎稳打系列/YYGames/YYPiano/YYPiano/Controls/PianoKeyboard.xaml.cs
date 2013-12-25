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

using YYPiano.Controls.Parts;

namespace YYPiano.Controls
{
    public partial class PianoKeyboard : UserControl
    {
        public PianoKeyboard()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Keyboard_Loaded);
        }

        void Keyboard_Loaded(object sender, RoutedEventArgs e)
        {
            // 用 26 个钢琴键构造钢琴键盘
            for (int i = 0; i < 26; i++)
            {
                PianoKey pianoKey = new PianoKey();
                pianoKey.Top = 0;
                pianoKey.Left = i * 24;
                pianoKey.Key = (Key)(i + 30);
                pianoKey.Click += new EventHandler<PianoKeyEventArgs>(pianoKey_Click);

                pianoContainer.Children.Add(pianoKey);
            }
        }

        void pianoKey_Click(object sender, PianoKeyEventArgs e)
        {
            // 钢琴键被单击后，触发键盘单击事件
            OnClick(e.Key);

            // 钢琴键被单击后，调用相应的方法
            Play(e.Key);
        }      

        /// <summary>
        /// 指定的键值被敲击后所执行的方法
        /// </summary>
        /// <param name="key">键值</param>
        public void Play(Key key)
        {
            if (key >= Key.A && key <= Key.Z)
            {
                // 提示该钢琴键被敲击了
                var pianoKey = pianoContainer.Children[(int)key - 30] as PianoKey;
                pianoKey.Play();

                // 播放该钢琴键所对应的音阶
                scalePlayer.Play(key);
            }
        }

        // 钢琴键盘被敲击的事件
        public event EventHandler<PianoKeyEventArgs> Click;
        public void OnClick(Key key)
        {
            if (Click != null)
                Click(this, new PianoKeyEventArgs() { Key = key });
        }
    }
}
