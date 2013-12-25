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
    public partial class GameTip : UserControl
    {
        public GameTip()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            OnStart();
        }

        /// <summary>
        /// 用于开始游戏的按钮上所显示的文本
        /// </summary>
        public string ButtonText
        {
            get
            {
                return btnStart.Content.ToString();
            }

            set
            {
                btnStart.Content = value;
            }
        }

        /// <summary>
        /// 游戏开始事件
        /// </summary>
        public event EventHandler Start;
        public void OnStart()
        {
            if (Start != null)
                Start(this, EventArgs.Empty);
        }
    }
}
