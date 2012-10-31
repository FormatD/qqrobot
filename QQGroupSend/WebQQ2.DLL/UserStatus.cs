using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebQQ2.DLL
{
    /// <summary>
    /// 2011-1-5
    /// by hackren
    /// Email:hackren@vip.qq.com
    /// </summary>
    public enum UserStatus
    {
        /// <summary>
        ///链接异常
        /// </summary>
        ConnnectionUnCommon,
        /// <summary>
        /// 密码错误
        /// </summary>
        BadPassword,
        /// <summary>
        /// 验证码错误
        /// </summary>
        BadVerifyCode,
        /// <summary>
        /// 登陆成功
        /// </summary>
        Logined,
        /// <summary>
        /// 没有开通空间
        /// </summary>
        NoOpenQzone,
        /// <summary>
        /// 开通空间成功
        /// </summary>
        OpenQzoneTrue,
        /// <summary>
        /// 其他错误
        /// </summary>
        ErrorOther,
        /// <summary>
        /// 登陆失败
        /// </summary>
        LoginFault,
        /// <summary>
        /// 验证码为空
        /// </summary>
        EmptyVerifyCode,
        /// <summary>
        /// 页面过期
        /// </summary>
        PageOutdated,
        /// <summary>
        /// 转载日志的时候出错，没有开通空间
        /// </summary>
        reprintedBlogIsNoOpenBlog,
        /// <summary>
        /// 转载日志成功
        /// </summary>
        reprintedBlogTrue,
        /// <summary>
        /// 转载日子错误
        /// </summary>
        reprintedBlogError,
        Frezen
    }
}
