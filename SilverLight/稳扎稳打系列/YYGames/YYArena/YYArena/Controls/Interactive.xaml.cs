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

namespace YYArena.Controls
{
    public partial class Interactive : UserControl
    {
        public Interactive()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(StartInterface_Loaded);
        }

        void StartInterface_Loaded(object sender, RoutedEventArgs e)
        {
            root.Width = App.AreaWidth;
            root.Height = App.AreaHeight;

            controls.Text = Resource.Localization.Controls + ":";
            up.Text = "W " + Resource.Localization.Or + " ↑ = " + Resource.Localization.Forward;
            down.Text = "S " + Resource.Localization.Or + " ↓ = " + Resource.Localization.Backward;
            left.Text = "A " + Resource.Localization.Or + " ← = " + Resource.Localization.Left;
            right.Text = "D " + Resource.Localization.Or + " → = " + Resource.Localization.Right;
            fire.Text = "J " + Resource.Localization.Or + " Ctrl = " + Resource.Localization.Fire;
        }

        /// <summary>
        /// 按钮点击后
        /// </summary>
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (InteractiveType)
            {
                case InteractiveType.Start:
                    if (Start != null)
                        Start(this, EventArgs.Empty);
                    InteractiveType = InteractiveType.Running;
                    break;
                case InteractiveType.LevelUp:
                    if (LevelUp != null)
                        LevelUp(this, EventArgs.Empty);
                    InteractiveType = InteractiveType.Running;
                    break;
                case InteractiveType.Over:
                    if (Over != null)
                        Over(this, EventArgs.Empty);
                    InteractiveType = InteractiveType.Start;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 加载用户交互界面
        /// </summary>
        public void Load(InteractiveType type, int currentLevel)
        {
            InteractiveType = type;

            switch (type)
            {
                case InteractiveType.Start:
                    title.Text = title2.Text = Resource.Localization.StarArena;
                    buttonText.Text = buttonText2.Text = Resource.Localization.Play;
                    break;
                case InteractiveType.LevelUp:
                    title.Text = title2.Text = Resource.Localization.Level + ": " + currentLevel.ToString();
                    buttonText.Text = buttonText2.Text = Resource.Localization.Continue;
                    break;
                case InteractiveType.Over:
                    title.Text = title2.Text = Resource.Localization.GameOver;
                    buttonText.Text = buttonText2.Text = Resource.Localization.Again;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 交互类型
        /// </summary>
        public InteractiveType InteractiveType { get; set; }

        public event EventHandler<EventArgs> Start;
        public event EventHandler<EventArgs> LevelUp;
        public event EventHandler<EventArgs> Over;
    }
}
