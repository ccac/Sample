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
    public partial class PianoKey : UserControl
    {
        public PianoKey()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 键值 A - Z，显示在钢琴键上，用于标记
        /// </summary>
        public Key Key
        {
            get { return Convert.ToChar(key.Text).ToKey(); }
            set { key.Text = value.ToChar().ToString(); }
        }

        /// <summary>
        /// 钢琴键相对父 Canvas 的 Y 方向上距离，左上角为原点
        /// </summary>
        public double Top
        {
            set { canvas.SetValue(Canvas.TopProperty, value); }
        }

        /// <summary>
        /// 钢琴键相对父 Canvas 的 X 方向上距离，左上角为原点
        /// </summary>
        public double Left
        {
            set { canvas.SetValue(Canvas.LeftProperty, value); }
        }

        /// <summary>
        /// 播放动画，用于提示该钢琴键被敲击了
        /// </summary>
        public void Play ()
        {
            ani.Begin();
        }

        private void canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 左键单击该钢琴键
            OnClick();
        }

        /// <summary>
        /// 该钢琴键被敲击的事件
        /// </summary>
        public event EventHandler<PianoKeyEventArgs> Click;
        public void OnClick()
        {
            if (Click != null)
                Click(this, new PianoKeyEventArgs() { Key = this.Key });
        }
    }
}
