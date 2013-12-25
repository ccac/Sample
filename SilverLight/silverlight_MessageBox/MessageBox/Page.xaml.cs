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
using System.Diagnostics;

namespace MessageBox
{
    //http://silverlight.services.live.com/invoke/72193/messagebox/iframe.html
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();      
    }

      
    void OnHardCodeClick(object sender, EventArgs args)
    {
        HardCodeMessageBox.Show("硬编码模式消息框！");
    }

    void OnNormalClick(object sender, EventArgs args)
    {        
        MessageBox.ShowAsync("简单调用, 无回调, 无状态, 无样式！");
        //下面注释的代码包括状态和回调事件　
        //MessageBox.ShowAsync("As previously but with a callback - hit NO", (s, e) =>
        //  {
        //    Debug.Assert(e.Result == MessageBoxResult.No);
        //  });

        //MessageBox.ShowAsync("As previously but with state - hit YES", 101, (s, e) =>
        //  {
        //    Debug.Assert((e.Result == MessageBoxResult.Yes) && ((int)e.AsyncState == 101));
        //  });        
    }

    void OnShapeClick(object sender, EventArgs args)
    {
        MessageBox.ShowAsync(new Ellipse()
        {
            Width = 80,
            Height = 80,
            Fill = new SolidColorBrush(Colors.Green)
        });
    }  

    void OnChangeStyleClick(object sender, EventArgs args)
    {
        
        Style myStyle = this.Resources["myStyle"] as Style;

        MessageBox.ShowAsync("使用一个不同的样式",
            101,//状态
            (s, e) =>　//处理事件
            {
                if (e.Result == MessageBoxResult.No && ((int)e.AsyncState == 101))
                {
                    HtmlPage.Window.Alert("您点击了No按钮");
                }

                if (e.Result == MessageBoxResult.Yes)
                {
                    HtmlPage.Window.Alert("您点击了Yes按钮");
                }
            },
          myStyle);
    }
  }
}
