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
using System.Windows.Markup;
using System.Resources;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using System.Threading;

namespace MessageBox
{
    public class UserControlContentAccessor : UserControl
    {
        /// <summary>
        /// 获取当前UserControl的ContentProperty属性
        /// </summary>
        /// <param name="uc">当前UserControl</param>
        /// <returns>ContentProperty属性</returns>
        public static UIElement GetContent(UserControl uc)
        {
            return ((UIElement)uc.GetValue(UserControl.ContentProperty));
        }
        /// <summary>
        /// 设置当前UserControl的ContentProperty属性
        /// </summary>
        /// <param name="uc">当前UserControl</param>
        /// <param name="element">要设置的内容属性</param>
        public static void SetContent(UserControl uc, UIElement element)
        {
            uc.SetValue(UserControl.ContentProperty, element);
        }
    }

    /// <summary>
    /// 消息框类，该类可以看成是对"消息框控件类"使用封装(封装了事件绑定和内容信息)
    /// </summary>
    public static class MessageBox
    {

        /// <summary>
        /// 实际页面视图中的元素（用于当消息框关闭后，还原页面元素时使用）
        /// </summary>
        private static UIElement realVisual;
        /// <summary>
        /// 用于绑定当前页面中根元素节点
        /// </summary>
        private static Grid parentGrid;
        /// <summary>
        /// 状态值
        /// </summary>
        private static object asyncState;
        /// <summary>
        /// 用户绑定回调事件属性
        /// </summary>
        private static EventHandler<MessageBoxResultEventArgs> userCallback;

        public static void ShowAsync(object content)
        {
            ShowAsync(content, null);
        }

        public static void ShowAsync(object content,
          EventHandler<MessageBoxResultEventArgs> callback)
        {
            ShowAsync(content, null, callback);
        }

        public static void ShowAsync(object content, object userState,
          EventHandler<MessageBoxResultEventArgs> callback)
        {
            ShowAsync(content, userState, callback, null);
        }

        public static void ShowAsync(object content, object userState,
          EventHandler<MessageBoxResultEventArgs> callback, Style controlTemplate)
        {
            MessageBoxControl control = new MessageBoxControl();
            control.Content = content;
            //绑定指定样式
            if (controlTemplate != null)
            {
                control.Style = controlTemplate;
            }
            ShowAsync(control, userState, callback);
        }
        public static void ShowAsync(MessageBoxControl control, object userState,
          EventHandler<MessageBoxResultEventArgs> callback)
        {
            UserControl uc = Application.Current.RootVisual as UserControl;

            if (uc != null)
            {
                asyncState = userState;//用户状态绑定
                userCallback = callback;//回调方法
                realVisual = UserControlContentAccessor.GetContent(uc);
                realVisual.IsHitTestVisible = false;　//使底层控件点击不可见

                parentGrid = new Grid();//声明一个Grid对象，用于加载新的内容
                UserControlContentAccessor.SetContent(uc, parentGrid);
                
                parentGrid.Children.Add(realVisual); //加载realVisual内容(注:此处内容中的控制已不支持点击了)
                parentGrid.Children.Add(control);　//加载消息框实例,后加载的显示在上(前)面

                control.MessageBoxDismissed += OnDismissed; //绑定要处理的事件，该事件会在点击消息框中的"yes"或"no"按钮时执行
            }
        }
        static void OnDismissed(object sender, MessageBoxResultEventArgs e)
        {

            MessageBoxControl control = sender as MessageBoxControl;

            UserControl uc = Application.Current.RootVisual as UserControl;

            if (uc != null)
            {　 //清除之前的页面UI元素，并还原页面初始时的元素设置
                parentGrid.Children.Clear();
                realVisual.IsHitTestVisible = true;
                UserControlContentAccessor.SetContent(uc, realVisual);
            }
            if (control != null)
            {
                control.MessageBoxDismissed -= OnDismissed;
            }
            try
            {
                if (userCallback != null)
                {
                    //执行用户绑定的事件（并传递事件参数）
                    userCallback(null, new MessageBoxResultEventArgs()
                    {
                        Result = e.Result,
                        AsyncState = asyncState
                    });
                }
            }
            finally
            {
                realVisual = null;
                parentGrid = null;
                asyncState = null;
                userCallback = null;
            }
        }

    
    }
}