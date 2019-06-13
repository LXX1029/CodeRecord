using Common;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraReports.UI;
using DLCodeRecord.DevelopForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLCodeRecord.Reports
{
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
            this.Load += ReportFrm_Load;
        }

        private void ReportFrm_Load(object sender, EventArgs e)
        {


            Task.Run(() =>
               {
                   this.Invoke((Action)delegate
                   {
                       documentViewer1.DocumentSource = _xtraReport;

                       _xtraReport.CreateDocument();
                   });
               });





            // Create a form to show a progress bar,
            // and adjust its properties.
            //Form form = new Form()
            //{
            //    FormBorderStyle = FormBorderStyle.None,
            //    Size = new System.Drawing.Size(300, 25),
            //    ShowInTaskbar = false,
            //    StartPosition = FormStartPosition.CenterScreen,
            //    TopMost = true
            //};

            //// Create a ProgressBar along with its ReflectorBar.
            //ProgressBarControl progressBar = new ProgressBarControl();
            //ReflectorBar reflectorBar = new ReflectorBar(progressBar);

            //// Add a progress bar to a form and show it.
            //form.Controls.Add(progressBar);
            //progressBar.Dock = DockStyle.Fill;
            //form.Show();

            //try
            //{
            //    // Register the reflector bar, so that it reflects
            //    // the state of a ProgressReflector.
            //    _xtraReport.PrintingSystem.ProgressReflector = reflectorBar;
            //_xtraReport.CreateDocument();
            //}
            //finally
            //{
            //    // Unregister the reflector bar, so that it no longer
            //    // reflects the state of a ProgressReflector.
            //    _xtraReport.PrintingSystem.ResetProgressReflector();
            //    form.Close();
            //    form.Dispose();
            //}
        }
    }
}
