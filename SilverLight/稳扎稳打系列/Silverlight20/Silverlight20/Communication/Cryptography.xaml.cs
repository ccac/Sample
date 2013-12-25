using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using Silverlight20.WCFServiceReference;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Silverlight20.Communication
{
    public partial class Cryptography : UserControl
    {
        public Cryptography()
        {
            InitializeComponent();

            Demo();
        }

        void Demo()
        {
            WCFServiceClient client = new WCFServiceClient();

            client.GetUserByCryptographyCompleted+=new EventHandler<GetUserByCryptographyCompletedEventArgs>(client_GetUserByCryptographyCompleted);
            client.GetUserByCryptographyAsync(Encrypt("webabcd"));
        }

        void client_GetUserByCryptographyCompleted(object sender, GetUserByCryptographyCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                lblMsg.Text += e.Error.ToString() + "\r\n";
                return;
            }

            if (e.Cancelled != true)
            {
                lblMsg.Text += string.Format("姓名：{0}；生日：{1}\r\n",
                    e.Result.Name,
                    e.Result.DayOfBirth.ToString("yyyy-MM-dd"));
            }
        }

        /// <summary>
        /// 加密数据
        /// </summary>
        /// <param name="input">加密前的字符串</param>
        /// <returns>加密后的字符串</returns>
        private string Encrypt(string input)
        {
            // 盐值
            string saltValue = "saltValue";
            // 密码值
            string pwdValue = "pwdValue";

            byte[] data = UTF8Encoding.UTF8.GetBytes(input);
            byte[] salt = UTF8Encoding.UTF8.GetBytes(saltValue);

            // AesManaged - 高级加密标准(AES) 对称算法的管理类
            AesManaged aes = new AesManaged();

            // Rfc2898DeriveBytes - 通过使用基于 HMACSHA1 的伪随机数生成器，实现基于密码的密钥派生功能 (PBKDF2 - 一种基于密码的密钥派生函数)
            // 通过 密码 和 salt 派生密钥
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(pwdValue, salt);

            /*
             * AesManaged.BlockSize - 加密操作的块大小（单位：bit）
             * AesManaged.LegalBlockSizes - 对称算法支持的块大小（单位：bit）
             * AesManaged.KeySize - 对称算法的密钥大小（单位：bit）
             * AesManaged.LegalKeySizes - 对称算法支持的密钥大小（单位：bit）
             * AesManaged.Key - 对称算法的密钥
             * AesManaged.IV - 对称算法的密钥大小
             * Rfc2898DeriveBytes.GetBytes(int 需要生成的伪随机密钥字节数) - 生成密钥
             */

            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            aes.Key = rfc.GetBytes(aes.KeySize / 8);
            aes.IV = rfc.GetBytes(aes.BlockSize / 8);

            // 用当前的 Key 属性和初始化向量 IV 创建对称加密器对象
            ICryptoTransform encryptTransform = aes.CreateEncryptor();

            // 加密后的输出流
            MemoryStream encryptStream = new MemoryStream();

            // 将加密后的目标流（encryptStream）与加密转换（encryptTransform）相连接
            CryptoStream encryptor = new CryptoStream(encryptStream, encryptTransform, CryptoStreamMode.Write);

            // 将一个字节序列写入当前 CryptoStream （完成加密的过程）
            encryptor.Write(data, 0, data.Length);
            encryptor.Close();

            // 将加密后所得到的流转换成字节数组，再用Base64编码将其转换为字符串
            string encryptedString = Convert.ToBase64String(encryptStream.ToArray());

            return encryptedString;
        }        
    }
}
