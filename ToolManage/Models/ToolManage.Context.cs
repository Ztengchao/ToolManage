﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ToolManageDataContext : DbContext
    {
        public ToolManageDataContext()
            : base("name=ToolManageDataContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Authority> Authority { get; set; }
        public virtual DbSet<ChangeLog> ChangeLog { get; set; }
        public virtual DbSet<CheckDetail> CheckDetail { get; set; }
        public virtual DbSet<CheckType> CheckType { get; set; }
        public virtual DbSet<ConsumeReturn> ConsumeReturn { get; set; }
        public virtual DbSet<Inner> Inner { get; set; }
        public virtual DbSet<Maintenance> Maintenance { get; set; }
        public virtual DbSet<MaintenanceDetail> MaintenanceDetail { get; set; }
        public virtual DbSet<RepairApplication> RepairApplication { get; set; }
        public virtual DbSet<ScrapApplication> ScrapApplication { get; set; }
        public virtual DbSet<ToolDef> ToolDef { get; set; }
        public virtual DbSet<ToolEntity> ToolEntity { get; set; }
        public virtual DbSet<WorkCell> WorkCell { get; set; }
        public virtual DbSet<ScrapDoc> ScrapDoc { get; set; }
    }
}
