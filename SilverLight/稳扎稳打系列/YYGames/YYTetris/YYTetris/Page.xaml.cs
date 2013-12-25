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

using YYTetris.Piece;
using System.Windows.Threading;

namespace YYTetris
{
    public partial class Page : UserControl
    {
        UIControl _control;

        public Page()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            uc.Focus();

            _control = new UIControl();
            _control.GameOver += new EventHandler(_control_GameOver);
            uc.DataContext = _control;

            foreach (Block block in _control.Container)
            {
                canvasBox.Children.Add(block);
            }

            foreach (Block block in _control.NextContainer)
            {
                canvasBoxPrev.Children.Add(block);
            }
        }

        void _control_GameOver(object sender, EventArgs e)
        {
            gameOver.Visibility = Visibility.Visible;
            play.Content = "开始游戏";
        }

        private void uc_KeyDown(object sender, KeyEventArgs e)
        {
            if (_control.GameStatus != GameStatus.Play) return;

            if (e.Key == Key.Left)
            {
                _control.MoveToLeft();
            }
            else if (e.Key == Key.Right)
            {
                _control.MoveToRight();
            }
            else if (e.Key == Key.Up)
            {
                _control.Rotate();
            }
            else if (e.Key == Key.Down)
            {
                _control.MoveToDown();
            }
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            if (play.Content.ToString() == "开始游戏")
            {
                if (_control.GameStatus == GameStatus.Over)
                {
                    _control.Clear();
                    gameOver.Visibility = Visibility.Collapsed;
                    _control.Score = 0;
                    _control.Level = 0;
                    _control.RemoveRowCount = 0;
                }

                _control.Play();
                play.Content = "暂停游戏";
            }
            else
            {
                _control.Pause();
                play.Content = "开始游戏";
            }
        }
    }
}
