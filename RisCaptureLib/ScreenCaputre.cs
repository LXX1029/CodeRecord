using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RisCaptureLib
{
    public class ScreenCaputre
    {
        public event EventHandler<EventArgs> ScreenCaputreCancelled;

        public event EventHandler<ScreenCaputredEventArgs> ScreenCaputred;

        public void StartCaputre(int timeOutSeconds)
        {
            StartCaputre(timeOutSeconds, null);
        }

        public void StartCaputre(int timeOutSeconds, Size? defaultSize)
        {
            var mask = new MaskWindow(this);
            mask.Show(timeOutSeconds, defaultSize);
        }

        internal void OnScreenCaputreCancelled(object sender)
        {
            if (ScreenCaputreCancelled != null)
            {
                ScreenCaputreCancelled(sender, EventArgs.Empty);
            }
        }

        internal void OnScreenCaputred(object sender, BitmapSource caputredBmp)
        {
            if (ScreenCaputred != null)
            {
                ScreenCaputred(sender, new ScreenCaputredEventArgs(caputredBmp));
            }
        }
    }
}