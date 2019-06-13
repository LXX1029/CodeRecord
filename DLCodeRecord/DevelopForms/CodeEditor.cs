using Common;
using ScintillaNET;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using namespaceFrm = System.Windows.Forms;

using Win = System.Drawing;

namespace DLCodeRecord.DevelopForms
{
    public partial class CodeEditor : UserControl
    {
        public Scintilla scintilla = new Scintilla();

        public CodeEditor()
        {
            InitializeComponent();


            InitialScintilla();

            scintilla.UpdateUI += Scintilla_UpdateUI;
            scintilla.CharAdded += scintilla_CharAdded;
            scintilla.DragEnter += new namespaceFrm.DragEventHandler(scintilla_DragEnter);
            this.Controls.Add(scintilla);
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
            scintilla.AllowDrop = true;
            this.scintilla.AdditionalCaretsBlink = false;
            this.scintilla.AnnotationVisible = ScintillaNET.Annotation.Boxed;
            this.scintilla.AutoCChooseSingle = true;
            this.scintilla.CaretLineVisible = true;
            this.scintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla.EdgeColor = System.Drawing.Color.Maroon;
            this.scintilla.EdgeColumn = 2;
            this.scintilla.IdleStyling = ScintillaNET.IdleStyling.ToVisible;
            this.scintilla.Location = new System.Drawing.Point(0, 0);
            this.scintilla.Name = "scintilla";
            this.scintilla.Size = new System.Drawing.Size(713, 581);
            this.scintilla.TabIndex = 0;

            this.scintilla.WrapIndentMode = ScintillaNET.WrapIndentMode.Indent;
            this.scintilla.WrapMode = ScintillaNET.WrapMode.Word;
            this.scintilla.WrapVisualFlagLocation = ScintillaNET.WrapVisualFlagLocation.StartByText;
            this.scintilla.WrapVisualFlags = ((ScintillaNET.WrapVisualFlags)(((ScintillaNET.WrapVisualFlags.End | ScintillaNET.WrapVisualFlags.Start)
            | ScintillaNET.WrapVisualFlags.Margin)));

            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 14;

            scintilla.StyleClearAll();
            scintilla.AllowDrop = true;
            scintilla.BorderStyle = BorderStyle.FixedSingle;
            this.scintilla.Margins[0].Width = 30;

            // C#
            scintilla.Lexer = Lexer.Cpp;
            #endregion

            #region 设置行号
            scintilla.Styles[Style.LineNumber].ForeColor = Win.Color.FromArgb(72, 145, 175);
            #endregion

            #region 代码折叠
            scintilla.SetProperty("tab.timmy.whinge.level", "0");
            scintilla.SetProperty("fold", "1");

            scintilla.SetProperty("fold.compact", "0");
            // //{   //} 形式折叠
            scintilla.SetProperty("fold.comment", "1");
            // 设置#region #endregion 垂直线显示
            scintilla.SetProperty("fold.preprocessor", "1");


            // 折叠显示符号
            scintilla.Margins[2].Type = MarginType.Symbol;
            scintilla.Margins[2].Mask = Marker.MaskFolders;
            scintilla.Margins[2].Sensitive = true;
            scintilla.Margins[2].Width = 20;

            for (int i = Marker.FolderEnd - 1; i <= Marker.FolderOpen; i++)
            {
                scintilla.Markers[i].SetForeColor(Win.SystemColors.ControlLightLight);
                scintilla.Markers[i].SetBackColor(Win.SystemColors.ControlDark);
            }
            // 折叠块符号
            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            // 展开符号
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.Folder].SetBackColor(Win.SystemColors.ControlText);

            // 中间嵌套区折叠域符号
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.VLine;

            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            // 结束线样式
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            scintilla.AutomaticFold = AutomaticFold.Click;
            scintilla.ViewWhitespace = WhitespaceMode.VisibleAlways;
            // 设置折叠区域margin颜色
            scintilla.SetFoldMarginColor(false, Win.Color.FromArgb(106, 226, 108));
            scintilla.LineEndTypesAllowed = LineEndType.Default;
            scintilla.LineFromPosition(2);
            scintilla.SetFoldFlags(FoldFlags.LineAfterContracted);
            #endregion

            #region 设置备注

            scintilla.Styles[Style.Cpp.Comment].ForeColor = Win.Color.FromArgb(0, 128, 0); // Green
            scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Win.Color.FromArgb(0, 128, 0); // Green
            //scintilla.Styles[Style.Cpp.CommentLine].Size = 20;
            scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = Win.Color.FromArgb(128, 128, 128); // Gray
            #endregion

            #region 其它设置
            scintilla.Styles[Style.Cpp.Default].ForeColor = Win.Color.Silver;

            // 数字样式
            scintilla.Styles[Style.Cpp.Number].Bold = false;
            scintilla.Styles[Style.Cpp.Number].ForeColor = Win.Color.Black;

            scintilla.Styles[Style.Cpp.Word].ForeColor = Win.Color.Blue;
            //scintilla.Styles[Style.Cpp.Word].Size = 16;
            scintilla.Styles[Style.Cpp.Word2].ForeColor = Win.Color.Blue;
            scintilla.Styles[Style.Cpp.String].ForeColor = Win.Color.FromArgb(163, 21, 21); // Red

            // 字符串设置
            scintilla.Styles[Style.Cpp.String].ForeColor = Win.Color.FromArgb(207, 103, 21);
            scintilla.Styles[Style.Cpp.String].BackColor = Win.Color.Transparent;
            scintilla.Styles[Style.Cpp.String].Hotspot = true;
            scintilla.Styles[Style.Cpp.String].Underline = false;

            // 输入字符设定
            scintilla.Styles[Style.Cpp.Character].ForeColor = Win.Color.Black;
            scintilla.Styles[Style.Cpp.Character].BackColor = Win.Color.White;

            //scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.Black;
            //scintilla.Styles[Style.Cpp.Verbatim].BackColor = Color.White;

            // 操作符前景色
            scintilla.Styles[Style.Cpp.Operator].ForeColor = Win.Color.Purple;
            scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Win.Color.Maroon;


            // 正则表达式
            scintilla.Styles[Style.Cpp.Regex].ForeColor = Win.Color.Blue;

            scintilla.AnnotationVisible = Annotation.Boxed;
            scintilla.DocLineFromVisible(1);


            scintilla.Styles[Style.Cpp.StringRaw].FillLine = true;
            scintilla.Styles[Style.Cpp.StringRaw].BackColor = Win.Color.Yellow;

            // 设置匹配框高度
            scintilla.AutoCMaxHeight = 15;
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
            scintilla.WhitespaceSize = 1;

            scintilla.WrapMode = WrapMode.Whitespace;
            scintilla.WrapVisualFlagLocation = WrapVisualFlagLocation.Default;
            #endregion

            #region 大括号设置
            scintilla.IndentationGuides = IndentView.LookBoth;
            scintilla.Styles[Style.BraceLight].BackColor = Win.Color.LightGray;
            scintilla.Styles[Style.BraceLight].ForeColor = Win.Color.BlueViolet;
            scintilla.Styles[Style.BraceBad].ForeColor = Win.Color.Red;
            #endregion

            #region 设置关键字

            scintilla.SetKeywords(0, FirstKeyWords);
            scintilla.SetKeywords(1, SecondKeyWords);
            scintilla.FontQuality = FontQuality.LcdOptimized;

            #endregion

            #region 事件
            scintilla.CharAdded += scintilla_CharAdded;
            scintilla.UpdateUI += Scintilla_UpdateUI;

            scintilla.WrapVisualFlags = WrapVisualFlags.Margin;
            #endregion

        }
        #endregion 初始化scintilla

        #region Scintilla 事件
        /// <summary>
        /// 最后一次光标位置
        /// </summary>
        private int lastCaretPos { get; set; } = 0;
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
            var caretPos = scintilla.CurrentPosition;
            if (lastCaretPos != caretPos)
            {
                lastCaretPos = caretPos;
                var bracePos1 = -1;
                var bracePos2 = -1;

                // Is there a brace to the left or right?
                if (caretPos > 0 && IsBrace(scintilla.GetCharAt(caretPos - 1)))
                    bracePos1 = (caretPos - 1);
                else if (IsBrace(scintilla.GetCharAt(caretPos)))
                    bracePos1 = caretPos;

                if (bracePos1 >= 0)
                {
                    // Find the matching brace
                    bracePos2 = scintilla.BraceMatch(bracePos1);
                    if (bracePos2 == Scintilla.InvalidPosition)
                    {
                        scintilla.BraceBadLight(bracePos1);
                        scintilla.HighlightGuide = 0;
                    }
                    else
                    {
                        scintilla.BraceHighlight(bracePos1, bracePos2);
                        scintilla.HighlightGuide = scintilla.GetColumn(bracePos1);
                    }
                }
                else
                {
                    // Turn off brace matching
                    scintilla.BraceHighlight(Scintilla.InvalidPosition, Scintilla.InvalidPosition);
                    scintilla.HighlightGuide = 0;
                }
            }
        }

        #region 添加字符事件

        private void scintilla_CharAdded(object sender, CharAddedEventArgs e)
        {
            // Find the word start
            var currentPos = scintilla.CurrentPosition;
            var wordStartPos = scintilla.WordStartPosition(currentPos, true);

            // 显示匹配列表
            var lenEntered = currentPos - wordStartPos;
            if (lenEntered > 0)  // 匹配开始长度
            {
                if (!scintilla.AutoCActive)
                {
                    // 取得键盘输入字符的ascii码，并转换为相应的字符
                    int ascii = e.Char;
                    ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                    byte[] btNumber = new byte[] { (byte)ascii };
                    string key = asciiEncoding.GetString(btNumber);
                    scintilla.AutoCShow(lenEntered, AutoComplateCharWords(key));
                }
            }
        }

        #endregion 添加字符事件

        #region 拖放文件到Scintilla

        /// <summary>
        /// 拖放文件到 控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scintilla_DragEnter(object sender, namespaceFrm.DragEventArgs e)
        {
            try
            {
                this.scintilla.Text = "";
                Array aryFiles = ((System.Array)e.Data.GetData(namespaceFrm.DataFormats.FileDrop));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < aryFiles.Length; i++)
                {
                    string[] valuesArray = File.ReadAllLines(aryFiles.GetValue(i).ToString(), Encoding.UTF8);
                    if (valuesArray.Length > 0)
                    {
                        valuesArray.ToList().ForEach(p =>
                        {
                            if (p != "")
                                builder.Append(p + "\r\n");
                        });
                    }
                }
                this.scintilla.Text = builder.ToString();
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