using DevExpress.XtraWaitForm;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLInitialDB
{
    public partial class WaitForm1 : WaitForm
    {
        public WaitForm1()
        {
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;
            this.ShowOnTopMode = ShowFormOnTopMode.AboveAll;
            this.Load += WaitForm1_Load;
        }

        public enum WaitFormCommand
        {
        }

        /// <summary>
        /// 运行CMD命令
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns></returns>
        public static string Cmd(string[] cmd)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.AutoFlush = true;
            for (int i = 0; i < cmd.Length; i++)
            {
                p.StandardInput.WriteLine(cmd[i].ToString());
            }
            p.StandardInput.WriteLine("exit");
            string strRst = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            return strRst;
        }

        // 初始化数据
        public string InitialDB()
        {
            try
            {
                //string connectionStr = ConfigurationManager.ConnectionStrings["dbConnectionString"].ConnectionString;

                // 还原脚本命令
                //string SqlPath = AppDomain.CurrentDomain.BaseDirectory + "AllData.sql";
                //string sqlCmd = $"sqlcmd -S . -d RecordDB -i {SqlPath}";

                string delCmd = $"sqlcmd -S . -d master -Q \"DROP DATABASE RecordDB\"";

                // 还原bak命令
                string bakPath = AppDomain.CurrentDomain.BaseDirectory + "RecordDB.bak";
                string bakCmd = $"sqlcmd -S . -d master -Q \"RESTORE DATABASE RecordDB from disk = '{bakPath}' with replace\"";
                string[] cmds = new string[] {
                    delCmd,
                bakCmd
            };
                return Cmd(cmds);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return "";
        }

        private async void WaitForm1_Load(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要初始化数据", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;
            await Task.Run(() => { return InitialDB(); });
            progressPanel1.Caption = "数据初始化完毕!";
            Task.Delay(1000).Wait();
            Application.Exit();
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }

        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }

        #endregion Overrides
    }
}