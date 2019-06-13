using DevExpress.XtraRichEdit.Localization;

namespace DLCodeRecord.CustomerLocalizer
{
    public class RichEditControlLocalizer : DevExpress.XtraRichEdit.Localization.XtraRichEditLocalizer
    {
        public override string Language
        {
            get
            {
                return base.Language;
            }
        }

        public override string GetLocalizedString(XtraRichEditStringId id)
        {
            switch (id)
            {
                case XtraRichEditStringId.MenuCmd_CutSelection:
                    return "剪切";

                case XtraRichEditStringId.MenuCmd_CopySelection:
                    return "复制";

                case XtraRichEditStringId.MenuCmd_Delete:
                    return "删除";

                case XtraRichEditStringId.MenuCmd_Paste:
                    return "粘贴";

                case XtraRichEditStringId.MenuCmd_IncrementIndent:
                    return "增加缩进";

                case XtraRichEditStringId.MenuCmd_DecrementIndent:
                    return "减小缩进";

                case XtraRichEditStringId.MenuCmd_FontSubscript:
                    return "字体";

                case XtraRichEditStringId.MenuCmd_Bookmark:
                    return "水印";

                case XtraRichEditStringId.MenuCmd_Hyperlink:
                    return "创建链接";

                case XtraRichEditStringId.MenuCmd_HighlightText:
                    return "高亮";
                //case XtraRichEditStringId.menucmd_f:
                // return "高亮";
                case XtraRichEditStringId.Caption_ParagraphAlignment_Center:
                    return "居中";

                case XtraRichEditStringId.MenuCmd_ShowFontForm:
                    return "字体";

                case XtraRichEditStringId.MenuCmd_ShowParagraphForm:
                    return "段落";

                case XtraRichEditStringId.MenuCmd_ShowNumberingList:
                    return "项目符号和编号";
            }
            return base.GetLocalizedString(id);
        }
    }
}