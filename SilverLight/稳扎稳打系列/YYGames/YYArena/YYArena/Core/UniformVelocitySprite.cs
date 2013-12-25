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
    /// 匀速运动的 Sprite
    /// </summary>
    public class UniformVelocitySprite : Sprite
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="physicsSimulator">PhysicsSimulator</param>
        /// <param name="physicsControl">IPhysicsControl（初始速度为 IPhysicsControl.MaxLinearVelocity）</param>
        /// <param name="position">初始位置</param>
        /// <param name="angle">初始转角</param>
        public UniformVelocitySprite(PhysicsSimulator physicsSimulator,
            IPhysicsControl physicsControl, Vector2 position, float angle)
            : base(physicsSimulator, physicsControl, position, angle, physicsControl.MaxLinearVelocity)
        {

        }

        protected override Vector2 GetForce()
        {
            return Vector2.Zero;
        }

        protected override float GetTorque()
        {
            return 0f;
        }
    }
}
