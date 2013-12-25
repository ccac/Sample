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

namespace Silverlight20.Control
{
    public partial class Calendar : UserControl
    {
        public Calendar()
        {
            InitializeComponent();
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            // Calendar.SelectedDate - 选中的日期
            // Calendar.SelectedDates - 选中的多个日期集合

            this.txtMsg.Text = "";
            foreach (DateTime dt in calendar.SelectedDates)
            {
                this.txtMsg.Text += dt.ToString("yyyy-MM-dd");
                this.txtMsg.Text += " ";
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (this.calendar.SelectedDate != null && this.calendar.SelectedDate >= DateTime.Now.Date)
                this.calendar.SelectedDate = DateTime.Now;

            // Calendar.BlackoutDates - 不允许选择的日期集合
            // Calendar.BlackoutDates.AddDatesInPast() - 禁止选择今天之前的日期
            this.calendar.BlackoutDates.AddDatesInPast();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Calendar.BlackoutDates.Clear() - 清除 不允许选择的日期集合 的设置
            this.calendar.BlackoutDates.Clear();
        }

        private void selectionMode_Changed(object sender, RoutedEventArgs e)
        {
            // CalendarSelectionMode.None - 禁止选择日期
            // CalendarSelectionMode.SingleRange - 可以选择多个日期，连续日期（Shift键配合）
            // CalendarSelectionMode.MultipleRange - 可以选择多个日期，任意日期（Ctrl键配合）
            // CalendarSelectionMode.SingleDate - 只能选择一个日期
            switch (((System.Windows.Controls.RadioButton)sender).Name)
            {
                case "None":
                    this.calendar.SelectionMode = CalendarSelectionMode.None;
                    break;
                case "SingleRange":
                    this.calendar.SelectionMode = CalendarSelectionMode.SingleRange;
                    break;
                case "MultipleRange":
                    this.calendar.SelectionMode = CalendarSelectionMode.MultipleRange;
                    break;
                default:
                    this.calendar.SelectionMode = CalendarSelectionMode.SingleDate;
                    break;
            }
        }
    }
}
