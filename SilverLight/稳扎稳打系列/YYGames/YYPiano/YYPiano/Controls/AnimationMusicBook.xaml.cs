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

using YYPiano.Controls.Parts;
using System.Threading;

namespace YYPiano.Controls
{
    /// <summary>
    /// 乐谱动画
    /// </summary>
    public partial class AnimationMusicBook : UserControl
    {
        /// <summary>
        /// 当前进入到目标区域的按键集合（先进先出）
        /// </summary>
        private List<KeyHitModel> _currentKeys = new List<KeyHitModel>();

        public AnimationMusicBook()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 启动乐谱动画
        /// </summary>
        /// <param name="code">乐谱编码</param>
        /// <returns>是否成功地启动了乐谱动画</returns>
        public bool Start(string code)
        {
            code = code.ToUpper().Trim();

            // 清除已有的 AnimationKey 控件
            foreach (var c in root.Children)
            {
                var ak = c as AnimationKey;
                ak.Stop();
            }
            root.Children.Clear();
            _currentKeys.Clear();

            // 把乐谱编码解析为乐谱实体类（用于描述乐谱的每一音阶）集合
            var musicBook = new List<MusicBookModel>();
            var countDelay = 0;
            try
            {
                foreach (var s in code.Split(','))
                {
                    var delay = int.Parse(s.Trim().Substring(1));
                    var key = Convert.ToChar(s.Trim().Substring(0, 1)).ToKey();

                    musicBook.Add(new MusicBookModel() { Length = countDelay, Key = key });

                    countDelay += delay;
                }
            }
            catch (Exception)
            {
                return false;
            }

            // 在容器内放置相应的 AnimationKey 控件
            for (int i = 0; i < musicBook.Count; i++)
            {
                AnimationKey key = new AnimationKey();
                key.TargetIndex = i % 3;
                key.Key = musicBook[i].Key;
                key.BeginTime = TimeSpan.FromMilliseconds(musicBook[i].Length);
                key.Inside += new EventHandler<PianoKeyEventArgs>(key_Inside);
                key.Outside += new EventHandler<PianoKeyEventArgs>(key_Outside);
                key.Start();

                root.Children.Add(key);
            }

            return true;
        }

        /// <summary>
        /// 按键进入目标区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void key_Inside(object sender, PianoKeyEventArgs e)
        {
            _currentKeys.Add(new KeyHitModel { Key = e.Key, Hit = false });
        }

        /// <summary>
        /// 按键离开目标区
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void key_Outside(object sender, PianoKeyEventArgs e)
        {
            // 获取此次离开目标区的按键（进入到目标区域的按键集合的第一个成员）
            var key = _currentKeys.First();

            if (!key.Hit)
                OnLost();

            _currentKeys.RemoveAt(0);
        }

        /// <summary>
        /// 指定的键值被敲击后所执行的方法
        /// </summary>
        /// <param name="key">键值</param>
        public void Play(Key key)
        {
            if (key >= Key.A && key <= Key.Z && _currentKeys.Where(p => !p.Hit).Count() > 0)
            {
                var validKey = _currentKeys.Where(p => !p.Hit && p.Key == key).FirstOrDefault();
                if (validKey != null)
                {
                    OnScore();
                    validKey.Hit = true;
                }
                else
                {
                    OnLost();
                }
            }
        }

        /// <summary>
        /// 按键敲击正确的事件
        /// </summary>
        public event EventHandler<EventArgs> Score;
        public void OnScore()
        {
            if (Score != null)
            {
                Score(this, new EventArgs());
            }
        }

        /// <summary>
        /// 按键敲击错误或未及时敲击的事件
        /// </summary>
        public event EventHandler<EventArgs> Lost;
        public void OnLost()
        {
            if (Lost != null)
            {
                Lost(this, new EventArgs());
            }
        }
    }
}
