using DevExpress.XtraSplashScreen;
using System;

namespace Common
{
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

        public enum SplashScreenCommand
        {
        }
    }
}