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

using FarseerGames.FarseerPhysics.Mathematics;

namespace YYArena.Core
{
    public class Helper
    {
        /// <summary>
        /// 获取某点的坐标
        /// </summary>
        /// <param name="value">某点到原点的距离</param>
        /// <param name="rotation">某点到原点的连线与 ↑ 方向的夹角（弧度）</param>
        /// <returns></returns>
        public static Vector2 Convert2Vector(double value, double rotation)
        {
            rotation = rotation - Angle2Radian(90);

            return new Vector2(
                (float)(value * Math.Cos(rotation)),
                (float)(value * Math.Sin(rotation)));
        }

        /// <summary>
        /// 获取某点到原点的连线与 ↑ 方向的夹角（弧度 0 - 2*Math.PI）
        /// </summary>
        /// <param name="position">某点的位置（x 坐标和 y 坐标）</param>
        /// <returns></returns>
        public static double Convert2Rotation(Vector2 position)
        {
            var x = position.X;
            var y = position.Y;
            var rotation = Math.Atan(y / x);

            if ((y > 0 && x > 0) || (y < 0 && x > 0))
                rotation = rotation + Angle2Radian(90);
            else
                rotation = rotation - Angle2Radian(90);

            if (rotation < 0)
                rotation += 2 * Math.PI;

            return rotation;
        }

        /// <summary>
        /// 弧度转换为角度
        /// </summary>
        /// <param name="radian">弧度</param>
        /// <returns></returns>
        public static double Radian2Angle(double radian)
        {
            return radian * 180 / Math.PI;
        }

        /// <summary>
        /// 角度转换为弧度
        /// </summary>
        /// <param name="angle">弧度</param>
        /// <returns></returns>
        public static double Angle2Radian(double angle)
        {
            return angle * Math.PI / 180;
        }

        private static Random _random;
        /// <summary>
        /// 生成一个指定范围的随机数
        /// </summary>
        public static int GenerateRandom(int x, int y)
        {
            if (_random == null)
                _random = new Random();

            return _random.Next(x, y + 1);
        }
    }
}
