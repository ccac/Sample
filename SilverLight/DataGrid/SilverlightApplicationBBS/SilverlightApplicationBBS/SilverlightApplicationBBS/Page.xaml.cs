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

namespace SilverlightApplicationBBS
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            // 造些假数据。项目中数据来自数据库
            List<Message> msgList = new List<Message>();
            for (int i = 0; i < 30; i++)
            {
                Message msg = new Message()
                {
                    Title = "Message Title " + i.ToString(),
                    OpenedBy = (i % 2 == 0) ? "Tom" : "Tim",
                    OpenTime = DateTime.Now.ToShortDateString(),
                    Content = (i % 2 == 0) ? "水之真谛" : @"http://blog.csdn.net/FantasiaX"
                };
                msgList.Add(msg);
            }

            this.listBox.ItemsSource = msgList;
        }

        // 切换视图
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Silverlight里的VisualTreeHelper功能受限，所以只能这样做。WPF里的就方便多了。
            Button b = sender as Button;
            StackPanel p = VisualTreeHelper.GetParent(b) as StackPanel;
            p = VisualTreeHelper.GetParent(p) as StackPanel;
            p = p.FindName("detailPanel") as StackPanel;
            if (p.Visibility == Visibility.Collapsed)
            {
                p.Visibility = Visibility.Visible;
            }
            else
            {
                p.Visibility = Visibility.Collapsed;
            }
        }
    }

    public class Message
    {
        public string Title { get; set; }
        public string OpenedBy { get; set; }
        public string OpenTime { get; set; }
        public string Content { get; set; }
    }
}
