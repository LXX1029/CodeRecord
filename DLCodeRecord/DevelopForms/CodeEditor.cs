using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using ScintillaNET;
using namespaceFrm = System.Windows.Forms;
using Win = System.Drawing;

namespace DLCodeRecord.DevelopForms
{
    /// <summary>
    /// CodeEditor
    /// </summary>
    public partial class CodeEditor : UserControl
    {
        public Scintilla Scintilla = new Scintilla();

        public CodeEditor()
        {
            InitializeComponent();
            InitialScintilla();
            Scintilla.UpdateUI -= Scintilla_UpdateUI;
            Scintilla.CharAdded -= scintilla_CharAdded;
            Scintilla.DragEnter -= new namespaceFrm.DragEventHandler(scintilla_DragEnter);
            Scintilla.UpdateUI += Scintilla_UpdateUI;
            Scintilla.CharAdded += scintilla_CharAdded;
            Scintilla.DragEnter += new namespaceFrm.DragEventHandler(scintilla_DragEnter);
            this.Controls.Add(Scintilla);
        }

        #region 初始化scintilla

        /// <summary>
        /// 第一列关键字
        /// </summary>
        private static readonly string FirstKeyWords = UtilityHelper.GetConfigurationKeyValue("FirstKeyWords");

        /// <summary>
        /// 其它补充关键字
        /// </summary>
        private static readonly string OtherKeyWords = UtilityHelper.GetConfigurationKeyValue("OtherKeyWords");

        /// <summary>
        /// 第二列关键字
        /// </summary>
        private static readonly string SecondKeyWords = UtilityHelper.GetConfigurationKeyValue("SecondKeyWords");

        /// <summary>
        /// 自动匹配输入
        /// <param name="key">输入的关键字</param>
        /// </summary>
        private string AutoComplateCharWords(string key)
        {
            var totalKeywords = FirstKeyWords + SecondKeyWords + OtherKeyWords;
            System.Collections.Generic.IEnumerable<string> strArray = totalKeywords.Split(' ').OrderBy(o => o).Where(s => s.Contains(key));
            totalKeywords = string.Join(" ", strArray).Trim();
            return totalKeywords;
        }

        private void InitialScintilla()
        {
            #region 初始化
            Scintilla.AllowDrop = true;
            this.Scintilla.AdditionalCaretsBlink = false;
            this.Scintilla.AnnotationVisible = ScintillaNET.Annotation.Boxed;
            this.Scintilla.AutoCChooseSingle = true;
            this.Scintilla.CaretLineVisible = true;
            this.Scintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Scintilla.EdgeColor = System.Drawing.Color.Maroon;
            this.Scintilla.EdgeColumn = 2;
            this.Scintilla.IdleStyling = ScintillaNET.IdleStyling.ToVisible;
            this.Scintilla.Location = new System.Drawing.Point(0, 0);
            this.Scintilla.Name = "scintilla";
            this.Scintilla.Size = new System.Drawing.Size(713, 581);
            this.Scintilla.TabIndex = 0;

            this.Scintilla.WrapIndentMode = ScintillaNET.WrapIndentMode.Indent;
            this.Scintilla.WrapMode = ScintillaNET.WrapMode.Word;
            this.Scintilla.WrapVisualFlagLocation = ScintillaNET.WrapVisualFlagLocation.StartByText;
            this.Scintilla.WrapVisualFlags = (ScintillaNET.WrapVisualFlags.End | ScintillaNET.WrapVisualFlags.Start)
            | ScintillaNET.WrapVisualFlags.Margin;

            Scintilla.StyleResetDefault();
            Scintilla.Styles[Style.Default].Font = "Consolas";
            Scintilla.Styles[Style.Default].Size = 14;

            Scintilla.StyleClearAll();
            Scintilla.AllowDrop = true;
            Scintilla.BorderStyle = BorderStyle.FixedSingle;
            this.Scintilla.Margins[0].Width = 30;

            // C#
            Scintilla.Lexer = Lexer.Cpp;
            #endregion

            #region 设置行号
            Scintilla.Styles[Style.LineNumber].ForeColor = Win.Color.FromArgb(72, 145, 175);
            #endregion

            #region 代码折叠
            Scintilla.SetProperty("tab.timmy.whinge.level", "0");
            Scintilla.SetProperty("fold", "1");

            Scintilla.SetProperty("fold.compact", "0");
            // //{   //} 形式折叠
            Scintilla.SetProperty("fold.comment", "1");
            // 设置#region #endregion 垂直线显示
            Scintilla.SetProperty("fold.preprocessor", "1");

            // 折叠显示符号
            Scintilla.Margins[2].Type = MarginType.Symbol;
            Scintilla.Margins[2].Mask = Marker.MaskFolders;
            Scintilla.Margins[2].Sensitive = true;
            Scintilla.Margins[2].Width = 20;

            for (int i = Marker.FolderEnd - 1; i <= Marker.FolderOpen; i++)
            {
                Scintilla.Markers[i].SetForeColor(Win.SystemColors.ControlLightLight);
                Scintilla.Markers[i].SetBackColor(Win.SystemColors.ControlDark);
            }
            // 折叠块符号
            Scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            // 展开符号
            Scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            Scintilla.Markers[Marker.Folder].SetBackColor(Win.SystemColors.ControlText);

            // 中间嵌套区折叠域符号
            Scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.VLine;
            Scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.VLine;

            Scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            // 结束线样式
            Scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            Scintilla.AutomaticFold = AutomaticFold.Click;
            Scintilla.ViewWhitespace = WhitespaceMode.VisibleAlways;
            // 设置折叠区域margin颜色
            Scintilla.SetFoldMarginColor(false, Win.Color.FromArgb(106, 226, 108));
            Scintilla.LineEndTypesAllowed = LineEndType.Default;
            Scintilla.LineFromPosition(2);
            Scintilla.SetFoldFlags(FoldFlags.LineAfterContracted);
            #endregion

            #region 设置备注

            Scintilla.Styles[Style.Cpp.Comment].ForeColor = Win.Color.FromArgb(0, 128, 0); // Green
            Scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Win.Color.FromArgb(0, 128, 0); // Green
            //scintilla.Styles[Style.Cpp.CommentLine].Size = 20;
            Scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = Win.Color.FromArgb(128, 128, 128); // Gray
            #endregion

            #region 其它设置
            Scintilla.Styles[Style.Cpp.Default].ForeColor = Win.Color.Silver;

            // 数字样式
            Scintilla.Styles[Style.Cpp.Number].Bold = false;
            Scintilla.Styles[Style.Cpp.Number].ForeColor = Win.Color.Black;

            Scintilla.Styles[Style.Cpp.Word].ForeColor = Win.Color.Blue;
            //scintilla.Styles[Style.Cpp.Word].Size = 16;
            Scintilla.Styles[Style.Cpp.Word2].ForeColor = Win.Color.Blue;
            Scintilla.Styles[Style.Cpp.String].ForeColor = Win.Color.FromArgb(163, 21, 21); // Red

            // 字符串设置
            Scintilla.Styles[Style.Cpp.String].ForeColor = Win.Color.FromArgb(207, 103, 21);
            Scintilla.Styles[Style.Cpp.String].BackColor = Win.Color.Transparent;
            Scintilla.Styles[Style.Cpp.String].Hotspot = true;
            Scintilla.Styles[Style.Cpp.String].Underline = false;

            // 输入字符设定
            Scintilla.Styles[Style.Cpp.Character].ForeColor = Win.Color.Black;
            Scintilla.Styles[Style.Cpp.Character].BackColor = Win.Color.White;

            //scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.Black;
            //scintilla.Styles[Style.Cpp.Verbatim].BackColor = Color.White;

            // 操作符前景色
            Scintilla.Styles[Style.Cpp.Operator].ForeColor = Win.Color.Purple;
            Scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Win.Color.Maroon;

            // 正则表达式
            Scintilla.Styles[Style.Cpp.Regex].ForeColor = Win.Color.Blue;

            Scintilla.AnnotationVisible = Annotation.Boxed;
            Scintilla.DocLineFromVisible(1);

            Scintilla.Styles[Style.Cpp.StringRaw].FillLine = true;
            Scintilla.Styles[Style.Cpp.StringRaw].BackColor = Win.Color.Yellow;

            // 设置匹配框高度
            Scintilla.AutoCMaxHeight = 15;
            /* 增加行space
            scintilla.ExtraAscent = 5;
            scintilla.ExtraDescent = 5;*/

            //scintilla.MultipleSelection = true;
            //scintilla.MouseSelectionRectangularSwitch = true;
            //scintilla.AdditionalSelectionTyping = true;
            //scintilla.VirtualSpaceOptions = VirtualSpace.RectangularSelection;

            /* 显示空白
            scintilla.WhitespaceSize = 5;
            scintilla.ViewWhitespace = WhitespaceMode.VisibleAlways;
            scintilla.SetWhitespaceForeColor(true, Color.Orange);*/

            /* 放大缩小
            scintilla.ZoomIn(); // Increase
            scintilla.ZoomOut(); // Decrease
            scintilla.Zoom = 15; // */

            // 输入匹配长度
            Scintilla.WhitespaceSize = 1;

            Scintilla.WrapMode = WrapMode.Whitespace;
            Scintilla.WrapVisualFlagLocation = WrapVisualFlagLocation.Default;
            #endregion

            #region 大括号设置
            Scintilla.IndentationGuides = IndentView.LookBoth;
            Scintilla.Styles[Style.BraceLight].BackColor = Win.Color.LightGray;
            Scintilla.Styles[Style.BraceLight].ForeColor = Win.Color.BlueViolet;
            Scintilla.Styles[Style.BraceBad].ForeColor = Win.Color.Red;
            #endregion

            #region 设置关键字

            Scintilla.SetKeywords(0, FirstKeyWords);
            Scintilla.SetKeywords(1, SecondKeyWords);
            Scintilla.FontQuality = FontQuality.LcdOptimized;

            #endregion

            #region 事件
            Scintilla.CharAdded -= scintilla_CharAdded;
            Scintilla.UpdateUI -= Scintilla_UpdateUI;
            Scintilla.CharAdded += scintilla_CharAdded;
            Scintilla.UpdateUI += Scintilla_UpdateUI;

            Scintilla.WrapVisualFlags = WrapVisualFlags.Margin;
            #endregion

        }
        #endregion 初始化scintilla

        #region Scintilla 事件
        /// <summary>
        /// 最后一次光标位置
        /// </summary>
        private int LastCaretPos { get; set; }
        private static bool IsBrace(int c)
        {
            switch (c)
            {
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                case '<':
                case '>':
                    return true;
            }

            return false;
        }

        private void Scintilla_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            // Has the caret changed position?
            var caretPos = Scintilla.CurrentPosition;
            if (LastCaretPos != caretPos)
            {
                LastCaretPos = caretPos;
                var bracePos1 = -1;
                var bracePos2 = -1;

                // Is there a brace to the left or right?
                if (caretPos > 0 && IsBrace(Scintilla.GetCharAt(caretPos - 1)))
                    bracePos1 = caretPos - 1;
                else if (IsBrace(Scintilla.GetCharAt(caretPos)))
                    bracePos1 = caretPos;

                if (bracePos1 >= 0)
                {
                    // Find the matching brace
                    bracePos2 = Scintilla.BraceMatch(bracePos1);
                    if (bracePos2 == Scintilla.InvalidPosition)
                    {
                        Scintilla.BraceBadLight(bracePos1);
                        Scintilla.HighlightGuide = 0;
                    }
                    else
                    {
                        Scintilla.BraceHighlight(bracePos1, bracePos2);
                        Scintilla.HighlightGuide = Scintilla.GetColumn(bracePos1);
                    }
                }
                else
                {
                    // Turn off brace matching
                    Scintilla.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);
                    Scintilla.HighlightGuide = 0;
                }
            }
        }

        #region 添加字符事件

        private void scintilla_CharAdded(object sender, CharAddedEventArgs e)
        {
            // Find the word start
            var currentPos = Scintilla.CurrentPosition;
            var wordStartPos = Scintilla.WordStartPosition(currentPos, true);

            // 显示匹配列表
            var lenEntered = currentPos - wordStartPos;
            // 匹配开始长度
            if (lenEntered > 0)
            {
                if (!Scintilla.AutoCActive)
                {
                    // 取得键盘输入字符的ascii码，并转换为相应的字符
                    int ascii = e.Char;
                    ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                    byte[] btNumber = new byte[] { (byte)ascii };
                    string key = asciiEncoding.GetString(btNumber);
                    Scintilla.AutoCShow(lenEntered, AutoComplateCharWords(key));
                }
            }
        }

        #endregion 添加字符事件

        #region 拖放文件到Scintilla

        /// <summary>
        /// 拖放文件到 控件
        /// </summary>
        private void scintilla_DragEnter(object sender, namespaceFrm.DragEventArgs e)
        {
            try
            {
                this.Scintilla.Text = string.Empty;
                Array aryFiles = (System.Array)e.Data.GetData(namespaceFrm.DataFormats.FileDrop);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < aryFiles.Length; i++)
                {
                    string[] valuesArray = File.ReadAllLines(aryFiles.GetValue(i).ToString(), Encoding.UTF8);
                    if (valuesArray.Length > 0)
                    {
                        valuesArray.ToList().ForEach(p =>
                        {
                            if (!Common.VerifyHelper.IsEmptyOrNullOrWhiteSpace(p))
                                builder.Append(p + "\r\n");
                        });
                    }
                }
                this.Scintilla.Text = builder.ToString();
            }
            catch (Exception ex)
            {
                MsgHelper.ShowError(ex.Message);
                LoggerHelper.WriteException(ex.Message);
            }
        }

        #endregion 拖放文件到Scintilla

        #endregion Scintilla 事件
    }
}