using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MessageBox
{
    /// <summary>
    /// 消息框结果
    /// </summary>
    public enum MessageBoxResult
    {
        Yes,　　//是
        No,　　 //否
        Cancel　//取消
    }

    /// <summary>
    /// 消息事件参数
    /// </summary>
    public class MessageBoxResultEventArgs : EventArgs
    {
        public MessageBoxResult Result { get; set; }
        public object AsyncState { get; set; }
    }

    /// <summary>
    /// 消息框控件类，该模板包括三个组件(三个Button和一个Panel)
    /// </summary>
    [TemplatePart(Name = RootElement, Type = typeof(Panel))]
    [TemplatePart(Name = YesButtonElement, Type = typeof(Button))]
    [TemplatePart(Name = NoButtonElement, Type = (typeof(Button)))]
    [TemplatePart(Name = CancelButtonElement, Type = (typeof(Button)))]
    public class MessageBoxControl : ContentControl
    {
        public event EventHandler<MessageBoxResultEventArgs> MessageBoxDismissed;

        public MessageBoxControl()
        {
            DefaultStyleKey = typeof(MessageBoxControl);
        }
        public override void OnApplyTemplate()
        {
            #region 取消之前的事件绑定

            if (yesButton != null)
            {
                yesButton.Click -= OnYesButton;
            }
            if (noButton != null)
            {
                noButton.Click -= OnNoButton;
            }
            if (cancelButton != null)
            {
                cancelButton.Click -= OnCancelButton;
            }

            #endregion

            rootElement = base.GetTemplateChild(RootElement) as Panel;
            yesButton = base.GetTemplateChild(YesButtonElement) as Button;
            noButton = base.GetTemplateChild(NoButtonElement) as Button;
            cancelButton = base.GetTemplateChild(CancelButtonElement) as Button;

            #region 如果grid中有相应元素时，则绑定相应事件（详见下面的代码）

            if (yesButton != null)
            {
                yesButton.Click += OnYesButton;
            }
            if (noButton != null)
            {
                noButton.Click += OnNoButton;
            }
            if (cancelButton != null)
            {
                cancelButton.Click += OnCancelButton;
            }

            #endregion
        }

        void OnYesButton(object sender, EventArgs args)
        {
            FireDismissed(MessageBoxResult.Yes);
        }
        void OnNoButton(object sender, EventArgs args)
        {
            FireDismissed(MessageBoxResult.No);
        }
        void OnCancelButton(object sender, EventArgs args)
        {
            FireDismissed(MessageBoxResult.Cancel);
        }

        /// <summary>
        /// 调用绑定的事件，并传递相应参数
        /// </summary>
        /// <param name="result"></param>
        void FireDismissed(MessageBoxResult result)
        {
            //当绑定的事件不为空时...(绑定部分参见MessageBox的构造函数)
            if (MessageBoxDismissed != null)
            {
                MessageBoxDismissed(this, new MessageBoxResultEventArgs() { Result = result });
            }
        }

        Button yesButton;
        Button noButton;
        Button cancelButton;
        Panel rootElement;

        #region 赋值信息参见generic.xaml中的"x:Name"声明

        public const string RootElement = "RootElement";
        public const string YesButtonElement = "YesButtonElement";
        public const string NoButtonElement = "NoButtonElement";
        public const string CancelButtonElement = "CancelButtonElement";

        #endregion
    }
}