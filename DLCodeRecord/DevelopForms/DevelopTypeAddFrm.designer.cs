namespace DLCodeRecord.DevelopForms
{
    partial class DevelopTypeAddFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tlDevelopList = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.ColumnId = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.btnAddRoot = new DevExpress.XtraEditors.SimpleButton();
            this.txtRoot = new DevExpress.XtraEditors.TextEdit();
            this.txtChild = new DevExpress.XtraEditors.TextEdit();
            this.btnAddChild = new DevExpress.XtraEditors.SimpleButton();
            this.txtNode = new DevExpress.XtraEditors.TextEdit();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnEdit = new DevExpress.XtraEditors.SimpleButton();
            this.chkEnable = new DevExpress.XtraEditors.CheckEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl3 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.tlDevelopList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoot.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChild.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEnable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).BeginInit();
            this.groupControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlDevelopList
            // 
            this.tlDevelopList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn2,
            this.ColumnId});
            this.tlDevelopList.KeyFieldName = "Id";
            this.tlDevelopList.Location = new System.Drawing.Point(10, 10);
            this.tlDevelopList.Name = "tlDevelopList";
            this.tlDevelopList.ParentFieldName = "ParentId";
            this.tlDevelopList.Size = new System.Drawing.Size(286, 414);
            this.tlDevelopList.TabIndex = 0;
            this.tlDevelopList.TabStop = false;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "名称";
            this.treeListColumn2.FieldName = "Name";
            this.treeListColumn2.Name = "treeListColumn2";
            this.treeListColumn2.OptionsColumn.AllowEdit = false;
            this.treeListColumn2.Visible = true;
            this.treeListColumn2.VisibleIndex = 0;
            // 
            // ColumnId
            // 
            this.ColumnId.Caption = "treeListColumn1";
            this.ColumnId.FieldName = "ParentId";
            this.ColumnId.Name = "ColumnId";
            // 
            // btnAddRoot
            // 
            this.btnAddRoot.Location = new System.Drawing.Point(173, 27);
            this.btnAddRoot.Name = "btnAddRoot";
            this.btnAddRoot.Size = new System.Drawing.Size(75, 23);
            this.btnAddRoot.TabIndex = 1;
            this.btnAddRoot.Text = "添加";
            this.btnAddRoot.Click += new System.EventHandler(this.btnAddRoot_Click);
            // 
            // txtRoot
            // 
            this.txtRoot.Location = new System.Drawing.Point(16, 28);
            this.txtRoot.Name = "txtRoot";
            this.txtRoot.Size = new System.Drawing.Size(151, 20);
            this.txtRoot.TabIndex = 2;
            // 
            // txtChild
            // 
            this.txtChild.Location = new System.Drawing.Point(16, 56);
            this.txtChild.Name = "txtChild";
            this.txtChild.Size = new System.Drawing.Size(151, 20);
            this.txtChild.TabIndex = 3;
            // 
            // btnAddChild
            // 
            this.btnAddChild.Location = new System.Drawing.Point(173, 55);
            this.btnAddChild.Name = "btnAddChild";
            this.btnAddChild.Size = new System.Drawing.Size(75, 23);
            this.btnAddChild.TabIndex = 4;
            this.btnAddChild.Text = "添加";
            this.btnAddChild.Click += new System.EventHandler(this.btnAddChild_Click);
            // 
            // txtNode
            // 
            this.txtNode.Location = new System.Drawing.Point(16, 24);
            this.txtNode.Name = "txtNode";
            this.txtNode.Properties.ReadOnly = true;
            this.txtNode.Size = new System.Drawing.Size(156, 20);
            this.txtNode.TabIndex = 5;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(97, 50);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "删除";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(16, 50);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 7;
            this.btnEdit.Text = "修改";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // chkEnable
            // 
            this.chkEnable.Location = new System.Drawing.Point(185, 24);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Properties.Caption = "启用";
            this.chkEnable.Size = new System.Drawing.Size(75, 19);
            this.chkEnable.TabIndex = 8;
            this.chkEnable.CheckedChanged += new System.EventHandler(this.chkEnable_CheckedChanged);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.txtRoot);
            this.groupControl1.Controls.Add(this.btnAddRoot);
            this.groupControl1.Location = new System.Drawing.Point(304, 10);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(320, 65);
            this.groupControl1.TabIndex = 9;
            this.groupControl1.Text = "大版块";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Controls.Add(this.txtChild);
            this.groupControl2.Controls.Add(this.btnAddChild);
            this.groupControl2.Location = new System.Drawing.Point(304, 94);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(321, 95);
            this.groupControl2.TabIndex = 10;
            this.groupControl2.Text = "小版块";
            // 
            // labelControl1
            // 
            this.labelControl1.AllowHtmlString = true;
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 8F);
            this.labelControl1.Location = new System.Drawing.Point(16, 24);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(114, 13);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "<color=red>*添加之前选择大版块</color>";
            // 
            // groupControl3
            // 
            this.groupControl3.AutoSize = true;
            this.groupControl3.Controls.Add(this.txtNode);
            this.groupControl3.Controls.Add(this.chkEnable);
            this.groupControl3.Controls.Add(this.btnEdit);
            this.groupControl3.Controls.Add(this.btnDelete);
            this.groupControl3.Location = new System.Drawing.Point(304, 195);
            this.groupControl3.Name = "groupControl3";
            this.groupControl3.Size = new System.Drawing.Size(321, 229);
            this.groupControl3.TabIndex = 11;
            this.groupControl3.Text = "版块操作";
            // 
            // DevelopTypeAddFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 442);
            this.Controls.Add(this.groupControl3);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.tlDevelopList);
            this.Location = new System.Drawing.Point(0, 0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DevelopTypeAddFrm";
            this.Text = "DevelopAddFrm";
            ((System.ComponentModel.ISupportInitialize)(this.tlDevelopList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRoot.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtChild.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtNode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEnable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl3)).EndInit();
            this.groupControl3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraTreeList.TreeList tlDevelopList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn ColumnId;
        private DevExpress.XtraEditors.SimpleButton btnAddRoot;
        private DevExpress.XtraEditors.TextEdit txtRoot;
        private DevExpress.XtraEditors.TextEdit txtChild;
        private DevExpress.XtraEditors.SimpleButton btnAddChild;
        private DevExpress.XtraEditors.TextEdit txtNode;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnEdit;
        private DevExpress.XtraEditors.CheckEdit chkEnable;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.GroupControl groupControl3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
    }
}