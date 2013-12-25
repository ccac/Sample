/*
 * 演示用 BackgroundWorker 在后台线程上执行耗时的操作
 * 按“开始”键，开始在后台线程执行耗时操作，并向UI线程汇报执行进度
 * 按“取消”键，终止后台线程
 * BackgroundWorker 调用 UI 线程时会自动做线程同步
 */

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
    public partial class BackgroundWorker : UserControl
    {
        // BackgroundWorker - 用于在单独的线程上运行操作。例如可以在非UI线程上运行耗时操作，以避免UI停止响应
        System.ComponentModel.BackgroundWorker _backgroundWorker;

        public BackgroundWorker()
        {
            InitializeComponent();

            BackgroundWorkerDemo();
        }

        void BackgroundWorkerDemo()
        {
            /*
             * WorkerSupportsCancellation - 是否支持在其他线程中取消该线程的操作
             * WorkerReportsProgress - 是否可以报告操作进度
             * ProgressChanged - 报告操作进度时触发的事件
             * DoWork - BackgroundWorker 调用 RunWorkerAsync() 方法时触发的事件。在此执行具体操作
             * RunWorkerCompleted - 操作完成/取消/出错时触发的事件
             */

            _backgroundWorker = new System.ComponentModel.BackgroundWorker();

            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.WorkerReportsProgress = true;

            _backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(_backgroundWorker_ProgressChanged);
            _backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(_backgroundWorker_DoWork);
            _backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(_backgroundWorker_RunWorkerCompleted);
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            // IsBusy - 指定的 BackgroundWorker 是否正在后台操作
            // RunWorkerAsync(object argument) - 开始在后台线程执行指定的操作
            //     object argument - 需要传递到 DoWork 的参数
            if (!_backgroundWorker.IsBusy)
                _backgroundWorker.RunWorkerAsync("需要传递的参数");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // CancelAsync() - 取消 BackgroundWorker 正在执行的后台操作
            if (_backgroundWorker.WorkerSupportsCancellation)
                _backgroundWorker.CancelAsync();
        }

        void _backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            /*
             * DoWorkEventArgs.Argument - RunWorkerAsync(object argument)传递过来的参数
             * DoWorkEventArgs.Cancel - 取消操作
             * DoWorkEventArgs.Result - 操作的结果。将传递到 RunWorkerCompleted 所指定的方法
             * BackgroundWorker.ReportProgress(int percentProgress, object userState) - 向 ProgressChanged 汇报操作的完成进度
             *     int percentProgress - 操作完成的百分比 1% - 100%
             *     object userState - 传递到 ProgressChanged 的参数
             */

            for (int i = 0; i < 10; i++)
            {
                if ((_backgroundWorker.CancellationPending == true))
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                    _backgroundWorker.ReportProgress((i + 1) * 10, i);
                }
            }

            e.Result = "操作已完成";
        }

        void _backgroundWorker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            // ProgressChangedEventArgs.ProgressPercentage - ReportProgress 传递过来的操作完成的百分比
            // ProgressChangedEventArgs.UserState - ReportProgress 传递过来的参数
            txtProgress.Text = string.Format("完成进度：{0}%；参数：{1}",
                e.ProgressPercentage,
                e.UserState);
        }

        void _backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            /*
             * RunWorkerCompletedEventArgs.Error - DoWork 时产生的错误
             * RunWorkerCompletedEventArgs.Cancelled - 后台操作是否已被取消
             * RunWorkerCompletedEventArgs.Result - DoWork 的结果
             */

            if (e.Error != null)
            {
                txtMsg.Text += e.Error.ToString() + "\r\n";
            }
            else if (e.Cancelled)
            {
                txtMsg.Text += "操作被取消\r\n";
            }
            else
            {
                txtMsg.Text += e.Result.ToString() + "\r\n";
            }
        }
    }
}
