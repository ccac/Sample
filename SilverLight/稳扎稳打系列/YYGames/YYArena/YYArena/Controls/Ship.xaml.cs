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

using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Mathematics;
using System.Windows.Media.Imaging;

namespace YYArena.Controls
{
    public partial class Ship : System.Windows.Controls.UserControl, YYArena.Core.IPhysicsControl
    {
        public Ship()
        {
            InitializeComponent();
        }

        private SpriteType _shipType;
        /// <summary>
        /// 飞船的类型（两种：友军和敌军）
        /// </summary>
        public SpriteType ShipType
        {
            get { return _shipType; }
            set
            {
                _shipType = value;

                switch (value)
                {
                    case SpriteType.Friend:
                        imgShip.Source = new BitmapImage(new Uri("/Resource/ship1.png", UriKind.Relative));
                        break;
                    case SpriteType.Enemy:
                        imgShip.Source = new BitmapImage(new Uri("/Resource/ship2.png", UriKind.Relative));
                        break;
                    default:
                        imgShip.Source = new BitmapImage(new Uri("/Resource/ship1.png", UriKind.Relative));
                        break;
                }
            }
        }

        #region YYArena.Core.IPhysicsControl 接口的实现
        private Control _control;
        public Control Control
        {
            get
            {
                if (_control == null)
                    _control = (Control)this;
                return _control;
            }
            set { _control = value; }
        }

        private float _mass = 1f;
        public float Mass
        {
            get { return _mass; }
            set { _mass = value; }
        }

        private float _forceAmount = 1000f;
        public float ForceAmount
        {
            get { return _forceAmount; }
            set { _forceAmount = value; }
        }

        private float _torqueAmount = 5000f;
        public float TorqueAmount
        {
            get { return _torqueAmount; }
            set { _torqueAmount = value; }
        }

        private float _linearDragCoefficient = 2f;
        public float LinearDragCoefficient
        {
            get { return _linearDragCoefficient; }
            set { _linearDragCoefficient = value; }
        }

        private float _rotationalDragCoefficient = 300f;
        public float RotationalDragCoefficient
        {
            get { return _rotationalDragCoefficient; }
            set { _rotationalDragCoefficient = value; }
        }

        private float _maxLinearVelocity = 130f;
        public float MaxLinearVelocity
        {
            get { return _maxLinearVelocity; }
            set { _maxLinearVelocity = value; }
        }

        private float _maxAngularVelocity = 2f;
        public float MaxAngularVelocity
        {
            get { return _maxAngularVelocity; }
            set { _maxAngularVelocity = value; }
        }

        private Vertices _vertices;
        public Vertices Vertices
        {
            get
            {
                if (_vertices == null)
                {
                    _vertices = new Vertices();
                    _vertices = Vertices.CreateRectangle(32f, 32f);
                    //_vertices.Add(new Vector2(-5, -20));
                    //_vertices.Add(new Vector2(5, -20));
                    //_vertices.Add(new Vector2(5, -10));
                    //_vertices.Add(new Vector2(20, -10));
                    //_vertices.Add(new Vector2(20, 20));
                    //_vertices.Add(new Vector2(-20, 20));
                    //_vertices.Add(new Vector2(-20, -10));
                    //_vertices.Add(new Vector2(-5, -10));
                }

                return _vertices;
            }
            set { _vertices = value; }
        }
        #endregion
    }
}
