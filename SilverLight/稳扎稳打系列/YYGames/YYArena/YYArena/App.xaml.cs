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

using YYArena.Core;
using YYArena.Controls;

using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Mathematics;

namespace YYArena
{
    public partial class App : Application
    {
        public static PhysicsSimulator PhysicsSimulator { get; private set; }

        public static KeyboardHandler KeyboardHandler { get; private set; }

        /// <summary>
        /// 场景宽
        /// </summary>
        public static double SceneWidth { get; private set; }
        /// <summary>
        /// 场景高
        /// </summary>
        public static double SceneHeight { get; private set; }
        
        /// <summary>
        /// 可视区宽
        /// </summary>
        public static double AreaWidth { get; private set; }
        /// <summary>
        /// 可视区高
        /// </summary>
        public static double AreaHeight { get; private set; }

        /// <summary>
        /// 主角向上到此偏移后，移动可视区
        /// </summary>
        public static double UpOffset { get; private set; }
        /// <summary>
        /// 主角向下到此偏移后，移动可视区
        /// </summary>
        public static double DownOffset { get; private set; }
        /// <summary>
        /// 主角向左到此偏移后，移动可视区
        /// </summary>
        public static double LeftOffset { get; private set; }
        /// <summary>
        /// 主角向右到此偏移后，移动可视区
        /// </summary>
        public static double RightOffset { get; private set; }

        public App()
        {
            this.Startup += this.Application_Startup;
            this.Exit += this.Application_Exit;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var main = new Page();
            this.RootVisual = main;

            // 初始化
            KeyboardHandler = new KeyboardHandler(main);
            KeyboardHandler.Attach();

            PhysicsSimulator = new PhysicsSimulator(new Vector2(0, 0));

            SceneWidth = 1600d;
            SceneHeight = 1200d;

            AreaWidth = 640d;
            AreaHeight = 480d;

            UpOffset = 160d;
            DownOffset = 160d;
            LeftOffset = 160d;
            RightOffset = 160d;

            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        private DateTime _prevDateTime = DateTime.Now;
        private double _leftoverSeconds = 0d; // 残余秒数
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            double seconds = (DateTime.Now - _prevDateTime).TotalSeconds + _leftoverSeconds;

            // 确保做平滑的物理计算
            // 避免在速度快的机器上，出现加速的情况
            while (seconds > .01)
            {
                PhysicsSimulator.Update(.01f);

                seconds -= .01;
            }

            _leftoverSeconds = seconds;
            _prevDateTime = DateTime.Now;           
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
                Deployment.Current.Dispatcher.BeginInvoke(delegate { ReportErrorToDOM(e); });
            }
        }
        private void ReportErrorToDOM(ApplicationUnhandledExceptionEventArgs e)
        {
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
