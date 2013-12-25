/*
 * 如何响应鼠标滚轮事件，可以参看 Deep Zoom Composer 生成的 MouseWheelHelper.cs
 */

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
    public partial class Wheel : UserControl
    {
        public Wheel()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Wheel_Loaded);
        }

        void Wheel_Loaded(object sender, RoutedEventArgs e)
        {
            HtmlPage.Window.AttachEvent("DOMMouseScroll", OnMouseWheel);
            HtmlPage.Window.AttachEvent("onmousewheel", OnMouseWheel);
            HtmlPage.Document.AttachEvent("onmousewheel", OnMouseWheel);
        }

        private void OnMouseWheel(object sender, HtmlEventArgs args)
        {
            args.PreventDefault();

            double mouseDelta = 0;
            ScriptObject eventObj = args.EventObject;

            // Mozilla and Safari  
            if (eventObj.GetProperty("detail") != null)
            {
                mouseDelta = ((double)eventObj.GetProperty("detail"));
            }

            // IE and Opera   
            else if (eventObj.GetProperty("wheelDelta") != null)
            {
                mouseDelta = ((double)eventObj.GetProperty("wheelDelta"));
            }

            // IE浏览器：mouseDelta == 120 向上滚动；mouseDelta == -120 向下滚动
            // FF浏览器：mouseDelta == -3 向上滚动；mouseDelta == 3 向下滚动
            lblMsg.Text += mouseDelta.ToString();
        }
    }
}
