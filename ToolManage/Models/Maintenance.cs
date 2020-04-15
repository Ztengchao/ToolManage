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
    
    public partial class Maintenance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Maintenance()
        {
            this.MaintenanceDetail = new HashSet<MaintenanceDetail>();
        }
    
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int ToolEntityId { get; set; }
        public System.DateTime Date { get; set; }
        public int CheckTypeId { get; set; }
        public string Remark { get; set; }
    
        public virtual Account Account { get; set; }
        public virtual CheckType CheckType { get; set; }
        public virtual ToolEntity ToolEntity { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MaintenanceDetail> MaintenanceDetail { get; set; }
    }
}
