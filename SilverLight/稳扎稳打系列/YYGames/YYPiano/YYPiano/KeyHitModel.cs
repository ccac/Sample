using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace YYPiano.Controls
{
    /// <summary>
    /// 按键/是否被敲击 实体类
    /// </summary>
    public class KeyHitModel
    {
        /// <summary>
        /// 按键
        /// </summary>
        public Key Key { get; set; }

        /// <summary>
        /// 是否被敲击
        /// </summary>
        public bool Hit { get; set; }
    }
}
