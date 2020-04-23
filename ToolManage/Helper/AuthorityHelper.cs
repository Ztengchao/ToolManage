using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                case AuthorityType.MaintanceList:
                    return "检修列表";
                case AuthorityType.RepairList:
                    return "维修列表";
                case AuthorityType.MaintanceManage:
                    return "检修管理";
                case AuthorityType.ScrapDocManage:
                    return "报废单管理";
                default:
                    return "";
            }
        }

        public static ActionResult ToActionResult(this AuthorityType authorityType)
        {
            switch (authorityType)
            {
                case AuthorityType.Authority:
                    return new RedirectResult("Authority/Index");
                case AuthorityType.Workcell:
                    return new RedirectResult("Department/Index");
                case AuthorityType.User:
                    return new RedirectResult("User/Index");
                case AuthorityType.RepairManage:
                    return new RedirectResult("Tool/Repair");
                case AuthorityType.ToolBorrow:
                    return new RedirectResult("Tool/Borrow");
                case AuthorityType.ToolInput:
                    return new RedirectResult("Tool/Create");
                case AuthorityType.ToolList:
                    return new RedirectResult("Tool/Index");
                case AuthorityType.MaintanceList:
                    return new RedirectResult("Maintance/Index");
                case AuthorityType.RepairList:
                    return new RedirectResult("Repair/Index");
                case AuthorityType.MaintanceManage:
                    return new RedirectResult("Maintance/Type");
                case AuthorityType.ScrapDocManage:
                    return new RedirectResult("Scrap/Index");
                default:
                    return new EmptyResult();
            }
        }

        /// <summary>
        /// 所有权限类型
        /// </summary>
        public static int AuthorityCount => 11;

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
        /// 权限管理
        /// </summary>
        Authority,
        /// <summary>
        /// 部门管理
        /// </summary>
        Workcell,
        /// <summary>
        /// 用户管理
        /// </summary>
        User,
        /// <summary>
        /// 夹具列表
        /// </summary>
        ToolList,
        /// <summary>
        /// 夹具录入
        /// </summary>
        ToolInput,
        /// <summary>
        /// 夹具借用
        /// </summary>
        ToolBorrow,
        /// <summary>
        /// 报修管理
        /// </summary>
        RepairManage,
        /// <summary>
        /// 检修列表
        /// </summary>
        MaintanceList,
        /// <summary>
        /// 维修列表
        /// </summary>
        RepairList,
        /// <summary>
        /// 检修管理
        /// </summary>
        MaintanceManage,
        /// <summary>
        /// 报废单管理
        /// </summary>
        ScrapDocManage,
    }
}