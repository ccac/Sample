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
    public partial class DataGrid01 : UserControl
    {
        public DataGrid01()
        {
            InitializeComponent();

            BindData();
        }

        void BindData()
        {
            var source = new Data.SourceData();

            // 设置 DataGrid 的数据源
            DataGrid1.ItemsSource = source.GetData();
        }

        private void chkFreezeRowDetails_Changed(object sender, RoutedEventArgs e)
        {
            // AreRowDetailsFrozen - 是否冻结 RowDetailsTemplate 。 默认值为 false
            //     如果等于 true ，那么在 DataGrid 的水平滚动条滚动的时候 RowDetailsTemplate 不会跟着滚动

            CheckBox chk = sender as CheckBox;

            if (DataGrid1 != null)
                DataGrid1.AreRowDetailsFrozen = (bool)chk.IsChecked;
        }
    }
}
