﻿using DevExpress.CodeParser;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace DLCodeRecord
{
    /// <summary>
    /// 设置代码高亮类
    /// CS,XAML
    /// C#
    /// Visual Basic
    /// JavaScript
    /// HTML
    /// XAML
    /// CSS
    /// </summary>
    public class CustomOtherSyntaxHighlightService : ISyntaxHighlightService
    {
        private readonly RichEditControl syntaxEditor;
        private SyntaxHighlightProperties commentProperties;
        private SyntaxHighlightProperties keywordProperties;
        private SyntaxHighlightProperties lineProperties;
        private SyntaxHighlightProperties stringProperties;
        private SyntaxColors syntaxColors;
        private SyntaxHighlightProperties textProperties;
        private SyntaxHighlightProperties xmlCommentProperties;

        public CustomOtherSyntaxHighlightService(RichEditControl syntaxEditor)
        {
            this.syntaxEditor = syntaxEditor;
            syntaxColors = new SyntaxColors(UserLookAndFeel.Default);
        }

        private void HighlightCategorizedToken(CategorizedToken token, List<SyntaxHighlightToken> syntaxTokens)
        {
            Color backColor = syntaxEditor.ActiveView.BackColor;
            TokenCategory category = token.Category;
            if (category == TokenCategory.Comment)
                syntaxTokens.Add(SetTokenColor(token, commentProperties, backColor));
            else if (category == TokenCategory.Keyword)
                syntaxTokens.Add(SetTokenColor(token, keywordProperties, backColor));
            else if (category == TokenCategory.String)
                syntaxTokens.Add(SetTokenColor(token, stringProperties, backColor));
            else if (category == TokenCategory.XmlComment)
                syntaxTokens.Add(SetTokenColor(token, xmlCommentProperties, backColor));
            else if (category == TokenCategory.Number)
                syntaxTokens.Add(SetTokenColor(token, lineProperties, backColor));
            else
                syntaxTokens.Add(SetTokenColor(token, textProperties, backColor));
        }

        private void HighlightSyntax(TokenCollection tokens)
        {
            commentProperties = new SyntaxHighlightProperties();
            commentProperties.ForeColor = syntaxColors.CommentColor;

            keywordProperties = new SyntaxHighlightProperties();
            keywordProperties.ForeColor = syntaxColors.KeywordColor;

            stringProperties = new SyntaxHighlightProperties();
            stringProperties.ForeColor = syntaxColors.StringColor;

            xmlCommentProperties = new SyntaxHighlightProperties();
            xmlCommentProperties.ForeColor = syntaxColors.XmlCommentColor;

            textProperties = new SyntaxHighlightProperties();
            textProperties.ForeColor = syntaxColors.TextColor;

            // 数字 颜色
            lineProperties = new SyntaxHighlightProperties();
            lineProperties.ForeColor = syntaxColors.LineNumberColor;

            if (tokens == null || tokens.Count == 0)
                return;

            Document document = syntaxEditor.Document;
            CharacterProperties cp = document.BeginUpdateCharacters(0, 1);
            List<SyntaxHighlightToken> syntaxTokens = new List<SyntaxHighlightToken>(tokens.Count);
            foreach (Token token in tokens)
            {
                HighlightCategorizedToken((CategorizedToken)token, syntaxTokens);
            }
            document.ApplySyntaxHighlight(syntaxTokens);
            document.EndUpdateCharacters(cp);
        }

        private SyntaxHighlightToken SetTokenColor(Token token, SyntaxHighlightProperties foreColor, Color backColor)
        {
            if (syntaxEditor.Document.Paragraphs.Count < token.Range.Start.Line)
                return null;
            int paragraphStart = DocumentHelper.GetParagraphStart(syntaxEditor.Document.Paragraphs[token.Range.Start.Line - 1]);
            int tokenStart = paragraphStart + token.Range.Start.Offset - 1;
            if (token.Range.End.Line != token.Range.Start.Line)
                paragraphStart = DocumentHelper.GetParagraphStart(syntaxEditor.Document.Paragraphs[token.Range.End.Line - 1]);

            int tokenEnd = paragraphStart + token.Range.End.Offset - 1;
            Debug.Assert(tokenEnd > tokenStart);
            return new SyntaxHighlightToken(tokenStart, tokenEnd - tokenStart, foreColor);
        }

        #region #ISyntaxHighlightServiceMembers

        public void Execute()
        {
            string newText = syntaxEditor.Text;

            // Determine language by file extension.
            string ext = System.IO.Path.GetExtension(syntaxEditor.Options.DocumentSaveOptions.CurrentFileName);
            ParserLanguageID lang_ID = ParserLanguage.FromFileExtension(ext);
            // Do not parse HTML or XML.
            //if (lang_ID == ParserLanguageID.Html ||
            //    lang_ID == ParserLanguageID.Xml ||
            //    lang_ID == ParserLanguageID.None) return;
            // Use DevExpress.CodeParser to parse text into tokens.
            ITokenCategoryHelper tokenHelper = TokenCategoryHelperFactory.CreateHelper(lang_ID);
            TokenCollection highlightTokens;
            highlightTokens = tokenHelper.GetTokens(newText);
            HighlightSyntax(highlightTokens);
        }

        public void ForceExecute()
        {
            Execute();
        }

        #endregion #ISyntaxHighlightServiceMembers
    }

    /// <summary>
    ///  This class provides colors to highlight the tokens.
    /// </summary>
    public class SyntaxColors
    {
        private UserLookAndFeel lookAndFeel;

        public SyntaxColors(UserLookAndFeel lookAndFeel)
        {
            this.lookAndFeel = lookAndFeel;
        }

        public Color CommentColor { get { return GetCommonColorByName(CommonSkins.SkinInformationColor, DefaultCommentColor); } }
        public Color KeywordColor { get { return GetCommonColorByName(CommonSkins.SkinQuestionColor, DefaultKeywordColor); } }
        public Color LineNumberColor { get { return GetCommonColorByName(CommonSkins.SkinWarningColor, DefaultLineNumberColor); } }
        public Color StringColor { get { return GetCommonColorByName(CommonSkins.SkinWarningColor, DefaultStringColor); } }
        public Color TextColor { get { return GetCommonColorByName(CommonColors.WindowText, DefaultTextColor); } }
        public Color XmlCommentColor { get { return GetCommonColorByName(CommonColors.DisabledText, DefaultXmlCommentColor); } }
        private static Color DefaultCommentColor
        { get { return Color.Green; } }
        private static Color DefaultKeywordColor
        { get { return Color.Blue; } }
        private static Color DefaultLineNumberColor
        { get { return Color.Red; } }
        private static Color DefaultStringColor
        { get { return Color.Brown; } }
        private static Color DefaultTextColor
        { get { return Color.Black; } }
        private static Color DefaultXmlCommentColor
        { get { return Color.Gray; } }

        private Color GetCommonColorByName(string colorName, Color defaultColor)
        {
            Skin skin = CommonSkins.GetSkin(lookAndFeel);
            if (skin == null)
                return defaultColor;
            return skin.Colors[colorName];
        }
    }
}