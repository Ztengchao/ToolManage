using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToolManage.Models;

namespace ToolManage.Helper
{
    public static class AuthorityHelper
    {
        /// <summary>
        /// 权限类型是否具有某个权限
        /// </summary>
        /// <param name="authority"></param>
        /// <param name="authorityType"></param>
        /// <returns></returns>
        public static bool HasAuthority(this Authority authority, AuthorityType authorityType)
        {
            // 权限类型为long
            // 与 0...010(权限等级个0)0做与，不为0则表示具有权限
            return ((ulong)authority.Data & (1uL << (int)authorityType)) != 0;
        }

        /// <summary>
        /// 为权限类型添加权限
        /// </summary>
        /// <param name="authority"></param>
        /// <param name="authorityType"></param>
        public static void AddAuthority(this Authority authority, AuthorityType authorityType)
            => authority.Data = (long)((ulong)authority.Data | 1uL << (int)authorityType);

        /// <summary>
        /// 获取一个权限类型下的所有权限
        /// </summary>
        /// <param name="authority"></param>
        /// <returns></returns>
        public static List<AuthorityType> AllAuthority(this Authority authority)
        {
            var list = new List<AuthorityType>();
            for (var i = 0; i < AuthorityCount; i++)
            {
                if (authority.HasAuthority((AuthorityType)i))
                {
                    list.Add((AuthorityType)i);
                }
            }
            return list;
        }

        public static string ToMyString(this AuthorityType authorityType)
        {
            switch (authorityType)
            {
                case AuthorityType.Authority:
                    return "权限管理";
                case AuthorityType.Workcell:
                    return "部门管理";
                case AuthorityType.User:
                    return "用户管理";
                case AuthorityType.RepairManage:
                    return "报修管理";
                case AuthorityType.ToolBorrow:
                    return "夹具借用";
                case AuthorityType.ToolInput:
                    return "夹具录入";
                case AuthorityType.ToolList:
                    return "夹具列表";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 所有权限类型
        /// </summary>
        public static int AuthorityCount => 7;

        public static List<AuthorityTypeString> AllAuthorityType()
        {
            var list = new List<AuthorityTypeString>();
            for(var i = 0; i < AuthorityCount; i++)
            {
                list.Add(new AuthorityTypeString { AuthorityType = (AuthorityType)i });
            }
            return list;
        }

        public static List<AuthorityTypeString> TypesAuthorityNotHave(this Authority authority)
        {
            var list = new List<AuthorityTypeString>();
            for(var i = 0; i < AuthorityCount; i++)
            {
                if (!authority.HasAuthority((AuthorityType)i))
                {
                    list.Add(new AuthorityTypeString { AuthorityType = (AuthorityType)i });
                }
            }
            return list;
        }
    }

    public class AuthorityTypeString
    {
        public int Id => (int)AuthorityType;
        public string Name => AuthorityType.ToMyString();
        public AuthorityType AuthorityType { get; set; }
    }

    /// <summary>
    /// 权限类型
    /// </summary>
    public enum AuthorityType
    {
        /// <summary>
        /// 权限管理 权限
        /// </summary>
        Authority,
        /// <summary>
        /// 部门管理 权限
        /// </summary>
        Workcell,
        /// <summary>
        /// 用户管理 权限
        /// </summary>
        User,
        /// <summary>
        /// 夹具列表 权限
        /// </summary>
        ToolList,
        /// <summary>
        /// 夹具录入 权限
        /// </summary>
        ToolInput,
        /// <summary>
        /// 夹具借用 权限
        /// </summary>
        ToolBorrow,
        /// <summary>
        /// 报修管理
        /// </summary>
        RepairManage
    }
}