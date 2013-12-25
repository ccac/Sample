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

using YYTetris.Piece;
using System.Windows.Threading;
using System.Collections.Generic;
using System.ComponentModel;

namespace YYTetris
{
    public class UIControl : INotifyPropertyChanged
    {
        /// <summary>
        /// 俄罗斯方块容器
        /// </summary>
        public Block[,] Container { get; set; }

        /// <summary>
        /// 下一个形状的容器（4×4）
        /// </summary>
        public Block[,] NextContainer { get; set; }

        /// <summary>
        /// 游戏状态（Ready, Play, Pause, Over）
        /// </summary>
        public GameStatus GameStatus { get; set; }

        private int _rows = 20; // 行数（Y 方向）
        private int _columns = 10; // 列数（X 方向）
        private int _positionX = 3; // 形状所属的 4×4 容器的 X 坐标
        private int _positionY = 0; // 形状所属的 4×4 容器的 Y 坐标

        private List<PieceBase> _pieces; // 形状集合

        private PieceBase _currentPiece; // 当前形状
        private PieceBase _nextPiece; // 下一个形状

        private int _initSpeed = 400; // 初始速率（毫秒）
        private int _levelSpeed = 50; // 每增加一个级别所需增加的速率（毫秒）

        private DispatcherTimer _timer;

        /// <summary>
        /// 构造函数
        /// </summary>
        public UIControl()
        {
            // 初始化形状集合，共七种形状
            _pieces = new List<PieceBase>() { new I(), new L(), new L2(), new N(), new N2(), new O(), new T() };

            // 初始化方块容器（用 Block 对象填满整个容器）
            Container = new Block[_rows, _columns];
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    var block = new Block();
                    block.Top = i * block.rectangle.ActualHeight;
                    block.Left = j * block.rectangle.ActualWidth;
                    block.Color = null;

                    Container[i, j] = block;
                }
            }

            // 初始化下一个形状的容器（用 Block 对象将其填满）
            NextContainer = new Block[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    var block = new Block();
                    block.Top = i * block.rectangle.ActualHeight;
                    block.Left = j * block.rectangle.ActualWidth;
                    block.Color = null;

                    NextContainer[i, j] = block;
                }
            }

            // 创建一个新的形状
            CreatePiece();
            // 呈现当前创建出的形状
            AddPiece(0, 0);

            // Timer 用于定时向下移动形状
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(_initSpeed);
            _timer.Tick += new EventHandler(_timer_Tick);

            GameStatus = GameStatus.Ready;
        }

        /// <summary>
        /// 开始游戏（启动计时器）
        /// </summary>
        public void Play()
        {
            GameStatus = GameStatus.Play;
            _timer.Start();
        }

        /// <summary>
        /// 暂停游戏（停止计时器）
        /// </summary>
        public void Pause()
        {
            GameStatus = GameStatus.Pause;
            _timer.Stop();
        }

        /// <summary>
        /// 创建一个新的形状
        /// </summary>
        private void CreatePiece()
        {
            // 逻辑移到 下坠后 的逻辑内
            for (int x = 0; x < _columns; x++)
            {
                if (Container[0, x].Color != null)
                {
                    OnGameOver(null);
                    break;
                }
            }

            // 计算 当前形状 和 下一个形状
            Random random = new Random();
            _currentPiece = _nextPiece == null ? _pieces[random.Next(0, 7)] : _nextPiece;
            _nextPiece = _pieces[random.Next(0, 7)];

            // 形状所属的 4×4 容器的 X 坐标和 Y 坐标
            _positionX = 3;
            _positionY = 0;

            // 设置“下一个形状的容器”的 UI
            SetNextContainerUI();
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            MoveToDown();
        }

        /// <summary>
        /// 向左移动
        /// </summary>
        public void MoveToLeft()
        {
            if (GameStatus != GameStatus.Play) return;

            if (!IsBoundary(_currentPiece.Matrix, -1, 0))
            {
                RemovePiece();
                AddPiece(-1, 0);
            }
        }

        /// <summary>
        /// 向右移动
        /// </summary>
        public void MoveToRight()
        {
            if (GameStatus != GameStatus.Play) return;

            if (!IsBoundary(_currentPiece.Matrix, 1, 0))
            {
                RemovePiece();
                AddPiece(1, 0);
            }
        }

        /// <summary>
        /// 向下移动
        /// </summary>
        public void MoveToDown()
        {
            if (GameStatus != GameStatus.Play) return;

            if (!IsBoundary(_currentPiece.Matrix, 0, 1))
            {
                RemovePiece();
                AddPiece(0, 1);
            }
            else
            {
                // 如果触及底边了，则消除可消的行并且创建新的形状
                RemoveRow();
                CreatePiece();

                // 每落下一个形状加 1 分
                Score++;
            }
        }

        /// <summary>
        /// 变形
        /// </summary>
        public void Rotate()
        {
            if (GameStatus != GameStatus.Play) return;

            if (!IsBoundary(_currentPiece.GetRotate(), 0, 0))
            {
                RemovePiece();
                _currentPiece.Rotate();
                AddPiece(0, 0);
            }
        }

        /// <summary>
        /// 清除俄罗斯方块容器
        /// </summary>
        public void Clear()
        {
            for (int x = 0; x < _columns; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    Container[y, x].Color = null;
                }
            }
        }

        /// <summary>
        /// 边界判断（是否超过边界）
        /// </summary>
        /// <param name="matrix">当前操作的形状的4×4矩阵</param>
        /// <param name="offsetX">矩阵 X 方向的偏移量</param>
        /// <param name="offsetY">矩阵 Y 方向的偏移量</param>
        /// <returns></returns>
        private bool IsBoundary(int[,] matrix, int offsetX, int offsetY)
        {
            RemovePiece();

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (matrix[i, j] == 1)
                    {
                        if (j + _positionX + offsetX > _columns - 1 // 超过列的右边界
                            || i + _positionY + offsetY > _rows - 1 // 超过行的下边界
                            || j + _positionX + offsetX < 0 // 超过列的左边界
                            || Container[i + _positionY + offsetY, j + _positionX + offsetX].Color != null) // matrix 所需偏移的地方已经有 Block 占着了
                        {
                            AddPiece(0, 0);
                            return true;
                        }                       
                    }
                }
            }

            AddPiece(0, 0);
            return false;
        }

        /// <summary>
        /// 设置“下一个形状的容器”的 UI
        /// </summary>
        private void SetNextContainerUI()
        {
            // 清空
            foreach (Block block in NextContainer)
            {
                block.Color = null;
            }

            // 根据 _nextPiece 的矩阵设置相对应的 Block 对象的呈现
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (_nextPiece.Matrix[x, y] == 1)
                    {
                        NextContainer[x, y].Color = _nextPiece.Color;
                    }
                }
            }
        }

        /// <summary>
        /// 移除 _currentPiece 在界面上的呈现
        /// </summary>
        private void RemovePiece()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_currentPiece.Matrix[i, j] == 1)
                    {
                        Container[i + _positionY, j + _positionX].Color = null;
                    }
                }
            }
        }

        /// <summary>
        /// 增加 _currentPiece 在界面上的呈现
        /// </summary>
        /// <param name="offsetX">X 方向上的偏移量</param>
        /// <param name="offsetY">Y 方向上的偏移量</param>
        private void AddPiece(int offsetX, int offsetY)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (_currentPiece.Matrix[i, j] == 1)
                    {
                        Container[i + _positionY + offsetY, j + _positionX + offsetX].Color = _currentPiece.Color;
                    }
                }
            }

            _positionX += offsetX;
            _positionY += offsetY;
        }

        /// <summary>
        /// 根据游戏规则，如果某行出现连续的直线则将其删除，该线以上的部分依次向下移动
        /// </summary>
        private void RemoveRow()
        {
            // 删除的行数
            int removeRowCount = 0;

            // 行的遍历（Y 方向）
            for (int y = 0; y < _rows; y++)
            {
                // 该行是否是一条连续的直线
                bool isLine = true;

                // 列的遍历（X 方向）
                for (int x = 0; x < _columns; x++)
                {
                    if (Container[y, x].Color == null)
                    {
                        // 出现断行，则继续遍历下一行
                        isLine = false;
                        break;
                    }
                }

                // 该行是一条连续的直线则将其删除，并将该行以上的部分依次向下移动
                if (isLine)
                {
                    removeRowCount++;

                    // 删除该行
                    for (int x = 0; x < _columns; x++)
                    {
                        Container[y, x].Color = null;
                    }

                    // 将被删除行的以上行依次向下移动
                    for (int i = y; i > 0; i--)
                    {
                        for (int x = 0; x < _columns; x++)
                        {
                            Container[i, x].Color = Container[i - 1, x].Color;
                        }
                    }
                }
            }

            // 加分，计算方法： 2 的 removeRowCount 次幂 乘以 10
            if (removeRowCount > 0)
                Score += 10 * (int)Math.Pow(2, removeRowCount);                

            // 更新总的已消行数
            RemoveRowCount += removeRowCount;

            // 根据已消行数计算级别，依据丁学的建议，计算方法： 已消行数/5 的平方根 取整
            Level = (int)Math.Sqrt(RemoveRowCount / 5);

            // 根据级别计算速率，计算方法： 初始速率 减 （每多一个级别所需增加的速率 乘以 当前级别）
            _timer.Interval = TimeSpan.FromMilliseconds(_initSpeed - _levelSpeed * Level > _levelSpeed ? _initSpeed - _levelSpeed * Level : _levelSpeed);
        }

        private int _score = 0;
        /// <summary>
        /// 得分
        /// </summary>
        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Score"));
                }
            }
        }

        private int _removeRowCount = 0;
        /// <summary>
        /// 总共被消除的行数
        /// </summary>
        public int RemoveRowCount
        {
            get { return _removeRowCount; }
            set
            {
                _removeRowCount = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RemoveRowCount"));
                }
            }
        }

        private int _level = 0;
        /// <summary>
        /// 级别（游戏难度）
        /// </summary>
        public int Level
        {
            get { return _level; }
            set
            {
                _level = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Level"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 游戏结束的事件委托
        /// </summary>
        public event EventHandler GameOver;

        /// <summary>
        /// 游戏结束后所调用的方法，并触发游戏结束事件
        /// </summary>
        /// <param name="e"></param>
        private void OnGameOver(EventArgs e)
        {
            GameStatus = GameStatus.Over;
            _timer.Interval = TimeSpan.FromMilliseconds(_initSpeed);
            _timer.Stop();

            EventHandler handler = GameOver;
            if (handler != null)
                handler(this, e);
        }
    }
}
