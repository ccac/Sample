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

using YYMatch.Models;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;

namespace YYMatch.ViewModels
{
    public class ScoreboardViewModel : INotifyPropertyChanged
    {
        DispatcherTimer _timer;

        public ScoreboardViewModel()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Tick += new EventHandler(_timer_Tick);
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            Time = Time.Add(new TimeSpan(0, 0, 1));
            AddScore(-1);
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            _time = new TimeSpan(0, 0, 0);
            _level = 1;
            _score = 0;
        }

        public void Pause()
        {
            _timer.Stop();
        }

        public void AddScore(int score)
        {
            Interlocked.Add(ref _score, score);

            if (_score < 0)
                Interlocked.Exchange(ref _score, 0);
            
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Score"));
        }

        private TimeSpan _time;
        /// <summary>
        /// 用时
        /// </summary>
        public TimeSpan Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Time"));
            }
        }

        private int _level = 1;
        /// <summary>
        /// 级别
        /// </summary>
        public int Level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Level"));
            }
        }

        private int _score;
        /// <summary>
        /// 得分
        /// </summary>
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Score"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
