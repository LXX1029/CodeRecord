using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RisCaptureLib
{
    /// <summary>
    /// ToolBarControl.xaml 的交互逻辑
    /// </summary>
    public partial class ToolBarControl : UserControl
    {
        public ToolBarControl()
        {
            InitializeComponent();
        }
        #region 自定义事件
        public event EventHandler CancelHandler;
        public event EventHandler OkHandler;
        #endregion

        /// <summary>
        /// 截屏取消
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CancelHandler?.Invoke(sender, null);
        }

        /// <summary>
        /// 截屏保存
        /// </summary>
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            OkHandler?.Invoke(sender, null);
        }
    }
}
