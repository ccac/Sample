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
    /// 乐谱菜单实体类
    /// </summary>
    public class MenuModel
    {
        /// <summary>
        /// 乐谱名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 乐谱编码
        /// </summary>
        public string Code { get; set; }
    }
}
