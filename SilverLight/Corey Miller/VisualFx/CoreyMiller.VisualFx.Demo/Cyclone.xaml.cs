using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using CoreyMiller.VisualFx.Demo.CycloneParts;

namespace CoreyMiller.VisualFx.Demo
{
    public partial class Cyclone : UserControl
    {

        #region Declarations

        int _maxParticles = 200;
        int _funnelHeight = 3000;
        int _speed = 300;
        double _stiffness = .65;

        bool _dragging = false;
        int _numParticles = 0;

        List<CycloneOval> _ovals = new List<CycloneOval>();

        System.Windows.Threading.DispatcherTimer _timer = new System.Windows.Threading.DispatcherTimer();

        #endregion

        #region Constructor

        public Cyclone()
        {
            InitializeComponent();

            //set the timer control for dynamic drawing
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 5);
            _timer.Tick += new EventHandler(_timer_Tick);
            _timer.Start();
        }

        #endregion

        #region Properties

        /// <summary>
        /// default value is 200
        /// </summary>
        public int MaxParticles
        {
            get { return _maxParticles; }
            set { _maxParticles = value; }
        }

        /// <summary>
        /// default value is 3000
        /// </summary>
        public int FunnelHeight
        {
            get { return _funnelHeight; }
            set { _funnelHeight = value; }
        }

        /// <summary>
        /// default value is 300
        /// </summary>
        public int Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        /// <summary>
        /// value between 0 to 1
        /// </summary>
        public double Stiffness
        {
            get { return _stiffness; }
            set { _stiffness = value; }
        }

        #endregion

        //what happens every frame
        private void Live()
        {
            if (_dragging) Sidewind();
        }

        //tell all particles to follow the cursor
        private void Sidewind()
        {
            for (int i = _maxParticles-1; i >= 0; i--)
            {
                if (i + 1 == _maxParticles)
                {
                    _ovals[i].Sidewind((double)ball.GetValue(Canvas.LeftProperty) + 7.5);
                }
                else
                {
                    _ovals[i].Sidewind((double)_ovals[i + 1].GetValue(Canvas.LeftProperty));
                }
            }
        }
        
        //creates the particles
        private void MakeParticle(int index)
        {
            double radius = index * index / _maxParticles + 2;

            double scale = radius * .8;

            CycloneOval oval = new CycloneOval(index, _stiffness, radius, scale, _speed);
            
            LayoutRoot.Children.Add(oval);

            oval.SetValue(Canvas.TopProperty, _funnelHeight / (radius + 10));

            _ovals.Add(oval);
        }
        
        //timer controls the entire thing... this event is like a frame command in flash
        void _timer_Tick(object sender, EventArgs e)
        {
            if (_numParticles < _maxParticles)
            {
                MakeParticle(_maxParticles - _numParticles);
                _numParticles++;
            }

            Live();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ball.CaptureMouse();
            _dragging = true;
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragging = false;
            ball.ReleaseMouseCapture();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_dragging)
            {
                double _x = e.GetPosition(LayoutRoot).X;

                ball.SetValue(Canvas.LeftProperty, _x - 7.5);
            }
        }
      
    }
}
