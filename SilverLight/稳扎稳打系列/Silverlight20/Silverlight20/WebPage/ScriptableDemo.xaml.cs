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

namespace Silverlight20.WebPage
{
    public partial class ScriptableDemo : UserControl
    {
        System.Threading.Timer _timer;
        System.Threading.SynchronizationContext _syncContext;

        public ScriptableDemo()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(ScriptableDemo_Loaded);
        }

        void ScriptableDemo_Loaded(object sender, RoutedEventArgs e)
        {
            // UI 线程
            _syncContext = System.Threading.SynchronizationContext.Current;

            Scriptable s = new Scriptable() { CurrentTime = DateTime.Now };

            // 将 Scriptable 对象注册到客户端中，所对应的客户端的对象名为 scriptable
            HtmlPage.RegisterScriptableObject("scriptable", s);

            _timer = new System.Threading.Timer(MyTimerCallback, s, 0, 1000);
        }

        private void MyTimerCallback(object state)
        {
            Scriptable s = state as Scriptable;

            // 每秒调用一次 UI 线程上的指定的方法
            _syncContext.Post(OnStart, s);
        }

        void OnStart(object state)
        {
            Scriptable s = state as Scriptable;

            // 调用 Scriptable 对象的 OnStart() 方法，以触发 Start 事件
            s.OnStart(DateTime.Now);
        }
    }
}
