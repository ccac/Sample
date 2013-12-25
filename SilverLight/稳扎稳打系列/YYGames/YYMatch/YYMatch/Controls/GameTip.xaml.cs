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

namespace YYMatch.Controls
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
