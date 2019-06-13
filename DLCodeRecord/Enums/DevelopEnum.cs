using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLCodeRecord.DevelopForms
{
    /// <summary>
    /// 窗体数据状态枚举
    /// </summary>
    public enum DevelopActiveState
    {
        /// <summary>
        /// 无操作状态
        /// </summary>
        Normal,
        /// <summary>
        /// 处于新增数据状态
        /// </summary>
        Adding,
        /// <summary>
        /// 处于修改数据状态
        /// </summary>
        Updating,
    }


    /// <summary>
    /// 功能枚举
    /// </summary>
    public enum DevelopFunCaptions
    {
        DevelopUser = 5,
        DevelopSetting = 6,
        DevelopAdd = 7,
        DevelopUpdate = 8,
        DevelopDelete = 9,
        DevelopReport = 10,
        DevelopTypeAdd = 11,
        Print = 12,
        Exist = 500,
        ReLoadData = 501,
    }


}
