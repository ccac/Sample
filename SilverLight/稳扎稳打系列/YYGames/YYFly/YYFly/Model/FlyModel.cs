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
using System.Windows.Threading;

namespace YYFly.Model
{
    public class FlyModel : IFlyModel
    {
        private DispatcherTimer timer; // 改变苍蝇位置的计时器
        private SineWave sineWave; // 苍蝇的运动轨迹（正弦波）的参数
        private FlyEventArgs evt; // 苍蝇的位置参数
        private Random random; 
        private double minX = -100, minY = 40, maxY = 300, minZ = 0.1, maxZ = 0.5;

        private static readonly object objLock = new object();

        public FlyModel()
        {
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 33);
        }

        void InitData()
        {
            random = new Random();

            sineWave = new SineWave
            {
                A = random.Next(40, 60),
                OffsetY = random.Next((int)minY, (int)maxY),
                Omega = 1d / random.Next(20, 50),
                Phi = random.Next((int)minX * 2, (int)minX),
            };

            evt = new FlyEventArgs()
            {
                X = minX,
                Z = (minZ + maxZ) / 2,
                Z_Out = Convert.ToBoolean(random.Next(0, 2))
            };
        }

        void timer_Tick(object sender, EventArgs e)
        {
            evt.X += Singleton<Scorer>.Instance.Level * 1.5;
            evt.Y = sineWave.OffsetY + sineWave.A * Math.Sin(sineWave.Omega * evt.X + sineWave.Phi);

            if (evt.Z_Out)
            {
                if (evt.Z < maxZ)
                    evt.Z += random.Next(0, 3) / 200d;
                else
                    evt.Z_Out = false;
            }
            else
            {
                if (evt.Z > minZ)
                    evt.Z -= random.Next(0, 3) / 200d;
                else
                    evt.Z_Out = true;
            }

            OnFlyChanging(evt);
        }

        public void Start()
        {
            InitData();
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        public void Score()
        {
            try
            {
                lock (objLock)
                {
                    Singleton<Scorer>.Instance.Score += 1;
                    Singleton<Scorer>.Instance.Level = (int)Singleton<Scorer>.Instance.Score / 10 + 1;
                }
            }
            finally {}
        }

        public event EventHandler<FlyEventArgs> FlyChanging;
        protected virtual void OnFlyChanging(FlyEventArgs e)
        {
            EventHandler<FlyEventArgs> handler = FlyChanging;
            if (handler != null)
                handler(this, e);
        }
    }
}
