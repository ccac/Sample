using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SilverlightDemoDataBinding
{
    public partial class BindList : UserControl
    {
        public BindList()
        {
            InitializeComponent();

            Blog blog = new Blog();
            blog.Posts = new List<string>
            {
                "一步一步学Silverlight 2系列（10）：使用用户控件",
                "一步一步学Silverlight 2系列（9）：使用控件模板",
                "一步一步学Silverlight 2系列（8）：使用样式封装控件观感",
                "一步一步学Silverlight 2系列（7）：全屏模式支持"
            };

            PostList.DataContext = blog;
        }
    }

    public class Blog
    {
        public List<string> Posts { get; set; }
    }
}
