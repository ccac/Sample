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
using FarseerGames.FarseerPhysics.Dynamics;

namespace YYArena.Core
{
    /// <summary>
    /// 敌军 Sprite
    /// </summary>
    public class AISprite : Sprite, IFire
    {
        private Sprite _attackTarget;
        private int _aiLevel;
        private IPhysicsControl _physicsControl;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="physicsSimulator">PhysicsSimulator</param>
        /// <param name="physicsControl">IPhysicsControl</param>
        /// <param name="position">初始位置</param>
        /// <param name="angle">初始转角</param>
        /// <param name="originalVelocity">初始速度</param>
        /// <param name="attackTarget">攻击目标</param>
        /// <param name="aiLevel">ai等级</param>
        public AISprite(PhysicsSimulator physicsSimulator,
            IPhysicsControl physicsControl, Vector2 position, float angle, float originalVelocity, Sprite attackTarget, int aiLevel)
            : base(physicsSimulator, physicsControl, position, angle, originalVelocity)
        {
            // 上次开火时间
            PrevFireDateTime = DateTime.Now.AddSeconds(3);
            // 最小开火间隔
            MinFireInterval = 3000d;

            _attackTarget = attackTarget;
            _aiLevel = aiLevel;
            _physicsControl = physicsControl;

            InitAI();

            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        private void InitAI()
        {
            // 根据 ai 等级设置最小开火间隔
            double fireCoefficient = 1 + 30 / _aiLevel;
            MinFireInterval = Helper.GenerateRandom((int)MinFireInterval, (int)(fireCoefficient * MinFireInterval));
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (Enabled && AttackTarget.Enabled)
            {
                // 是否开火
                if ((DateTime.Now - PrevFireDateTime).TotalMilliseconds > MinFireInterval)
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


        public Sprite AttackTarget
        {
            get { return _attackTarget; }
            set { _attackTarget = value; }
        }

        protected override Vector2 GetForce()
        {
            Vector2 force = Vector2.Zero;

            if (!_attackTarget.Enabled)
                return force;

            force += Helper.Convert2Vector(_physicsControl.ForceAmount, playerBox.Body.Rotation);

            // 根据 ai 等级做最大线性速度限制
            if (playerBox.Body.LinearVelocity.Length() > _physicsControl.MaxLinearVelocity * Helper.GenerateRandom(50, 200) / 1000)
                force = Vector2.Zero;

            return force;
        }

        protected override float GetTorque()
        {
            float torque = 0f;

            if (!_attackTarget.Enabled)
                return torque;

            // 按某个方向旋转，原则是以最小的旋转角度对准目标
            Vector2 relativePosition = _attackTarget.PhysicsBox.Body.Position - playerBox.Body.Position;
            double targetRotation = Helper.Convert2Rotation(relativePosition);
            double currentRotation = playerBox.Body.Rotation;
            double relativeAngle = Helper.Radian2Angle(targetRotation - currentRotation);
            if (relativeAngle < 0)
                relativeAngle += 360;

            if (relativeAngle > 1)
            {
                if (relativeAngle < 180 && relativeAngle > 0)
                    torque += _physicsControl.TorqueAmount;
                else if (relativeAngle > 180 && relativeAngle < 360)
                    torque -= _physicsControl.TorqueAmount;
            }
            else
            {
                playerBox.Body.AngularVelocity = 0;
            }


            // 最大转速限制
            if (Math.Abs(playerBox.Body.AngularVelocity) > _physicsControl.MaxAngularVelocity)
                torque = 0;
           
            return torque;
        }
    }
}
