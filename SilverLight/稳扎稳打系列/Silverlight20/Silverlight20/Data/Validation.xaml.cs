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

using System.Text.RegularExpressions;

namespace Silverlight20.Data
{
    public partial class Validation : UserControl
    {
        public Validation()
        {
            InitializeComponent();
        }

        private void StackPanel_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
            // ValidationErrorEventArgs - 用于提供 BindingValidationError 事件的一些信息
            //     ValidationErrorEventArgs.Action - 验证状态
            //     ValidationErrorEventArgs.Error - 触发 BindingValidationError 事件的错误信息
            //     ValidationErrorEventArgs.Handled - 标记该路由事件是否已被处理
            // ValidationErrorEventAction.Added - 因为出现验证错误而触发此事件
            // ValidationErrorEventAction.Removed - 因为解决上次验证错误而触发此事件

            if (e.Action == ValidationErrorEventAction.Added)
            {
                textBox.Background = new SolidColorBrush(Colors.Red);
                textBox.Text = e.Error.Exception.Message;
            }
            else if (e.Action == ValidationErrorEventAction.Removed)
            {
                textBox.Background = new SolidColorBrush(Colors.White);
            }
        }
    }

    /// <summary>
    /// 验证类。验证是否为正整数
    /// </summary>
    public class MyValidation
    {
        private string _count;
        public string Count
        {
            get { return _count; }
            set
            {
                if (!Regex.IsMatch(value, @"^\d+$"))
                {
                    // 绑定引擎可以报告由属性的 setter 抛出的异常，也可以报告由转换器（IValueConverter）抛出的异常
                    throw new Exception("必须是正整数");
                }

                _count = value;
            }
        }
    }
}
