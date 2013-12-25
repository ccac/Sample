using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Collections.Generic;
using System.Text;

using System.Security.Cryptography;
using System.IO;

/// <summary>
/// 提供 WCF 服务的类
/// </summary>
[ServiceContract]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
public class WCFService
{
    /// <summary>
    /// 返回指定的 User 对象（用于演示 Silverlight 调用 WCF 服务）
    /// </summary>
    /// <param name="name">名字</param>
    /// <returns></returns>
    [OperationContract]
    public User GetUser(string name)
    {
        return new User { Name = name, DayOfBirth = new DateTime(1980, 2, 14) };
    }

    /// <summary>
    /// 返回指定的 User 对象（用于演示传输信息的加密/解密）
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    [OperationContract]
    public User GetUserByCryptography(string name)
    {
        return new User { Name = Decrypt(name), DayOfBirth = new DateTime(1980, 2, 14) };
    }

    /// <summary>
    /// 解密数据
    /// </summary>
    /// <param name="input">加密后的字符串</param>
    /// <returns>加密前的字符串</returns>
    public string Decrypt(string input)
    {
        // 盐值（与加密时设置的值一致）
        string saltValue = "saltValue";
        // 密码值（与加密时设置的值一致）
        string pwdValue = "pwdValue";

        byte[] encryptBytes = Convert.FromBase64String(input);
        byte[] salt = Encoding.UTF8.GetBytes(saltValue);

        AesManaged aes = new AesManaged();

        Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(pwdValue, salt);

        aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
        aes.KeySize = aes.LegalKeySizes[0].MaxSize;
        aes.Key = rfc.GetBytes(aes.KeySize / 8);
        aes.IV = rfc.GetBytes(aes.BlockSize / 8);

        // 用当前的 Key 属性和初始化向量 IV 创建对称解密器对象
        ICryptoTransform decryptTransform = aes.CreateDecryptor();

        // 解密后的输出流
        MemoryStream decryptStream = new MemoryStream();

        // 将解密后的目标流（decryptStream）与解密转换（decryptTransform）相连接
        CryptoStream decryptor = new CryptoStream(decryptStream, decryptTransform, CryptoStreamMode.Write);

        // 将一个字节序列写入当前 CryptoStream （完成解密的过程）
        decryptor.Write(encryptBytes, 0, encryptBytes.Length);
        decryptor.Close();

        // 将解密后所得到的流转换为字符串
        byte[] decryptBytes = decryptStream.ToArray();
        string decryptedString = UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);

        return decryptedString;
    }
}
