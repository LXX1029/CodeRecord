using Common;
using DevExpress.Utils;
using DevExpress.XtraCharts;
using DataEntitys;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;

namespace DLCodeRecord.DevelopForms
{
    /// <summary>
    /// 统计窗体类
    /// </summary>
    public partial class DevelopReportFrm : BaseFrm
    {
        #region 属性
        private ChartControl chartControl = new ChartControl();
        #endregion 属性


        #region 构造函数
        public DevelopReportFrm()
        {
            InitializeComponent();

            #region 初始化设置
            this.Text = "统计";
            chartControl.Dock = DockStyle.Fill;
            this.Controls.Add(chartControl);
            #endregion 初始化设置

            #region 加载数据
            this.Load -= new EventHandler(DevelopReportFrm_Load);
            this.Load += new EventHandler(DevelopReportFrm_Load);
            #endregion 加载数据
        }
        #endregion

        #region 窗口加载
        private async void DevelopReportFrm_Load(object sender, EventArgs e)
        {
            var tuple = UtilityHelper.GetWorkingAreaSize();
            this.Size = tuple.Item3;
            this.Location = tuple.Item4;
            await LoadReportDate();
            InitialChartControl();
        }
        #endregion

        #region 加载报表数据

        private async Task LoadReportDate()
        {
            try
            {
                ShowSplashScreenForm(this, PromptHelper.D_LOADINGDATA);
                await Task.Delay(1000);
                CloseSplashScreenForm();
                // 获取数据
                IList<ClickCountReportEntity> reportList = base.UnityStatisticsFacade.GetClickCountReport().ToList();
                if (reportList.Count != 0)
                {
                    // 线
                    Series series1 = null;
                    // 点集合
                    List<SeriesPoint> SeriesPointList = null;
                    // 遍历排序后的集合
                    var result = from p in reportList
                                 group p by p.ParentTypeName into g
                                 select new
                                 {
                                     ParentName = g.Key,
                                     ClickCount = g.Sum(s => s.ClickCount),
                                     ParentId = g.Select(p => p.ParentTypeId).First()
                                 };

                    // 添加到控件显示
                    foreach (var i in result.OrderByDescending(o => o.ClickCount))
                    {
                        SeriesPointList = new List<SeriesPoint>();
                        SeriesPoint seriesPoint = new SeriesPoint(i.ParentName, new double[] { i.ClickCount });

                        List<ClickCountReportEntity> childrenTypeList =
                            (from c in reportList
                             where c.ParentTypeId == i.ParentId
                             select new ClickCountReportEntity
                             {
                                 ParentTypeName = c.ParentTypeName,
                                 SubTypeName = c.SubTypeName,
                                 ClickCount = c.ClickCount
                             }).ToList();
                        seriesPoint.Tag = childrenTypeList;
                        SeriesPointList.Add(seriesPoint);
                        series1 = new Series(i.ParentName + ":" + i.ClickCount, ViewType.Bar)
                        {
                            ShowInLegend = true,
                            ToolTipPointPattern = string.Format("次数：{0}", i.ClickCount),
                        };
                        series1.Points.AddRange(SeriesPointList.ToArray());
                        series1.Label.TextPattern = "{A}:{V:F1}";
                        chartControl.Series.Add(series1);
                    }
                }
                else
                {
                    this.Hide();
                    MsgHelper.ShowInfo(PromptHelper.D_NODATA);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                CatchLoadException(this, ex);
            }
        }

        /// <summary>
        /// 初始化ChartControl
        /// </summary>
        private void InitialChartControl()
        {
            // 设置标题
            chartControl.Titles.AddRange(new ChartTitle[] { new ChartTitle() { Text = "查询次数统计" } });
            // 设置chartControl 说明属性
            chartControl.Legend.Visibility = DefaultBoolean.True;
            chartControl.Legend.Border.Color = Color.Black;
            chartControl.Legend.MarkerVisible = true;

            //chartControl.BackColor = Color.LightBlue;
            chartControl.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.False;
            // 设置ToolTip可用
            chartControl.ToolTipEnabled = DevExpress.Utils.DefaultBoolean.True;
            // 设置ToolTip显示位置方式
            chartControl.ToolTipOptions.ToolTipPosition = new ToolTipMousePosition();

            // 提示控件
            ToolTipController toolTipController = new ToolTipController
            {
                Rounded = true,
                ShowBeak = true,
                // 设置显示位置
                ToolTipLocation = ToolTipLocation.TopRight,
                // 设置显示时间
                AutoPopDelay = 5000,
                // 单击关闭
                CloseOnClick = DefaultBoolean.True
            };
            toolTipController.BeforeShow += new ToolTipControllerBeforeShowEventHandler(ToolTipController_BeforeShow);
            chartControl.ToolTipController = toolTipController;

            ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
            System.Windows.Forms.MenuItem rotatedItem = new MenuItem("旋转", RotatedItem_Click);
            System.Windows.Forms.MenuItem LegendItem = new MenuItem("说明", RotatedItem_Click)
            {
                Checked = true
            };
            contextMenu.MenuItems.Add(rotatedItem);
            contextMenu.MenuItems.Add(LegendItem);
            chartControl.ContextMenu = contextMenu;
        }

        #endregion 加载报表数据

        #region 数据旋转事件

        /// <summary>
        /// 旋转
        /// </summary>
        private void RotatedItem_Click(object sender, EventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                string text = menuItem.Text;
                if (text == "旋转")
                {
                    bool rotated = !((XYDiagram)chartControl.Diagram).Rotated;
                    ((XYDiagram)chartControl.Diagram).Rotated = rotated;
                    menuItem.Checked = rotated;
                }
                if (text == "说明")
                {
                    bool visible = true;
                    if (chartControl.Legend.Visibility == DefaultBoolean.True)
                    {
                        visible = false;
                        chartControl.Legend.Visibility = DefaultBoolean.False;
                    }
                    else
                    {
                        visible = true;
                        chartControl.Legend.Visibility = DefaultBoolean.True;
                    }
                    menuItem.Checked = visible;
                }
            }
        }

        #endregion 数据旋转事件

        #region 构建toolTip 显示数据

        /// <summary>
        /// 鼠标悬浮图片
        /// </summary>
        /// <param name="toolTipChartDataSource">数据源</param>
        private Image CreateChart(List<ClickCountReportEntity> toolTipChartDataSource)
        {
            if (toolTipChartDataSource == null || toolTipChartDataSource.Count == 0) return null;
            ChartControl chart = new ChartControl();
            chart.BorderOptions.Visibility = DefaultBoolean.False;
            //chart.BackColor = Color.Yellow;
            chart.Series.Add(new Series("小版块", ViewType.Line));
            chart.DataSource = toolTipChartDataSource;
            chart.Series[0].ValueDataMembers.AddRange("ClickCount");
            chart.Series[0].ArgumentDataMember = "SubTypeName";
            //
            // 显示线---数值
            chart.Series[0].LabelsVisibility = DefaultBoolean.True;
            chart.Series[0].ShowInLegend = true;
            //chart.Series[0].View.Color = seriesColor;
            chart.Series[0].ArgumentScaleType = ScaleType.Auto;
            chart.Series[0].ValueScaleType = ScaleType.Numerical;
            chart.Legend.Visibility = DefaultBoolean.False;
            XYDiagram xyDiagram = chart.Diagram as XYDiagram;
            ChartTitle chartTitle = new ChartTitle() { Text = toolTipChartDataSource[0].ParentTypeName + " 小版块查询统计", Font = new Font("Tahoma", 12) };
            xyDiagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;
            xyDiagram.AxisY.MinorCount = 5;
            chart.Titles.Add(chartTitle);
            chart.Size = new System.Drawing.Size(900, 300);
            Image chartAsImage;
            using (MemoryStream stream = new MemoryStream())
            {
                chart.ExportToImage(stream, ImageFormat.Png);
                chartAsImage = new Bitmap(stream);
            }
            return chartAsImage;
        }

        /// <summary>
        /// ToolTip 显示之前发生，用于构建呈现
        /// </summary>
        private void ToolTipController_BeforeShow(object sender, ToolTipControllerShowEventArgs e)
        {
            if (sender is ToolTipController controller && controller.ActiveObject is SeriesPoint seriesPoint &&
               seriesPoint.Tag is List<ClickCountReportEntity> list && list.Count > 0)
            {
                string chartTitle = list[0].ParentTypeName;
                e.ToolTipImage = CreateChart(list);
                e.ToolTip = "";
            }
        }
        #endregion 构建toolTip 显示数据
    }
}