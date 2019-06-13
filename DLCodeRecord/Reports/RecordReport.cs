using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using Common;
using DataEntitys;
using DevExpress.XtraPrinting.Drawing;

namespace DLCodeRecord.Reports
{
    public partial class RecordReport : DevExpress.XtraReports.UI.XtraReport
    {
        public RecordReport()
        {
            InitializeComponent();
        }

        public RecordReport(DevelopRecordEntity entity)
        {
            InitializeComponent();

            #region 添加水印
            Watermark.Text = "代码管理工具";
            Watermark.TextDirection = DirectionMode.ForwardDiagonal;
            Watermark.Font = new Font(Watermark.Font.FontFamily, 36, FontStyle.Italic);
            Watermark.ForeColor = Color.OrangeRed;
            Watermark.TextTransparency = 100;
            Watermark.ShowBehind = true;
            /*
            设置显示页码范围
            格式："1-3" / "1,4-5" /2,5
            */
            //Watermark.PageRange = "1-2";
            #endregion


            #region 初始化报表页面结构
            xrpbLogo.ImageUrl = UtilityHelper.AppLaunchPath + @"\Images\Bug.ico";
            xrpbLogo.SizeF = new SizeF(40, 40);
            #endregion
            this.xrlbTitle.Font = new Font("宋体", 15, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrlbTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrlbTitle.Text = entity?.Title;
            this.xrrtDesc.Text = entity?.Desc;
            this.xrrtDesc.Font = new Font("宋体", 12, FontStyle.Regular);

            this.xrlbuser.Text = entity?.UserName;
            this.xrlbParentName.Text = entity?.ParentTypeName;
            this.xrlbTypeName.Text = entity?.SubTypeName;
            this.xrlbCreatedTime.Text = entity?.CreatedTime.ToString();
            this.xrlbUpdatedTime.Text = entity?.UpdatedTime.ToString();
            // 是否显示图片
            if (entity?.BitMap != null)
            {
                XRLabel xrlbImg = new XRLabel();
                xrlbImg.Text = "效果图:";
                xrlbImg.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
                xrlbImg.LocationF = new PointF(10, 370);
                xrlbImg.SizeF = new SizeF(70, 30);


                XRPictureBox xrPb = new XRPictureBox();
                xrPb.WidthF = this.PageWidth - 50;
                xrPb.HeightF = (float)entity?.BitMap.Height;
                xrPb.Image = entity?.BitMap;
                xrPb.LocationF = new PointF(10, 400);
                xrPb.SizeF = new SizeF(590, 220);
                xrPb.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
                this.Detail.Controls.AddRange(new XRControl[] { xrlbImg, xrPb });
            }

            // 改变Detail中的label颜色
            foreach (XRControl control in this.Detail.Controls)
            {
                XRLabel lb = control as XRLabel;
                if (lb != null)
                    lb.ForeColor = Color.OrangeRed;
            }

        }
    }
}
