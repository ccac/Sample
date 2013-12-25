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

namespace YYArena.Controls
{
    public partial class Map : UserControl
    {
        public Map()
        {
            InitializeComponent();
        }

        public void Refresh()
        {
            container.Children.Clear();

            foreach (var element in Panel.Children)
            {
                if (element is PhysicsBox)
                {
                    // 地图上只显示 PhysicsBox 类型的控件（飞船和子弹）
                    var physicsBox = element as PhysicsBox;

                    Rectangle rect = new Rectangle();

                    // 友军在地图上显示为蓝色，敌军为红色
                    if (physicsBox.Geom.CollisionGroup == (int)SpriteType.Friend)
                        rect.Fill = new SolidColorBrush(Colors.Blue);
                    else
                        rect.Fill = new SolidColorBrush(Colors.Red);

                    // 飞船在地图上显示为 4×4 矩形，子弹为 2×2 矩形
                    if (physicsBox.PhysicsControl.Control is Bullet)
                        rect.Width = rect.Height = 2;
                    else
                        rect.Width = rect.Height = 4;

                    // 设置需要显示的矩形框的位置
                    rect.SetValue(Canvas.LeftProperty, (double)element.GetValue(Canvas.LeftProperty) * this.Width / App.SceneWidth - rect.Width / 2);
                    rect.SetValue(Canvas.TopProperty, (double)element.GetValue(Canvas.TopProperty) * this.Height / App.SceneHeight - rect.Height / 2);

                    container.Children.Add(rect);
                }
            }
        }

        /// <summary>
        /// 需要侦测的 Panel 控件
        /// 地图上会缩略显示该 Panel 控件内的内容
        /// </summary>
        public Panel Panel { get; set; }
    }
}
