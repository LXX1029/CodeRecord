using DevExpress.XtraEditors.Controls;

namespace DLCodeRecord.CustomerLocalizer
{
    public class EditorsLocalizer : Localizer
    {
        public override string Language
        {
            get
            {
                return "zh-chs";
            }
        }

        public override string GetLocalizedString(StringId id)
        {
            switch (id)
            {
                case StringId.OK:
                    return "确定";

                case StringId.Cancel:
                    return "关闭";

                case StringId.XtraMessageBoxOkButtonText:
                    return "确定";

                case StringId.XtraMessageBoxCancelButtonText:
                    return "取消";

                case StringId.DateEditToday:
                    {
                        return "今天";
                    }
                case StringId.DateEditClear:
                    {
                        return "清除";
                    }
                case StringId.PictureEditSaveFileFilter:
                    {
                        return "Bitmap Files (*.bmp)|*.bmp|Graphics Interchange Format (*.gif)|*.gif|JPEG File Interchange Format (*.jpg)|*.jpg";
                    }
                case StringId.PictureEditMenuSave: return "另存为";
                case StringId.PictureEditMenuCopy: return "复制";
                case StringId.PictureEditSaveFileTitle: return "另存为";
                case StringId.PictureEditMenuPaste: return "粘贴";
                case StringId.PictureEditMenuCut: return "剪切";
                case StringId.PictureEditMenuDelete: return "删除";
                case StringId.PictureEditMenuLoad: return "打开";
                case StringId.PictureEditMenuZoom: return "缩放";
                case StringId.PictureEditMenuFullSize: return "原始尺寸";
                case StringId.PictureEditMenuFitImage: return "图像自适";
                case StringId.PictureEditMenuZoomIn: return "放大";
                case StringId.PictureEditMenuZoomOut: return "缩小";
                case StringId.PictureEditMenuZoomTo: return "手动调节";
                case StringId.TextEditMenuCopy: return "复制";
                case StringId.TextEditMenuCut: return "剪切";
                case StringId.TextEditMenuDelete: return "删除";
                case StringId.TextEditMenuPaste: return "粘贴";
                case StringId.TextEditMenuSelectAll: return "全选";
                case StringId.TextEditMenuUndo: return "撤销";
            }

            return string.Empty;
        }
    }
}