namespace DLCodeRecord.DevelopForms
{
    partial class CodeEditor
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();

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
            this.Scintilla.WrapVisualFlags = ((ScintillaNET.WrapVisualFlags)(((ScintillaNET.WrapVisualFlags.End | ScintillaNET.WrapVisualFlags.Start)
            | ScintillaNET.WrapVisualFlags.Margin)));


            // 
            // CodeEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "CodeEditor";
            this.Size = new System.Drawing.Size(665, 548);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
