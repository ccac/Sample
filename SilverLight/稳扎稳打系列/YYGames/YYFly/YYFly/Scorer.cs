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

using System.ComponentModel;

namespace YYFly
{
    /// <summary>
    /// 计分相关的实体类
    /// </summary>
    public class Scorer : INotifyPropertyChanged
    {
        private int _score = 0;
        /// <summary>
        /// 分数
        /// </summary>
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Score"));
            }
        }

        private int _level = 1;
        /// <summary>
        /// 级别
        /// </summary>
        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Level"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
