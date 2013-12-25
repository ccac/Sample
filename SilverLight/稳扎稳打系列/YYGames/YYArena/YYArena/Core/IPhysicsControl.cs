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

using FarseerGames.FarseerPhysics.Collisions;

namespace YYArena.Core
{
    /// <summary>
    /// 物理控件接口
    /// </summary>
    public interface IPhysicsControl
    {
        /// <summary>
        /// 控件
        /// </summary>
        Control Control { get; set; }

        /// <summary>
        /// 质量
        /// </summary>
        float Mass { get; set; }

        /// <summary>
        /// 牵引力
        /// </summary>
        float ForceAmount { get; set; }
        /// <summary>
        /// 力矩
        /// </summary>
        float TorqueAmount { get; set; }

        /// <summary>
        /// 线性摩擦系数
        /// </summary>
        float LinearDragCoefficient { get; set; }
        /// <summary>
        /// 旋转摩擦系数
        /// </summary>
        float RotationalDragCoefficient { get; set; }

        /// <summary>
        /// 最大线性速度
        /// </summary>
        float MaxLinearVelocity { get; set; }
        /// <summary>
        /// 做大旋转速度
        /// </summary>
        float MaxAngularVelocity { get; set; }

        /// <summary>
        /// 用于检测碰撞的图形（点的集合）
        /// </summary>
        Vertices Vertices { get; set; }
    }
}
