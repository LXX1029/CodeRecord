using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraReports.UI;
using DLCodeRecord.DevelopForms;

namespace DLCodeRecord.Reports
{
    /// <summary>
    /// 打印窗体
    /// </summary>
    public partial class ReportFrm : BaseFrm
    {
        private XtraReport _xtraReport { get; set; }

        public ReportFrm(XtraReport report)
        {
            InitializeComponent();
            var tuple = UtilityHelper.GetWorkingAreaSize();
            this.Size = tuple.Item3;
            this.Location = tuple.Item4;
            this.TopMost = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "打印";
            report.PrintingSystem.ProgressReflector = null;
            _xtraReport = report;
            this.Load -= ReportFrm_Load;
            this.Load += ReportFrm_Load;
        }

        private void ReportFrm_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
               {
                   this.Invoke(new Action(() =>
                   {
                       documentViewer1.DocumentSource = _xtraReport;
                       _xtraReport.CreateDocument();
                   }));
               });
        }
    }
}
