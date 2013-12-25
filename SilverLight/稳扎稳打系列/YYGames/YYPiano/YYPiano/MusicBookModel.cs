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

namespace YYPiano.Controls
{
    /// <summary>
    /// 乐谱实体类，用于描述乐谱的每一音阶
    /// </summary>
    public class MusicBookModel
    {
        /// <summary>
        /// 键值 Key 所对应的播放时长。单位：毫秒
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 键值，即音阶。A键 对应 C 大调的 dou，以此类推
        /// </summary>
        public Key Key { get; set; }
    }
}
