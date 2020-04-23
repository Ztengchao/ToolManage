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


        public class ScrapDocMetadata
        {
        }
    }
}