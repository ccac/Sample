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
    /// 正弦波实体类
    /// </summary>
    public class SineWave
    {
        public double A { get; set; }
        public double Omega { get; set; }
        public double Phi { get; set; }

        // Y 轴方向上的偏移量
        public double OffsetY { get; set; }
    }
}
