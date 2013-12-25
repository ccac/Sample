/* 用于游戏的倒计时的控件 */

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
    public partial class Gun : UserControl
    {
        public Gun()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 开火。缩小瞄准镜以提示开火
        /// </summary>
        public void Fire()
        {
            st.ScaleX = st.ScaleY = 0.8;
        }

        /// <summary>
        /// 瞄准镜复位
        /// </summary>
        public void Reset()
        {
            st.ScaleX = st.ScaleY = 1;
        }
    }
}
