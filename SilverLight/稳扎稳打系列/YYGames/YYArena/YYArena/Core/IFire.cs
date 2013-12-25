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

namespace YYArena.Core
{
    /// <summary>
    /// 开火接口
    /// </summary>
    public interface IFire
    {
        /// <summary>
        /// 上一次开火时间
        /// </summary>
        DateTime PrevFireDateTime { get; set; }

        /// <summary>
        /// 最小开火间隔
        /// </summary>
        double MinFireInterval { get; set; }

        /// <summary>
        /// 开火触发的事件
        /// </summary>
        event EventHandler<EventArgs> Fire;
    }
}
