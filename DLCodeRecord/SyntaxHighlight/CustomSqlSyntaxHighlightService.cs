using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Services;
using System.Collections.Generic;
using System.Drawing;

namespace DLCodeRecord
{
    /// <summary>
    /// Sql代码高亮类
    /// </summary>
    public class CustomSqlSyntaxHighlightService : ISyntaxHighlightService
    {
        public CustomSqlSyntaxHighlightService(Document document)
        {
            this.document = document;
        }

        #region #parsetokens

        private readonly Document document;
        private SyntaxHighlightProperties defaultSettings = new SyntaxHighlightProperties() { ForeColor = Color.Black };

        private string[] keywords = new string[] {
            "INSERT","insert", "SELECT", "CREATE", "TABLE", "USE", "IDENTITY", "ON", "OFF", "NOT", "NULL", "WITH", "SET"
            ,"public","private","void","class","try","catch","object","EventArgs"
            ,"string","string[]","int","double","decimale","double","Image","byte","byte[]"
            ,"from","in","on","join","where"
            ,"for","foreach","if","else","switch","case","then","break","default","continue","return"
        };

        private SyntaxHighlightProperties keywordSettings = new SyntaxHighlightProperties() { ForeColor = Color.Blue };
        private SyntaxHighlightProperties stringSettings = new SyntaxHighlightProperties() { ForeColor = Color.Green };

        private void AddPlainTextTokens(List<SyntaxHighlightToken> tokens)
        {
            int count = tokens.Count;
            if (count == 0)
            {
                tokens.Add(new SyntaxHighlightToken(0, document.Range.End.ToInt(), defaultSettings));
                return;
            }
            tokens.Insert(0, new SyntaxHighlightToken(0, tokens[0].Start, defaultSettings));
            for (int i = 1; i < count; i++)
            {
                tokens.Insert(i * 2, new SyntaxHighlightToken(tokens[i * 2 - 1].End,
 tokens[i * 2].Start - tokens[i * 2 - 1].End, defaultSettings));
            }
            tokens.Add(new SyntaxHighlightToken(tokens[count * 2 - 1].End,
 document.Range.End.ToInt() - tokens[count * 2 - 1].End, defaultSettings));
        }

        private bool IsRangeInTokens(DocumentRange range, List<SyntaxHighlightToken> tokens)
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (range.Start.ToInt() >= tokens[i].Start && range.End.ToInt() <= tokens[i].End)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 转换成符号
        /// </summary>
        /// <returns></returns>
        private List<SyntaxHighlightToken> ParseTokens()
        {
            List<SyntaxHighlightToken> tokens = new List<SyntaxHighlightToken>();
            DocumentRange[] ranges = null;
            // search for quotation marks
            ranges = document.FindAll("'", SearchOptions.None);
            for (int i = 0; i < ranges.Length / 2; i++)
            {
                tokens.Add(new SyntaxHighlightToken(ranges[i * 2].Start.ToInt(),
                    ranges[i * 2 + 1].Start.ToInt() - ranges[i * 2].Start.ToInt() + 1, stringSettings));
            }
            // search for keywords
            for (int i = 0; i < keywords.Length; i++)
            {
                ranges = document.FindAll(keywords[i].ToLower(), SearchOptions.CaseSensitive | SearchOptions.WholeWord);

                for (int j = 0; j < ranges.Length; j++)
                {
                    if (!IsRangeInTokens(ranges[j], tokens))
                        tokens.Add(new SyntaxHighlightToken(ranges[j].Start.ToInt(), ranges[j].Length, keywordSettings));
                }
            }
            // order tokens by their start position
            tokens.Sort(new SyntaxHighlightTokenComparer());
            // fill in gaps in document coverage
            AddPlainTextTokens(tokens);
            return tokens;
        }

        #endregion #parsetokens

        #region #ISyntaxHighlightServiceMembers

        public void Execute()
        {
            document.ApplySyntaxHighlight(ParseTokens());
        }

        public void ForceExecute()
        {
            Execute();
        }

        #endregion #ISyntaxHighlightServiceMembers
    }

    #region #SyntaxHighlightTokenComparer

    public class SyntaxHighlightTokenComparer : IComparer<SyntaxHighlightToken>
    {
        public int Compare(SyntaxHighlightToken x, SyntaxHighlightToken y)
        {
            return x.Start - y.Start;
        }
    }

    #endregion #SyntaxHighlightTokenComparer
}