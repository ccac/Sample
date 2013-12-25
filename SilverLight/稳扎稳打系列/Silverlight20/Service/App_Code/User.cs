using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

/// <summary>
/// User实体类
/// </summary>
[DataContract(Namespace="http://webabcd.cnblogs.com/")]
public class User
{
    /// <summary>
    /// 用户名
    /// </summary>
    [DataMember(Order = 0)]
    public string Name { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    [DataMember(Order = 1)]
    public DateTime DayOfBirth { get; set; }
}