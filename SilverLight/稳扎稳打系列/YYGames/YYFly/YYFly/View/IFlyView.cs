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

namespace YYFly.View
{
    public interface IFlyView
    {
        /// <summary>
        /// 更新苍蝇的位置
        /// </summary>
        /// <param name="x">X 轴坐标</param>
        /// <param name="y">Y 轴坐标</param>
        /// <param name="z">Z 轴方向上的缩放比例</param>
        void Update(double x, double y, double z);

        /// <summary>
        /// 苍蝇开始飞的事件处理器
        /// </summary>
        event EventHandler Start;
        /// <summary>
        /// 苍蝇停止飞的事件处理器
        /// </summary>
        event EventHandler Stop;
        /// <summary>
        /// 打死苍蝇后的计分事件
        /// </summary>
        event EventHandler Score;
    }
}
