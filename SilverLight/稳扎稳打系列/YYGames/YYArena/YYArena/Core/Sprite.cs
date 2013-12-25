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

using FarseerGames.FarseerPhysics;
using FarseerGames.FarseerPhysics.Mathematics;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Collisions;

namespace YYArena.Core
{
    /// <summary>
    /// Sprite 基类
    /// </summary>
    public abstract class Sprite
    {
        private PhysicsSimulator _physicsSimulator;

        protected PhysicsBox playerBox;
        protected Geom playerGeometry;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="physicsSimulator">PhysicsSimulator</param>
        /// <param name="physicsControl">IPhysicsControl</param>
        /// <param name="position">初始位置</param>
        /// <param name="angle">初始转角</param>
        /// <param name="originalVelocity">初始速度</param>
        public Sprite(PhysicsSimulator physicsSimulator,
            IPhysicsControl physicsControl, Vector2 position, float angle, float originalVelocity)
        {
            _physicsSimulator = physicsSimulator;

            playerBox = new PhysicsBox(physicsControl);
            playerBox.Body.Position = position;
            playerBox.Body.Rotation = (float)Helper.Angle2Radian(angle);
            playerBox.Body.LinearVelocity = Helper.Convert2Vector(originalVelocity, (float)Helper.Angle2Radian(angle));

            // Body 和 Geom 的 Tag 保存为 Sprite，方便引用
            playerBox.Body.Tag = this;
            playerBox.Geom.Tag = this;

            playerBox.Update();
        }

        /// <summary>
        /// 即时计算力和力矩
        /// </summary>
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (Enabled)
            {
                var force = GetForce();
                var torque = GetTorque();

                playerBox.Body.ApplyForce(force);
                playerBox.Body.ApplyTorque(torque);

                playerBox.Update();
            }
        }

        /// <summary>
        /// 返回 Sprite 当前受的力
        /// </summary>
        protected abstract Vector2 GetForce();
        /// <summary>
        /// 返回 Sprite 当前受的力矩
        /// </summary>
        protected abstract float GetTorque();

        public PhysicsBox PhysicsBox
        {
            get { return playerBox; }
        }

        private bool _enabled = false;
        /// <summary>
        /// 是否启用此 Sprite
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set
            { 
                _enabled = value;

                if (value)
                {
                    CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);

                    _physicsSimulator.Add(playerBox.Body);
                    _physicsSimulator.Add(playerBox.Geom);
                }
                else
                {
                    CompositionTarget.Rendering -= new EventHandler(CompositionTarget_Rendering);

                    GC.SuppressFinalize(this);
                    _physicsSimulator.Remove(playerBox.Body);
                    _physicsSimulator.Remove(playerBox.Geom);
                }
            }
        }
    }
}
