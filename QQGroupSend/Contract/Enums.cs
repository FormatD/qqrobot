using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WebQQ2.DLL
{
    /// <summary>
    /// 2011-1-5
    /// by hackren
    /// Email:hackren@vip.qq.com
    /// </summary>
    public enum OnlineStatusEnum
    {
        [Description("Q我吧")]
        Callme,
        [Description( "在线")]
        Online,
        [Description( "离开")]
        Away,
        [Description("忙碌")]
        Busy,
        [Description("静音")]
        Silent,
        [Description("隐身")]
        Hidden,
        [Description("离线")]
        Offline
    }

    public enum ClientTypeEnum
    {
        [Description("PC")]
        PC,
        [Description("手机QQ")]
        Phone,
        [Description("WebQQ")]
        WebQQ,
        [Description("未知客户端")]
        Unknow
    }

    public enum UserClassType
    {
        Online,
        Stranger,
        BlackList
    }

    public enum GroupType
    {
        CommonGroup,
        SeniorGroup,
        SuperGroup,
        ForbiddenGroup,
        EnterpriseGroup,
        ExpireSuperGroup
    }

    public enum LoginStatus
    {
        Unknown = -1,

        /// <summary>
        /// 登录成功 0
        /// </summary>
        Success = 0,
        /// <summary>
        /// 系统繁忙，请稍后再试 1
        /// </summary>
        Busy = 1,
        /// <summary>
        /// 已经过期的QQ号码 2,12
        /// </summary>
        ExpiredQQ = 2,
        /// <summary>
        /// 密码有误 3
        /// </summary>
        InvalidPassword = 3,
        /// <summary>
        /// 验证码有误 4
        /// </summary>
        InvalidVerifyCode = 4,
        /// <summary>
        /// 你的IP密码错误次数过多 8,16
        /// </summary>
        LimitedIP = 8,
        /// <summary>
        /// 账号不存在 9,10,11
        /// </summary>
        InvalidQQ = 9,
        /// <summary>
        /// 需使用邮箱登录 14
        /// </summary>
        EnableEmail = 14,
        /// <summary>
        /// 未验证的邮箱 18
        /// </summary>
        UnVerifiedEmail = 18,
        /// <summary>
        /// 暂时不能登录 19,20
        /// </summary>
        TempLocked
    }
}
