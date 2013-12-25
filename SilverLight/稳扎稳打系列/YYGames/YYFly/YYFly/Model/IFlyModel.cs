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

namespace YYFly.Model
{
    public interface IFlyModel
    {
        /// <summary>
        /// 让一只苍蝇开始飞
        /// </summary>
        void Start();
        /// <summary>
        /// 让一只苍蝇停止飞
        /// </summary>
        void Stop();
        /// <summary>
        /// 计分方法
        /// </summary>
        void Score();

        /// <summary>
        /// 苍蝇改变位置所触发的事件
        /// </summary>
        event EventHandler<FlyEventArgs> FlyChanging;
    }
}
