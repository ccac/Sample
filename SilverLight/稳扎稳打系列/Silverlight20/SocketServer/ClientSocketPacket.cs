using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocketServer
{
    /// <summary>
    /// 对客户端 Socket 及其他相关信息做一个封装
    /// </summary>
    public class ClientSocketPacket
    {
        /// <summary>
        /// 客户端 Socket
        /// </summary>
        public System.Net.Sockets.Socket Socket { get; set; }

        private byte[] _buffer;
        /// <summary>
        /// 为该客户端 Socket 开辟的缓冲区
        /// </summary>
        public byte[] Buffer
        {
            get
            {
                if (_buffer == null)
                    _buffer = new byte[32];

                return _buffer;
            }
        }

        private List<byte> _receivedByte;
        /// <summary>
        /// 客户端 Socket 发过来的信息的字节集合
        /// </summary>
        public List<byte> ReceivedByte
        {
            get
            {
                if (_receivedByte == null)
                    _receivedByte = new List<byte>();

                return _receivedByte;
            }
        }
    }
}
