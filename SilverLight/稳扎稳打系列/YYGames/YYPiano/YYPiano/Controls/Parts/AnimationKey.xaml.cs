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
    public partial class AnimationKey : UserControl
    {
        public AnimationKey()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 键值 A - Z，动画显示，用于提示用户应该敲什么键
        /// </summary>
        public Key Key
        {
            get { return Convert.ToChar(key.Text).ToKey(); }
            set { key.Text = value.ToChar().ToString(); }
        }

        /// <summary>
        /// 开始动画
        /// </summary>
        public void Start()
        {
            mainAni.Begin();
        }

        /// <summary>
        /// 停止动画
        /// </summary>
        public void Stop()
        {
            mainAni.Stop();
            insideAni.Stop();
            outsideAni.Stop();
        }

        /// <summary>
        /// 动画开始时间
        /// </summary>
        public TimeSpan BeginTime
        {
            set { mainAni.BeginTime = value; }
        }

        private int _targetIndex;
        /// <summary>
        /// UI 上设置了 3 个目标区，设置键的动画最终要落到哪个区上
        /// </summary>
        public int TargetIndex
        {
            set
            {
                if (value == 0)
                    targetX.To = -120;
                else if (value == 1)
                    targetX.To = 0;
                else if (value == 2)
                    targetX.To = 120;
                else
                    targetX.To = 0;

                _targetIndex = value;
            }
        }

        /// <summary>
        /// 主动画完成后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainAni_Completed(object sender, EventArgs e)
        {
            if (_targetIndex == 0)
                target.Fill = new SolidColorBrush(Colors.Orange);
            else if (_targetIndex == 1)
                target2.Fill = new SolidColorBrush(Colors.Orange);
            else if (_targetIndex == 2)
                target3.Fill = new SolidColorBrush(Colors.Orange);

            insideAni.Begin();

            OnInside();
        }

        /// <summary>
        /// 目标区动画完成后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void insideAni_Completed(object sender, EventArgs e)
        {
            if (_targetIndex == 0)
                target.Fill = new SolidColorBrush(Colors.Transparent);
            else if (_targetIndex == 1)
                target2.Fill = new SolidColorBrush(Colors.Transparent);
            else if (_targetIndex == 2)
                target3.Fill = new SolidColorBrush(Colors.Transparent);

            outsideAni.Begin();

            OnOutside();
        }

        /// <summary>
        /// 动画进入目标区后的事件
        /// </summary>
        public event EventHandler<PianoKeyEventArgs> Inside;
        public void OnInside()
        {
            if (Inside != null)
            {
                Inside(this, new PianoKeyEventArgs() { Key = this.Key });
            }
        }

        /// <summary>
        /// 动画离开目标区后的事件
        /// </summary>
        public event EventHandler<PianoKeyEventArgs> Outside;
        public void OnOutside()
        {
            if (Outside != null)
            {
                Outside(this, new PianoKeyEventArgs() { Key = this.Key });
            }
        }
    }
}
