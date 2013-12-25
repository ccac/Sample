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

namespace YYFly
{
    /// <summary>
    /// 苍蝇的 EventArgs
    /// </summary>
    public class FlyEventArgs : EventArgs
    {
        /// <summary>
        /// X 轴坐标
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y 轴坐标
        /// </summary>
        public double Y { get; set; }
        /// <summary>
        /// Z 轴缩放比例
        /// </summary>
        public double Z { get; set; }

        /// <summary>
        /// Z 轴上变换的方向。ture - 向外；false - 向内
        /// </summary>
        public bool Z_Out { get; set; }
    }
}
