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

using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Mathematics;
using YYArena.Core;
using System.Windows.Media.Imaging;

namespace YYArena.Controls
{
    public partial class Bullet : UserControl, YYArena.Core.IPhysicsControl
    {
        public Bullet()
        {
            InitializeComponent();
        }

        private SpriteType _bulletType;
        /// <summary>
        /// 子弹类型。两种（友军和敌军）
        /// </summary>
        public SpriteType BulletType
        {
            get { return _bulletType; }
            set
            {
                _bulletType = value;

                switch (value)
                {
                    case SpriteType.Friend:
                        imgBullet.Source = new BitmapImage(new Uri("/Resource/bullet1.png", UriKind.Relative));
                        break;
                    case SpriteType.Enemy:
                        imgBullet.Source = new BitmapImage(new Uri("/Resource/bullet2.png", UriKind.Relative));
                        break;
                    default:
                        imgBullet.Source = new BitmapImage(new Uri("/Resource/bullet1.png", UriKind.Relative));
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

        private float _forceAmount = 0f;
        public float ForceAmount
        {
            get { return _forceAmount; }
            set { _forceAmount = value; }
        }

        private float _torqueAmount = 0f;
        public float TorqueAmount
        {
            get { return _torqueAmount; }
            set { _torqueAmount = value; }
        }

        private float _linearDragCoefficient = 0f;
        public float LinearDragCoefficient
        {
            get { return _linearDragCoefficient; }
            set { _linearDragCoefficient = value; }
        }

        private float _rotationalDragCoefficient = 0f;
        public float RotationalDragCoefficient
        {
            get { return _rotationalDragCoefficient; }
            set { _rotationalDragCoefficient = value; }
        }

        private float _maxLinearVelocity = 250f;
        public float MaxLinearVelocity
        {
            get { return _maxLinearVelocity; }
            set { _maxLinearVelocity = value; }
        }

        private float _maxAngularVelocity = 0f;
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
                    _vertices = Vertices.CreateRectangle(8f, 32f);
                return _vertices; 
            }
            set { _vertices = value; }
        }
        #endregion
    }
}
