/*
 * 得分先暂存到一个临时变量中，然后由定时器负责一部分一部分地更新 UI 上的分数，以出现动态效果
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

using System.Threading;

namespace YYPiano.Controls
{
    public partial class Score : UserControl
    {
        private Timer _timer = null;

        // 还需要增加的分数
        private int _bufferScore = 0;

        public Score()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Score_Loaded);
        }

        void Score_Loaded(object sender, RoutedEventArgs e)
        {
            _timer = new Timer(MyTimerCallback, null, 0, 10);
        }

        private void MyTimerCallback(object state)
        {
            // 如果有需要增加的分数
            if (_bufferScore > 0)
            {
                // 定时器的此次回调需要增加的得分数（缓冲分数为1或2位数则此次回调增加1分，3位数增加10分，4位数增加100分，以此类推）
                var i = 1;
                if (_bufferScore.ToString().Length > 2)
                    i = (int)Math.Pow(10, _bufferScore.ToString().Length - 2);

                Interlocked.Add(ref _bufferScore, -i);

                this.Dispatcher.BeginInvoke(
                    () => lblScore.Text = (int.Parse(lblScore.Text) + i).ToString().PadLeft(6, '0'));
            }
        }

        /// <summary>
        /// 增加分数
        /// </summary>
        /// <param name="score">需要增加分数</param>
        public void AddScore(int score)
        {
            Interlocked.Add(ref _bufferScore, score);
        }

        /// <summary>
        /// 分数清零
        /// </summary>
        public void ClearScore()
        {
            Interlocked.Exchange(ref _bufferScore, 0);
            lblScore.Text = "000000";
        }
    }
}
