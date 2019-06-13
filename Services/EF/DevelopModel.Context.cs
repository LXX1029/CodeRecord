﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Services.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;

    public partial class RecordDBEntities : DbContext
    {
        public RecordDBEntities()
            : base("name=RecordDBEntities")
        {
            Configuration.AutoDetectChangesEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<DevelopFun> DevelopFuns { get; set; }
        public virtual DbSet<DevelopPowerFun> DevelopPowerFuns { get; set; }
        public virtual DbSet<DevelopRecord> DevelopRecords { get; set; }
        public virtual DbSet<DevelopType> DevelopTypes { get; set; }
        public virtual DbSet<DevelopUser> DevelopUsers { get; set; }
        public virtual DbSet<View_DevelopUserPowerFun> View_DevelopUserPowerFun { get; set; }

        public virtual ObjectResult<proc_ClickCountReport_Result> proc_ClickCountReport()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_ClickCountReport_Result>("proc_ClickCountReport");
        }

        public virtual int proc_DeleteDevelopType(Nullable<int> typeid)
        {
            var typeidParameter = typeid.HasValue ?
                new ObjectParameter("typeid", typeid) :
                new ObjectParameter("typeid", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("proc_DeleteDevelopType", typeidParameter);
        }

        public virtual ObjectResult<proc_GetDevelopRecord_Result> proc_GetDevelopRecord(Nullable<int> startId, Nullable<int> endId)
        {
            var startIdParameter = startId.HasValue ?
                new ObjectParameter("startId", startId) :
                new ObjectParameter("startId", typeof(int));

            var endIdParameter = endId.HasValue ?
                new ObjectParameter("endId", endId) :
                new ObjectParameter("endId", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_GetDevelopRecord_Result>("proc_GetDevelopRecord", startIdParameter, endIdParameter);
        }

        public virtual ObjectResult<proc_GetDevelopRecordPager_Result> proc_GetDevelopRecordPager(Nullable<int> pageIndex, Nullable<int> pageCount)
        {
            var pageIndexParameter = pageIndex.HasValue ?
                new ObjectParameter("pageIndex", pageIndex) :
                new ObjectParameter("pageIndex", typeof(int));

            var pageCountParameter = pageCount.HasValue ?
                new ObjectParameter("pageCount", pageCount) :
                new ObjectParameter("pageCount", typeof(int));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<proc_GetDevelopRecordPager_Result>("proc_GetDevelopRecordPager", pageIndexParameter, pageCountParameter);
        }

        public virtual ObjectResult<Nullable<int>> proc_SaveUserFun(Nullable<int> userId, Nullable<int> funId, Nullable<bool> isEnable)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("UserId", userId) :
                new ObjectParameter("UserId", typeof(int));

            var funIdParameter = funId.HasValue ?
                new ObjectParameter("FunId", funId) :
                new ObjectParameter("FunId", typeof(int));

            var isEnableParameter = isEnable.HasValue ?
                new ObjectParameter("IsEnable", isEnable) :
                new ObjectParameter("IsEnable", typeof(bool));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("proc_SaveUserFun", userIdParameter, funIdParameter, isEnableParameter);
        }

        public virtual int SP_Pagination(string tables, string primaryKey, string sort, Nullable<int> currentPage, Nullable<int> pageSize, string fields, string filter, string group)
        {
            var tablesParameter = tables != null ?
                new ObjectParameter("Tables", tables) :
                new ObjectParameter("Tables", typeof(string));

            var primaryKeyParameter = primaryKey != null ?
                new ObjectParameter("PrimaryKey", primaryKey) :
                new ObjectParameter("PrimaryKey", typeof(string));

            var sortParameter = sort != null ?
                new ObjectParameter("Sort", sort) :
                new ObjectParameter("Sort", typeof(string));

            var currentPageParameter = currentPage.HasValue ?
                new ObjectParameter("CurrentPage", currentPage) :
                new ObjectParameter("CurrentPage", typeof(int));

            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));

            var fieldsParameter = fields != null ?
                new ObjectParameter("Fields", fields) :
                new ObjectParameter("Fields", typeof(string));

            var filterParameter = filter != null ?
                new ObjectParameter("Filter", filter) :
                new ObjectParameter("Filter", typeof(string));

            var groupParameter = group != null ?
                new ObjectParameter("Group", group) :
                new ObjectParameter("Group", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_Pagination", tablesParameter, primaryKeyParameter, sortParameter, currentPageParameter, pageSizeParameter, fieldsParameter, filterParameter, groupParameter);
        }
    }
}
