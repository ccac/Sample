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

namespace YYArena.Controls
{
    /// <summary>
    /// 交互类型
    /// </summary>
    public enum InteractiveType
    {
        /// <summary>
        /// 开始游戏
        /// </summary>
        Start,
        /// <summary>
        /// 游戏升级
        /// </summary>
        LevelUp,
        /// <summary>
        /// 游戏结束
        /// </summary>
        Over,
        /// <summary>
        /// 游戏进行中
        /// </summary>
        Running
    }
}
