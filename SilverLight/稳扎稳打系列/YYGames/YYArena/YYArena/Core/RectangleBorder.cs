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

using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Mathematics;
using FarseerGames.FarseerPhysics.Factories;
using FarseerGames.FarseerPhysics;

namespace YYArena.Core
{
    /// <summary>
    /// 矩形边框
    /// </summary>
    public class RectangleBorder
    {
        PhysicsSimulator _physicsSimulator;

        private Body _floorBody;
        private Geom _floorGeom;

        private float _height;
        private float _width;

        private Vector2 _position;

        private int _borderThickness = 100;

        public RectangleBorder(float width, float height, Vector2 position)
        {
            _width = width;
            _height = height;
            _position = position;
        }

        public void Load(PhysicsSimulator physicsSimulator)
        {
            _physicsSimulator = physicsSimulator;

            LoadBorder(_width, _borderThickness, new Vector2(_width / 2, -_borderThickness / 2));
            LoadBorder(_width, _borderThickness, new Vector2(_width / 2, _height + _borderThickness / 2));
            LoadBorder(_borderThickness, _height, new Vector2(-_borderThickness / 2, _height / 2));
            LoadBorder(_borderThickness, _height, new Vector2(_width + _borderThickness / 2, _height / 2));
        }

        private void LoadBorder(float width, float height, Vector2 position)
        {
            _floorBody = BodyFactory.Instance.CreateRectangleBody(_physicsSimulator, width, height, 1);
            _floorBody.IsStatic = true;
            _floorGeom = GeomFactory.Instance.CreateRectangleGeom(_physicsSimulator, _floorBody, width, height);
            _floorGeom.RestitutionCoefficient = .4f;
            _floorGeom.FrictionCoefficient = .4f;
            _floorGeom.CollisionGroup = CollisionGroup;
            _floorBody.Position = position;
        }

        public int CollisionGroup { get; set; }
    }
}
