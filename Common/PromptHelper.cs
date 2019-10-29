using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLCodeRecord
{
    public sealed class PromptHelper
    {
        #region 公共提示

        /// <summary>
        /// 添加成功
        /// </summary>
        public static string D_ADD_SUCCESS { get; } = "添加成功";
        /// <summary>
        /// 添加失败
        /// </summary>
        public static string D_ADD_FAIL { get; } = $"添加失败:{0}";
        /// <summary>
        /// 更新成功
        /// </summary>
        public static string D_UPDATE_SUCCESS { get; } = "更新成功";
        /// <summary>
        /// 更新失败
        /// </summary>
        public static string D_UPDATE_FAIL { get; } = $"更新失败:{0}";
        /// <summary>
        /// 确定删除
        /// </summary>
        public static string D_DELETE_CONFIRM { get; } = "确定删除?";
        /// <summary>
        /// 删除成功
        /// </summary>
        public static string D_DELETE_SUCCESS { get; } = "删除成功";
        /// <summary>
        /// 删除失败
        /// </summary>
        public static string D_DELETE_FAIL { get; } = $"删除失败:{0}";
        /// <summary>
        /// 确定退出
        /// </summary>
        public static string D_EXIST_CONFIRM { get; } = "确定退出?";

        /// <summary>
        /// 请选择要操作的数据行
        /// </summary>
        public static string D_SELECT_DATAROW { get; } = "请选择要操作的数据行";

        /// <summary>
        /// 正在生成数据
        /// </summary>
        public static string D_LOADINGDATA { get; } = "正在生成数据";

        /// <summary>
        /// 正在加载图片
        /// </summary>
        public static string D_LOADINGIMG { get; } = "正在加载图片";
        #endregion

        #region 主窗体
        /// <summary>
        /// 暂无权限
        /// </summary>
        public static string D_UnAuthority { get; } = "暂无权限";

        /// <summary>
        /// 请选择要打印的数据行
        /// </summary>
        public static string D_SELECT_PRINTEDDATAROW { get; } = "请选择要打印的数据行";

        /// <summary>
        /// 暂未提供可下载的文件包
        /// </summary>
        public static string D_NOPACKAGE { get; } = "暂未提供可下载的文件包";
        /// <summary>
        /// 正在获取文件包
        /// </summary>
        public static string D_DOWNLOADINGPACKAGE { get; } = "正在获取文件包...";
        /// <summary>
        /// 获取成功
        /// </summary>
        public static string D_DOWNLOADING_SUCCESS { get; } = "获取成功";

        /// <summary>
        /// 暂无数据
        /// </summary>
        public static string D_NODATA { get; } = "暂无数据";

        #endregion

        /// <summary>
        /// 设置成功
        /// </summary>
        public static string D_SETTING_SUCCESS { get; } = "设置成功";
        /// <summary>
        /// 设置失败
        /// </summary>
        public static string D_SETTING_FAIL { get; } = $"设置失败:{0}";
    }
}
