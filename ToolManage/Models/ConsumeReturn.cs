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
    
    public partial class ConsumeReturn
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ToolEntityId { get; set; }
        public System.DateTime Date { get; set; }
        public bool BorrowReturn { get; set; }
        public string Remark { get; set; }
        public string BorrowLocation { get; set; }
        public Nullable<System.DateTime> ReturnDate { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual ToolEntity ToolEntity { get; set; }
    }
}
