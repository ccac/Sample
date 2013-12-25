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

using System.Windows.Resources;

namespace Silverlight20.Tip
{
    public partial class CustomFont : UserControl
    {
        public CustomFont()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(CustomFont_Loaded);
        }

        void CustomFont_Loaded(object sender, RoutedEventArgs e)
        {
            // 以编码的方式使用自定义字体

            // 以华文行楷为例
            StreamResourceInfo sri = App.GetResourceStream(
              new Uri("/Silverlight20;component/Resource/STXINGKA.TTF", UriKind.Relative));

            // 设置需要显示的字体源
            lblMsg.FontSource = new FontSource(sri.Stream);

            // 设置需要显示的字体名称
            // STXingkai - 华文行楷的字体名称
            lblMsg.FontFamily = new FontFamily("STXingkai");
        }
    }
}
