using DevExpress.XtraGrid.Localization;

namespace DLCodeRecord.CustomerLocalizer
{
    public class GridControlLocalizer : GridLocalizer
    {
        public override string Language
        {
            get
            {
                return "zh-chs";
            }
        }

        public override string GetLocalizedString(GridStringId id)
        {
            string ret = string.Empty;

            switch (id)
            {
                case GridStringId.FileIsNotFoundError:
                    {
                        return "没有该查询数据";
                    }
                case GridStringId.FindControlFindButton:
                    return "查找";

                case GridStringId.FindControlClearButton:
                    return "清除";

                case GridStringId.FilterBuilderCaption:
                    return "数据筛选条件设定";

                case GridStringId.CustomFilterDialogEmptyValue:
                    return "空数据";

                case GridStringId.MenuColumnUnGroup: return "撤销分组";
                case GridStringId.MenuGroupPanelFullExpand: return "展开分组";
                case GridStringId.MenuGroupPanelFullCollapse: return "关闭分组";
                case GridStringId.MenuGroupPanelClearGrouping: return "撤销分组";
                case GridStringId.MenuGroupPanelShow: return "面板显示";
                case GridStringId.MenuGroupPanelHide: return "面板隐藏";
                case GridStringId.ColumnViewExceptionMessage: return " 要修正当前值吗?";
                case GridStringId.CustomizationCaption: return "自定义";
                case GridStringId.CustomizationColumns: return "列";
                case GridStringId.CustomizationBands: return "带宽";
                case GridStringId.PopupFilterAll: return "(全部)";
                case GridStringId.PopupFilterCustom: return "(自定义)";
                case GridStringId.PopupFilterBlanks: return "(空白)";
                case GridStringId.PopupFilterNonBlanks: return "(无空白)";
                case GridStringId.CustomFilterDialogFormCaption: return "用户自定义自动过滤器";
                case GridStringId.CustomFilterDialogCaption: return "显示符合下列条件的行:";
                case GridStringId.CustomFilterDialogRadioAnd: return "于(&A)";
                case GridStringId.CustomFilterDialogRadioOr: return "或(&O)";
                case GridStringId.CustomFilterDialogOkButton: return "确定(&O)";
                case GridStringId.CustomFilterDialogClearFilter: return "清除过滤器(&L)";
                case GridStringId.CustomFilterDialogCancelButton: return "取消(&C)";
                case GridStringId.CustomFilterDialog2FieldCheck: return "字段";
                case GridStringId.MenuFooterSum: return "和";
                case GridStringId.MenuFooterMin: return "最小值";
                case GridStringId.MenuFooterMax: return "最大值";
                case GridStringId.MenuFooterCount: return "计数";
                case GridStringId.MenuFooterAverage: return "平均值";
                case GridStringId.MenuFooterNone: return "无";
                case GridStringId.MenuFooterSumFormat: return "和={0:#.##}";
                case GridStringId.MenuFooterMinFormat: return "最小值={0}";
                case GridStringId.MenuFooterMaxFormat: return "最大值={0}";
                case GridStringId.MenuFooterCountFormat: return "{0}";
                case GridStringId.MenuFooterCountGroupFormat: return "计数={0}";
                case GridStringId.MenuFooterAverageFormat: return "平均={0:#.##}";
                case GridStringId.MenuFooterCustomFormat: return "统计值={0}";
                case GridStringId.MenuColumnSortAscending: return "升序排列";
                case GridStringId.MenuColumnSortDescending: return "降序排列";
                case GridStringId.MenuColumnRemoveColumn: return "移除该列";
                case GridStringId.MenuColumnShowColumn: return "显示列";
                case GridStringId.MenuColumnClearSorting: return "清除排序设置";
                case GridStringId.MenuColumnGroup: return "根据此列分组";
                case GridStringId.FilterPanelCustomizeButton: return "自定义";
                case GridStringId.MenuColumnColumnCustomization: return "列选择";
                case GridStringId.MenuColumnBestFit: return "最佳匹配";
                case GridStringId.MenuColumnFilter: return "允许筛选数据";
                case GridStringId.MenuColumnFilterEditor: return "设定数据筛选条件";
                case GridStringId.MenuColumnClearFilter: return "清除过滤器";
                case GridStringId.MenuColumnBestFitAllColumns: return "最佳匹配(所有列)";
                case GridStringId.PrintDesignerBandedView: return "打印设置 (Banded View)";
                case GridStringId.PrintDesignerGridView: return "打印设置(网格视图)";
                case GridStringId.PrintDesignerCardView: return "打印设置(卡视图)";
                case GridStringId.PrintDesignerBandHeader: return "起始带宽";
                case GridStringId.PrintDesignerDescription: return "为当前视图设置不同的打印选项";
                case GridStringId.MenuColumnGroupBox: return "分组依据框";
                case GridStringId.CardViewNewCard: return "新建卡";
                case GridStringId.CardViewQuickCustomizationButton: return "自定义";
                case GridStringId.CardViewQuickCustomizationButtonFilter: return "过滤器　";
                case GridStringId.CardViewQuickCustomizationButtonSort: return "排序方式:";
                case GridStringId.GridGroupPanelText: return "拖动列标题至此,根据该列分组";
                case GridStringId.GridNewRowText: return "在此处添加一行";
                case GridStringId.FilterBuilderOkButton: return "确定(&O)";
                case GridStringId.FilterBuilderCancelButton: return "取消(&C)";
                case GridStringId.FilterBuilderApplyButton: return "应用(&A)";
                case GridStringId.CustomFilterDialogHint: return "使用_查询任意单字符\r\n使用%查询多字符";
                default:
                    ret = base.GetLocalizedString(id);
                    break;
            }
            return ret;
        }
    }
}