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

namespace YYMatch
{
    public partial class Page : UserControl
    {
        // 连击次数
        private int _hits = 0;

        public Page()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            game.Model.Removed += new EventHandler(Model_Removed);
            game.Model.Failed += new EventHandler(Model_Failed);
            game.Model.Finished += new EventHandler(Model_Finished);

            scoreboard.Replace += new EventHandler(scoreboard_Replace);

            tip.Start += new EventHandler(tip_Start);
        }

        void tip_Start(object sender, EventArgs e)
        {
            if (tip.btnStart.Content.ToString() == "重新游戏")
            {
                tip.btnStart.Content = "开始游戏";
                scoreboard.Model.Level = 1;
                scoreboard.Model.Score = 0;
                scoreboard.Model.Time = new TimeSpan(0, 0, 0);
            }

            game.Model.Start(scoreboard.Model.Level);
            scoreboard.Model.Start();
            tip.Visibility = Visibility.Collapsed;
            _hits = 0;
        }

        void scoreboard_Replace(object sender, EventArgs e)
        {
            game.Model.Replace();
            _hits = 0;
        }

        void Model_Finished(object sender, EventArgs e)
        {
            tip.Visibility = Visibility.Visible;
            scoreboard.Model.Pause();

            if (scoreboard.Model.Level == 5)
            {
                tip.level.Text = "游戏结束。得分：" + scoreboard.Model.Score.ToString().PadLeft(4, '0');
                tip.btnStart.Content = "重新游戏";
            }
            else
            {
                scoreboard.Model.Level++;
                tip.level.Text = "级别：" + scoreboard.Model.Level.ToString();
            }
        }

        void Model_Failed(object sender, EventArgs e)
        {
            _hits = 0;
        }

        void Model_Removed(object sender, EventArgs e)
        {
            _hits++;
            scoreboard.Model.AddScore(_hits);
        }
    }
}
