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

namespace YYMatch.Models
{
    /// <summary>
    /// 卡片点实体
    /// </summary>
    public class CardPoint
    {
        /// <summary>
        /// X 方向上的索引
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y 方向上的索引
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// 在整个卡片集合中的索引
        /// </summary>
        public int Position { get; set; }

        public CardPoint(int x, int y)
        {
            X = x;
            Y = y;
            Position = y * Global.ContainerColumns + x;
        }

        public CardPoint(int position)
        {
            Position = position;
            X = position % Global.ContainerColumns;
            Y = (position - X) / Global.ContainerColumns;
        }
    }
}
