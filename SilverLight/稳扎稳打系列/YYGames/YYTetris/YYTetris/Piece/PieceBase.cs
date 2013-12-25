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

namespace YYTetris.Piece
{
    public abstract class PieceBase
    {
        public PieceBase()
        {
            InitPiece();
        }

        // 形状的矩阵
        public int[,] Matrix { get; set; }

        // 形状的索引
        private int _index = 0;

        // 形状的最大索引
        public int MaxIndex { get; set; }

        /// <summary>
        /// 初始化形状，需要设置 Matrix 和 MaxIndex
        /// </summary>
        public abstract void InitPiece();

        /// <summary>
        /// 变形
        /// </summary>
        /// <returns>变形后的矩阵</returns>
        public abstract int[,] GetRotate();

        /// <summary>
        /// 形状的颜色
        /// </summary>
        public abstract Color Color { get; }

        /// <summary>
        /// 获取下一个形状的索引。如果超过最大索引则返回最初索引
        /// </summary>
        /// <returns></returns>
        public int GetNextIndex()
        {
            int nextIndex = _index >= MaxIndex ? 0 : _index + 1;

            return nextIndex;
        }

        /// <summary>
        /// 变形。设置 Matrix 为变形后的矩阵
        /// </summary>
        public void Rotate()
        {
            Matrix = GetRotate();

            _index = GetNextIndex();
        }
    }
}
