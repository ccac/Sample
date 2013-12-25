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

namespace Silverlight20.Thread
{
    public partial class Thread : UserControl
    {
        string result = "";

        public Thread()
        {
            InitializeComponent();

            Demo();
        }

        void Demo()
        {
            /*
             * Thread - 用于线程的创建和控制的类
             *     Name - 线程名称
             *     IsBackground - 是否是后台线程（对于Silverlight来说，是否是后台线程没区别）
             *     Start(object parameter) - 启动后台线程
             *         object parameter - 为后台线程传递的参数
             *     IsAlive - 线程是否在执行中
             *     ManagedThreadId - 当前托管线程的唯一标识符
             *     ThreadState - 指定线程的状态 [System.Threading.ThreadState枚举]
             *     Abort() - 终止线程
             */

            // DoWork 是后台线程所执行的方法（此处省略掉了委托类型）
            // ThreadStart 委托不可以带参数, ParameterizedThreadStart 委托可以带参数
            System.Threading.Thread thread = new System.Threading.Thread(DoWork);
            thread.Name = "ThreadDemo";
            thread.IsBackground = true;
            thread.Start(1000);

            result += thread.IsAlive + "\r\n";
            result += thread.ManagedThreadId + "\r\n";
            result += thread.Name + "\r\n";
            result += thread.ThreadState + "\r\n";

            // thread.Join(); 阻塞调用线程（本例为主线程），直到指定线程（本例为thread）执行完毕为止

            // 阻塞调用线程（本例为主线程）
            // 如果指定线程执行完毕则继续（本例为thread执行完毕则继续）
            // 如果指定线程运行的时间超过指定时间则继续（本例为thread执行时间如果超过5秒则继续）
            // 返回值为在指定时间内指定线程是否执行完毕（本例中thread的执行时间为1秒，所以会返回true）
            if (thread.Join(5000)) 
            {
                result += "指定线程在5秒内执行完毕\r\n";
            }

            txtMsg.Text = result;
        }

        void DoWork(object sleepMillisecond)
        {
            System.Threading.Thread.Sleep((int)sleepMillisecond);

            result += "新开线程执行完毕\r\n";
        }
    }
}
