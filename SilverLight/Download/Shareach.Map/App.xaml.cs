/********************************************************************************
 * 地图层, 用于地图操作
 * 作者: Shareach
 * Email:yinpengxiang@hotmail.com
 * 网址: http://sps.shareach.com
 * 欢迎访问 http://www.fangtoo.com , Flex版的地图,
 * 这个是Silverlight 2 Bate 2 版本编译开发的, 此代码仅仅作为技术交流使用, 
 * 请不要能够用于商业用途, 著名出处, 欢迎讨论指教
 * 还有很多东西不会用, 刚看了半天就开干的, 纯粹使用C# 的知识 和 SDK文档
 * *............................................................................*/
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

namespace Shareach.Map
{
    public partial class App : Application
    {

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new Page();
        }

        private void Application_Exit(object sender, EventArgs e)
        {

        }
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if (!System.Diagnostics.Debugger.IsAttached)
            {

                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;

                try
                {
                    string errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                    errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                    System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight 2 Application " + errorMsg + "\");");
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
