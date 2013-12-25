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

using System.Windows.Browser;

namespace Silverlight20.Tip
{
    public partial class RightClick : UserControl
    {
        public RightClick()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(RightClick_Loaded);
        }

        void RightClick_Loaded(object sender, RoutedEventArgs e)
        {
            // 监听页面的 oncontextmenu 事件，并处理
            // 注：如果要监听 oncontextmenu 事件，需要将 Silverlight 程序的 Windowless 属性设置为 true
            HtmlPage.Document.AttachEvent("oncontextmenu", this.OnContextMenu);
        }

        private void OnContextMenu(object sender, HtmlEventArgs e) 
        {
            // 设置右键菜单出现的位置
            tt.X = e.OffsetX - 201;
            tt.Y = e.OffsetY - 30;

            // 禁止其他 DOM 响应该事件（屏蔽掉默认的右键菜单）
            // 相当于 event.returnValue = false;
            e.PreventDefault();
        }
    }
}
