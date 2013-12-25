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
    public partial class Explosion : UserControl
    {
        public Explosion()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Explosion_Loaded);
        }

        void Explosion_Loaded(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// 爆炸效果
        /// </summary>
        /// <param name="targetX">爆炸点的 x 坐标</param>
        /// <param name="targetY">爆炸点的 y 坐标</param>
        /// <param name="explosionRadius">爆炸半径</param>
        /// <param name="ballAmount">爆炸所产生的火球数量</param>
        /// <param name="minBallRadius">爆炸所产生的火球的最小半径</param>
        /// <param name="maxBallRadius">爆炸所产生的火球的最大半径</param>
        /// <param name="opacity">爆炸的不透明度</param>
        public void GenerateExplosion(double targetX, double targetY, int explosionRadius, int ballAmount, int minBallRadius, int maxBallRadius, double opacity)
        {
            Random random = new Random();

            for (int i = 0; i < ballAmount; i++)
            {
                var ball = new Fireball();
                int ballRadius = random.Next(minBallRadius, maxBallRadius + 1);

                ball.SetValue(Canvas.LeftProperty, targetX - ballRadius + random.Next(-explosionRadius + maxBallRadius, explosionRadius - maxBallRadius));
                ball.SetValue(Canvas.TopProperty, targetY - ballRadius + random.Next(-explosionRadius + maxBallRadius, explosionRadius - maxBallRadius));
                ball.Radius = ballRadius;
                ball.Opacity = opacity;

                ball.Begin(new TimeSpan(0, 0, 0, 0, Math.Abs(120 - i * 18)));

                ball.Completed += new EventHandler<EventArgs>(ball_Completed);

                container.Children.Add(ball);
            }
        }

        /// <summary>
        /// 火球爆发完后，从容器中移除该火球控件
        /// </summary>
        void ball_Completed(object sender, EventArgs e)
        {
            container.Children.Remove((Fireball)sender);
        }
    }
}
