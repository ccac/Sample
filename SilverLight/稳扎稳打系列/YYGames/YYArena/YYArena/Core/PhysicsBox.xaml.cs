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

using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Mathematics;
using FarseerGames.FarseerPhysics.Factories;
using FarseerGames.FarseerPhysics.Collisions;

namespace YYArena.Core
{
    /// <summary>
    /// 物理控件的容器
    /// </summary>
    public partial class PhysicsBox : UserControl
    {
        private float _x;
        private float _y;
        private float _rotation;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="element">物理控件</param>
        public PhysicsBox(IPhysicsControl element)
        {
            InitializeComponent();

            PhysicsControl = element;

            root.Children.Add(element.Control);
            Body = BodyFactory.Instance.CreateRectangleBody((float)element.Control.Width, (float)element.Control.Height, element.Mass);
            Body.LinearDragCoefficient = element.LinearDragCoefficient;
            Body.RotationalDragCoefficient = element.RotationalDragCoefficient;

            Geom = new Geom(Body, element.Vertices, Vector2.Zero, 0, element.Vertices.GetShortestEdge());

            // 使控件的位置为相对其中心点的位置
            translateTransform.X = -element.Control.Width / 2;
            translateTransform.Y = -element.Control.Height / 2;
        }

        /// <summary>
        /// 更新控件的位置
        /// </summary>
        public void Update()
        {
            if (Body == null)
                return;

            if (_x != Body.Position.X)
            {
                _x = Body.Position.X;
                this.SetValue(Canvas.LeftProperty, Convert.ToDouble(_x));
            }
            if (_y != Body.Position.Y)
            {
                _y = Body.Position.Y;
                this.SetValue(Canvas.TopProperty, Convert.ToDouble(_y));
            }
            if (Body.Rotation != _rotation)
            {
                _rotation = Body.Rotation;
                rotateTransform.Angle = (float)Helper.Radian2Angle(_rotation);
            }
        }

        public Body Body { get; set; }

        public Geom Geom { get; set; }

        public IPhysicsControl PhysicsControl { get; set; }
    }
}
