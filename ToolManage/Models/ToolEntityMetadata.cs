using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToolManage.Models
{
    public partial class ToolEntity
    {
        public string StateString
        {
            get
            {
                switch (State)
                {
                    case "9":
                        return "已删除";
                    case "0":
                        return "在库";
                    case "1":
                        return "借出";
                    case "2":
                        return "维修中";
                    case "3":
                        return "已报废";
                    default:
                        return "错误";
                
                }
            }
        }

    }
}