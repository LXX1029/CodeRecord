//------------------------------------------------------------------------------
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
    using System.Collections.Generic;

    public partial class DevelopRecord
    {
        public int RecordId { get; set; }
        public Nullable<int> TypeId { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Desc { get; set; }
        public Nullable<int> ClickCount { get; set; } = 0;
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<System.DateTime> UpdatedTime { get; set; }
        public byte[] Picture { get; set; }
        public byte[] Zip { get; set; }
        public Nullable<int> UserId { get; set; }

        public virtual DevelopType DevelopType { get; set; }
    }
}