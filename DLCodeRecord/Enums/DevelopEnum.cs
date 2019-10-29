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
        /// <summary>
        /// 用户管理
        /// </summary>
        DevelopUser = 5,
        /// <summary>
        /// 设置
        /// </summary>
        DevelopSetting = 6,
        /// <summary>
        /// 新增
        /// </summary>
        DevelopAdd = 7,
        /// <summary>
        /// 更新
        /// </summary>
        DevelopUpdate = 8,
        /// <summary>
        /// 删除
        /// </summary>
        DevelopDelete = 9,
        /// <summary>
        /// 报告
        /// </summary>
        DevelopReport = 10,
        /// <summary>
        /// 类型增加
        /// </summary>
        DevelopTypeAdd = 11,
        /// <summary>
        /// 打印
        /// </summary>
        Print = 12,
        /// <summary>
        /// 退出
        /// </summary>
        Exist = 500,
        /// <summary>
        /// 重载数据
        /// </summary>
        ReLoadData = 501,
    }
}
