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

namespace Silverlight20.Data
{
    public partial class ListBox : UserControl
    {
        public ListBox()
        {
            InitializeComponent(); 
            
            BindData();
        }

        void BindData()
        {
            var source = new Data.SourceData();

            // 设置 ListBox 的数据源
            ListBox1.ItemsSource = source.GetData();
        }
    }
}
