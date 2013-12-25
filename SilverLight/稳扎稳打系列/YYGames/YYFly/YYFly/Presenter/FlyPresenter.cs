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

using YYFly.Model;
using YYFly.View;

namespace YYFly.Presenter
{
    /// <summary>
    /// Model - View - Presenter 模式中的 Presenter
    /// </summary>
    public class FlyPresenter
    {
        public FlyPresenter(IFlyView flyView, IFlyModel flyModel)
        {
            FlyView = flyView;
            FlyModel = flyModel;

            FlyView.Start += new EventHandler(FlyView_Start);
            FlyView.Stop += new EventHandler(FlyView_Stop);
            flyView.Score += new EventHandler(flyView_Score);

            FlyModel.FlyChanging += new EventHandler<FlyEventArgs>(FlyModel_FlyChanging);
        }

        void FlyView_Start(object sender, EventArgs e)
        {
            FlyModel.Start();
        }

        void FlyView_Stop(object sender, EventArgs e)
        {
            FlyModel.Stop();
        }

        void flyView_Score(object sender, EventArgs e)
        {
            FlyModel.Score();
        }

        void FlyModel_FlyChanging(object sender, FlyEventArgs e)
        {
            FlyView.Update(e.X, e.Y, e.Z);
        }

        public IFlyView FlyView { get; set; }
        public IFlyModel FlyModel { get; set; }
    }
}
