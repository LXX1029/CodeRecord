using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLCodeRecord.DevelopForms
{
    /// <summary>
    /// 删除事件参数
    /// </summary>
    public class DeleteEventArgs : EventArgs
    {
        public int TypeId { get; set; }
    }
}
