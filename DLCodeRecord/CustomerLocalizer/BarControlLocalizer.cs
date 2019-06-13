using DevExpress.XtraBars.Localization;

namespace DLCodeRecord.CustomerLocalizer
{
    public class BarControlLocalizer : DevExpress.XtraBars.Localization.BarLocalizer
    {
        public override string Language
        {
            get
            {
                return "zh-chs";
            }
        }

        public override string GetLocalizedString(DevExpress.XtraBars.Localization.BarString id)
        {
            switch (id)
            {
                case BarString.AddOrRemove: return "添加或删除按钮(&A)";
                case BarString.ResetBar: return "确定要对 '{0}' 工具栏所做的改动进行重置吗？";
                case BarString.ResetBarCaption: return "自定义";
                case BarString.ResetButton: return "重设工具栏(&R)";
                case BarString.CustomizeButton: return "自定义...(&C)";
                case BarString.ToolBarMenu: return "重新设定(&R)$刪除(&D)$!重新命名(&N)$!默认格式(&L)$全文字模式(&T)$文字菜单(&O)$图片及文字(&A)$!启用组(&G)$可见的(&V)$最近使用的(&M)";
                case BarString.NewToolbarName: return "工具";
                case BarString.NewMenuName: return "主菜单";
                case BarString.NewStatusBarName: return "状态栏";
                case BarString.NewToolbarCustomNameFormat: return "自定义{0}";
                case BarString.NewToolbarCaption: return "新建工具栏";
                case BarString.RenameToolbarCaption: return "重命名工具栏";
                case BarString.CustomizeWindowCaption: return "自定义";
                case BarString.MenuAnimationSystem: return "(系统默认值)";
                case BarString.MenuAnimationNone: return "无";
                case BarString.MenuAnimationSlide: return "片";
                case BarString.MenuAnimationFade: return "减弱";
                case BarString.MenuAnimationUnfold: return "展开";
                case BarString.MenuAnimationRandom: return "随机";
                case BarString.PopupMenuEditor: return "弹出菜单编辑器";
                case BarString.ToolbarNameCaption: return "工具栏名称(&T)";
                case BarString.RibbonToolbarBelow: return "将快速访问工具栏显示在功能区下方(&S)";
                case BarString.RibbonToolbarAbove: return "将快速访问工具栏显示在功能区上方(&S)";
                case BarString.RibbonToolbarRemove: return "移除快速访问工具栏(&R)";
                case BarString.RibbonToolbarAdd: return "添加快速访问工具栏(&A)";
                case BarString.RibbonToolbarMinimizeRibbon: return "最小化功能区(&N)";
                case BarString.RibbonGalleryFilter: return "所有组";
                case BarString.RibbonGalleryFilterNone: return "无";
                case BarString.BarUnassignedItems: return "(未设定项)";
                case BarString.BarAllItems: return "(所有项)";
                case BarString.RibbonUnassignedPages: return "(未设定页)";
                case BarString.RibbonAllPages: return "(所有页)";
            }
            System.Diagnostics.Debug.WriteLine(id.ToString() + "的默认值(" + this.GetType().ToString() + ")=" + base.GetLocalizedString(id));

            return base.GetLocalizedString(id);
        }
    }
}