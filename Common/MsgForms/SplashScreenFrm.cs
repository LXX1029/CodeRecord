using System;
using DevExpress.XtraSplashScreen;

namespace Common
{
    /// <summary>
    /// SplashScreenFrm
    /// </summary>
    public partial class SplashScreenFrm : SplashScreen
    {
        public SplashScreenFrm()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        #region Overrides

        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion Overrides
    }
}