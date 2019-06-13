﻿using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System;

namespace RisCaptureLib
{
    internal class MaskWindow : Window
    {
        private MaskCanvas innerCanvas;
        private Bitmap screenSnapshot;
        private Timer timeOutTimmer;
        private readonly ScreenCaputre screenCaputreOwner;

        //截图显示尺寸label
        System.Windows.Controls.Label label = null;
        //截图显示按键
        ToolBarControl toolBarControl = null;
        //截图保存图片
        private System.Drawing.Bitmap m_bmpLayerCurrent;


        public MaskWindow(ScreenCaputre screenCaputreOwner)
        {
            this.screenCaputreOwner = screenCaputreOwner;
            Ini();
            innerCanvas.OnMove += DrawShowSize;
        }

        private void Ini()
        {

            //ini normal properties
            //Topmost = true;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            ShowInTaskbar = false;

            //set bounds to cover all screens
            var rect = SystemInformation.VirtualScreen;
            Left = rect.X;
            Top = rect.Y;
            Width = rect.Width;
            Height = rect.Height;

            //set background 
            screenSnapshot = HelperMethods.GetScreenSnapshot();
            if (screenSnapshot != null)
            {
                var bmp = screenSnapshot.ToBitmapSource();
                bmp.Freeze();
                Background = new ImageBrush(bmp);
            }

            //ini canvas
            innerCanvas = new MaskCanvas
            {
                MaskWindowOwner = this
            };
            Content = innerCanvas;

        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            //鼠标右键双击取消
            if (e.RightButton == MouseButtonState.Pressed && e.ClickCount >= 2)
            {
                CancelCaputre();
            }

            CreatLabel(e.GetPosition(innerCanvas));
            label.Visibility = Visibility.Visible;
            if (toolBarControl != null)
            {
                toolBarControl.Visibility = Visibility.Hidden;

            }
        }

        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (timeOutTimmer != null && timeOutTimmer.Enabled)
            {
                timeOutTimmer.Stop();
                timeOutTimmer.Start();
            }
            //设置左上角label和右下角toolbar鼠标跟随
            Rect temRect = innerCanvas.GetSelectionRegion();
            if (temRect == Rect.Empty)
            {
                return;
            }
            label.Content = $"选中区域大小:{temRect.Width}×{ temRect.Height } 宽:{temRect.Width} 高:{temRect.Height}";
            if (label != null)
            {
                Canvas.SetLeft(label, temRect.X);
                Canvas.SetTop(label, temRect.Y - 25);
            }
            if (toolBarControl != null)
            {
                Canvas.SetLeft(toolBarControl, temRect.X + temRect.Width - 75);
                Canvas.SetTop(toolBarControl, temRect.Y + temRect.Height);
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            CreatToolBar(e.GetPosition(innerCanvas));
            toolBarControl.Visibility = Visibility.Visible;

        }

        //创建提示选中区域大小控件
        private void CreatLabel(System.Windows.Point location)
        {
            if (label == null)
            {
                label = new System.Windows.Controls.Label
                {
                    VerticalAlignment = VerticalAlignment.Bottom,
                    Background = Config.RectLabelBackground,
                    Foreground = Config.RectLabelForeground
                };
                innerCanvas.Children.Add(label);

            }
            label.Content = GetLabelContent();
            //label.Foreground = (System.Windows.Media.Brush)Colors.Crimson;
            label.Height = 25;
            Canvas.SetLeft(label, location.X);
            Canvas.SetTop(label, location.Y - 25);
        }

        private void CreatToolBar(System.Windows.Point location)
        {
            if (toolBarControl == null)
            {
                toolBarControl = new ToolBarControl();
                innerCanvas.Children.Add(toolBarControl);
                Canvas.SetLeft(toolBarControl, location.X - 70);
                Canvas.SetTop(toolBarControl, location.Y);
            }
            toolBarControl.OkHandler += (m, n) =>
            {
                innerCanvas.finishAction();
            };
            toolBarControl.CancelHandler += (m, n) =>
            {
                CancelCaputre();
            };
        }
        private string GetLabelContent()
        {
            string strContent = "";
            return strContent;
        }

        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Escape)
            {
                CancelCaputre();
            }
        }

        private void CancelCaputre()
        {
            screenCaputreOwner.OnScreenCaputreCancelled(null);
            Close();

        }

        internal void OnShowMaskFinished(Rect maskRegion)
        {

        }

        internal void ClipSnapshot(Rect clipRegion)
        {
            BitmapSource caputredBmp = CopyFromScreenSnapshot(clipRegion);
            if (caputredBmp != null)
            {
                screenCaputreOwner.OnScreenCaputred(null, caputredBmp);
            }
            //close mask window
            Close();
        }


        internal BitmapSource CopyFromScreenSnapshot(Rect region)
        {
            if (region.Width.Equals(0.0) || region.Height.Equals(0.0))
            {
                return null;
            }
            var sourceRect = region.ToRectangle();
            var destRect = new Rectangle(0, 0, sourceRect.Width, sourceRect.Height);

            if (screenSnapshot != null)
            {
                var bitmap = new Bitmap(sourceRect.Width, sourceRect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.DrawImage(screenSnapshot, destRect, sourceRect, GraphicsUnit.Pixel);
                }

                return bitmap.ToBitmapSource();
            }

            return null;
        }

        private void SaveCaptureAction()
        {
            m_bmpLayerCurrent = innerCanvas.GetSnapBitmap();
            if (m_bmpLayerCurrent == null)
            {
                return;
            }
            System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();
            string mydocPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            saveDlg.InitialDirectory = mydocPath/* + "\\"*/;
            saveDlg.Filter = "Bitmap(*.bmp)|*.bmp|JPEG(*.jpg)|*.jpg";
            saveDlg.FilterIndex = 2;
            saveDlg.FileName = "SView截图";
            if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                switch (saveDlg.FilterIndex)
                {
                    case 1:
                        m_bmpLayerCurrent.Clone(new System.Drawing.Rectangle(0, 0, m_bmpLayerCurrent.Width, m_bmpLayerCurrent.Height),
                            System.Drawing.Imaging.PixelFormat.Format24bppRgb).Save(saveDlg.FileName,
                            System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 2:
                        m_bmpLayerCurrent.Save(saveDlg.FileName,
                            System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                }
            }

        }


        internal System.Drawing.Bitmap CopyBitmapFromScreenSnapshot(Rect region)
        {
            if (region.Width.Equals(0.0) || region.Height.Equals(0.0))
            {
                return null;
            }
            var sourceRect = region.ToRectangle();
            var destRect = new Rectangle(0, 0, sourceRect.Width, sourceRect.Height);

            if (screenSnapshot != null)
            {
                var bitmap = new Bitmap(sourceRect.Width, sourceRect.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.DrawImage(screenSnapshot, destRect, sourceRect, GraphicsUnit.Pixel);
                }

                return bitmap;
            }

            return null;
        }

        public void Show(int timeOutSecond, System.Windows.Size? defaultSize)
        {
            if (timeOutSecond > 0)
            {
                if (timeOutTimmer == null)
                {
                    timeOutTimmer = new Timer();
                    timeOutTimmer.Tick += OnTimeOutTimmerTick;
                }
                timeOutTimmer.Interval = timeOutSecond * 1000;
                timeOutTimmer.Start();
            }

            if (innerCanvas != null)
            {
                innerCanvas.DefaultSize = defaultSize;
            }

            Show();
            Focus();

        }

        private void OnTimeOutTimmerTick(object sender, System.EventArgs e)
        {
            timeOutTimmer.Stop();
            CancelCaputre();
        }

        public void DrawShowSize(Rect rec)
        {
            if (rec == Rect.Empty)
            {
                return;
            }
            var wX = rec.Width;
            var hY = rec.Height;
            label.Content = $"选中区域大小:{wX}×{ hY } 宽:{wX} 高:{hY}";

        }


        protected override void OnMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseRightButtonUp(e);
            if (label != null)
                label.Visibility = Visibility.Hidden;
            if (toolBarControl != null)
                toolBarControl.Visibility = Visibility.Hidden;
        }
    }
}
