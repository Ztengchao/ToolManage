using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToolManage.Models
{
    [MetadataType(typeof(WorkCellMetadata))]
    public partial class WorkCell
    {
        class WorkCellMetadata
        {
            [Required(ErrorMessage = "名称不能为空")]
            public string Name { get; set; }
        }
    }
}