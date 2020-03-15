//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToolManage.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class InOutStock
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public int RecordId { get; set; }
        public int BringId { get; set; }
        public bool InOut { get; set; }
        public Nullable<int> LineId { get; set; }
        public int ToolEntityId { get; set; }
        public string Location { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual Inner Inner { get; set; }
        public virtual ToolEntity ToolEntity { get; set; }
    }
}
