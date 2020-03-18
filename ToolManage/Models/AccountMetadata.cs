using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToolManage.Models
{
    [MetadataType(typeof(AccountMetadata))]
    public partial class Account
    {
        public class AccountMetadata
        {
            [Required(ErrorMessage = "密码不能为空")]
            public string PassWord { get; set; }

            [Required(ErrorMessage = "用户名不能为空")]
            public string UserName { get; set; }
        }
    }
}