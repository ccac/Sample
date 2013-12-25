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
    public partial class DataGrid02 : UserControl
    {
        public DataGrid02()
        {
            InitializeComponent();

            BindData();
        }

        void BindData()
        {
            var source = new Data.SourceData();

            // 设置 DataGrid 的数据源
            DataGrid1.DataContext = source.GetData();
        }

        private void chkReadOnly_Changed(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;

            // IsReadOnly - 单元格是否只读。默认值 false
            DataGrid1.IsReadOnly = (bool)chk.IsChecked;
        }

        private void chkFreezeColumn_Changed(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;

            // FrozenColumnCount - 表格所冻结的列的总数（从左边开始数）。默认值 0
            if (chk.IsChecked == true)
                DataGrid1.FrozenColumnCount = 1;
            else if (chk.IsChecked == false)
                DataGrid1.FrozenColumnCount = 0;
        }
       
        private void chkSelectionMode_Changed(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;

            // SelectionMode - 行的选中模式 [System.Windows.Controls.DataGridSelectionMode枚举]
            //     DataGridSelectionMode.Single - 只能单选
            //     DataGridSelectionMode.Extended - 可以多选（通过Ctrl或Shift的配合）。默认值
            if (chk.IsChecked == true)
                DataGrid1.SelectionMode = DataGridSelectionMode.Single;
            else if (chk.IsChecked == false)
                DataGrid1.SelectionMode = DataGridSelectionMode.Extended;
        }

        private void chkColReorder_Changed(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;

            // CanUserReorderColumns - 是否允许拖动列。默认值 true
            if (DataGrid1 != null)
                DataGrid1.CanUserReorderColumns = (bool)chk.IsChecked;
        }

        private void chkColResize_Changed(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;

            // CanUserResizeColumns - 是否允许改变列的宽度。默认值 true
            if (DataGrid1 != null)
                DataGrid1.CanUserResizeColumns = (bool)chk.IsChecked;
        }

        private void chkColSort_Changed(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;

            // CanUserSortColumns - 是否允许列的排序。默认值 true
            if (DataGrid1 != null)
                DataGrid1.CanUserSortColumns = (bool)chk.IsChecked;
        }

        private void chkCustomGridLineVertical_Changed(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;

            if (DataGrid1 != null)
            {
                // VerticalGridLinesBrush - 改变表格的垂直分隔线的 Brush
                if (chk.IsChecked == true)
                    DataGrid1.VerticalGridLinesBrush = new SolidColorBrush(Colors.Blue);
                else
                    DataGrid1.VerticalGridLinesBrush = new SolidColorBrush(Color.FromArgb(255, 223, 227, 230));
            }
        }

        private void chkCustomGridLineHorizontal_Changed(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;

            if (DataGrid1 != null)
            {
                // HorizontalGridLinesBrush - 改变表格的水平分隔线的 Brush
                if (chk.IsChecked == true)
                    DataGrid1.HorizontalGridLinesBrush = new SolidColorBrush(Colors.Blue);
                else
                    DataGrid1.HorizontalGridLinesBrush = new SolidColorBrush(Color.FromArgb(255, 223, 227, 230));
            }
        }

        private void cboHeaders_SelectionChanged(object sender, RoutedEventArgs e)
        {
            ComboBoxItem cbi = ((ComboBox)sender).SelectedItem as ComboBoxItem;

            if (DataGrid1 != null)
            {
                // HeadersVisibility - 表头（包括列头和行头）的显示方式 [System.Windows.Controls.DataGridHeadersVisibility枚举]
                //     DataGridHeadersVisibility.All - 列头和行头均显示
                //     DataGridHeadersVisibility.Column - 只显示列头。默认值
                //     DataGridHeadersVisibility.Row - 只显示行头
                //     DataGridHeadersVisibility.None - 列头和行头均不显示
                if (cbi.Tag.ToString() == "All")
                    DataGrid1.HeadersVisibility = DataGridHeadersVisibility.All;
                else if (cbi.Tag.ToString() == "Column")
                    DataGrid1.HeadersVisibility = DataGridHeadersVisibility.Column;
                else if (cbi.Tag.ToString() == "Row")
                    DataGrid1.HeadersVisibility = DataGridHeadersVisibility.Row;
                else if (cbi.Tag.ToString() == "None")
                    DataGrid1.HeadersVisibility = DataGridHeadersVisibility.None;
            }
        }
    }
}
