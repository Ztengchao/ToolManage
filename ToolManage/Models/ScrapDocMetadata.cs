using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToolManage.Models
{
    public partial class ScrapDoc
    {
        public ScrapDoc(ScrapDoc doc)
        {
            CreateDate = doc.CreateDate;
            Id = doc.Id;
            Name = doc.Name;
            Remark = doc.Remark;
        }

        public string StateString
        {
            get
            {
                switch (State)
                {
                    case "1":
                        return "未处理";
                    case "2":
                        return "已通过";
                    case "3":
                        return "已拒绝";
                    default:
                        return "错误";
                }
            }
        }

        public class ScrapDocMetadata
        {
        }
    }
}