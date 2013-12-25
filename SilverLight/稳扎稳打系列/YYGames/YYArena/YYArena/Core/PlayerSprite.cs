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

using System.Collections.Generic;
using FarseerGames.FarseerPhysics.Mathematics;
using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Collisions;

namespace YYArena.Core
{
    /// <summary>
    /// 玩家 Sprite
    /// </summary>
    public class PlayerSprite : Sprite, IFire
    {
        private List<Key> _upKeys { get; set; }
        private List<Key> _downKeys { get; set; }
        private List<Key> _leftKeys { get; set; }
        private List<Key> _rightKeys { get; set; }
        private List<Key> _fireKeys { get; set; }

        private KeyboardHandler _keyHandler;
        private IPhysicsControl _physicsControl;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="physicsSimulator">PhysicsSimulator</param>
        /// <param name="physicsControl">IPhysicsControl</param>
        /// <param name="position">初始位置</param>
        /// <param name="angle">初始转角</param>
        /// <param name="originalVelocity">初始速度</param>
        /// <param name="keyboardHandler">KeyboardHandler</param>
        /// <param name="up">操作玩家向前移动的按键集合</param>
        /// <param name="down">操作玩家向后移动的按键集合</param>
        /// <param name="left">操作玩家向左转动的按键集合</param>
        /// <param name="right">操作玩家向右转动的按键集合</param>
        /// <param name="fire">操作玩家开火的按键集合</param>
        public PlayerSprite(PhysicsSimulator physicsSimulator,
            IPhysicsControl physicsControl, Vector2 position, float angle, float originalVelocity,
            KeyboardHandler keyboardHandler,
            List<Key> up, List<Key> down, List<Key> left, List<Key> right, List<Key> fire)
            : base(physicsSimulator, physicsControl, position, angle, originalVelocity)
        {
            PrevFireDateTime = DateTime.MinValue;
            MinFireInterval = 500d;

            _upKeys = up;
            _downKeys = down;
            _leftKeys = left;
            _rightKeys = right;
            _fireKeys = fire;

            _keyHandler = keyboardHandler;
            _physicsControl = physicsControl;

            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (Enabled)
            {
                // 如果按了开火键，是否可开火
                if (_keyHandler.AnyKeyPressed(_fireKeys) && (DateTime.Now - PrevFireDateTime).TotalMilliseconds > MinFireInterval)
                {
                    PrevFireDateTime = DateTime.Now;
                    if (Fire != null)
                        Fire(this, EventArgs.Empty);
                }
            }
        }

        public DateTime PrevFireDateTime { get; set; }

        public double MinFireInterval { get; set; }

        public event EventHandler<EventArgs> Fire;

        protected override Vector2 GetForce()
        {
            Vector2 force = Vector2.Zero;

            if (_keyHandler.AnyKeyPressed(_upKeys))
                force += Helper.Convert2Vector(_physicsControl.ForceAmount, playerBox.Body.Rotation);
            if (_keyHandler.AnyKeyPressed(_downKeys))
                force += Helper.Convert2Vector(_physicsControl.ForceAmount, playerBox.Body.Rotation - Helper.Angle2Radian(180));

            //左右移动
            //if (_keyHandler.AnyKeyPressed(_leftKeys))
            //    force += Helper.Convert2Vector(_physicsControl.ForceAmount, playerBox.Body.Rotation - Helper.Angle2Radian(90));
            //if (_keyHandler.AnyKeyPressed(_rightKeys))
            //    force += Helper.Convert2Vector(_physicsControl.ForceAmount, playerBox.Body.Rotation - Helper.Angle2Radian(270));


            // 最大线性速度限制
            if (playerBox.Body.LinearVelocity.Length() > _physicsControl.MaxLinearVelocity)
                force = Vector2.Zero;

            return force;
        }

        protected override float GetTorque()
        {
            float torque = 0;

            if (_keyHandler.AnyKeyPressed(_leftKeys))
                torque -= _physicsControl.TorqueAmount;
            if (_keyHandler.AnyKeyPressed(_rightKeys))
                torque += _physicsControl.TorqueAmount;

            // 用于修正 RotationalDragCoefficient （在没有任何 Torque 的情况下，如果转速小于 1.3 则设其为 0）
            // 如果不做此修正的话，转速小于 1.3 后还会转好长时间
            if (!_keyHandler.AnyKeyPressed(_leftKeys) && !_keyHandler.AnyKeyPressed(_rightKeys) && Math.Abs(playerBox.Body.AngularVelocity) < 1.3)
                playerBox.Body.AngularVelocity = 0;

            // 最大转速限制
            if (Math.Abs(playerBox.Body.AngularVelocity) > _physicsControl.MaxAngularVelocity)
                torque = 0;

            return torque;
        }
    }
}
