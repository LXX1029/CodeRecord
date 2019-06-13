using System;
using System.Windows.Media.Imaging;

namespace RisCaptureLib
{
    public class ScreenCaputredEventArgs : EventArgs
    {
        public ScreenCaputredEventArgs(BitmapSource bmp)
        {
            Bmp = bmp;
        }

        public BitmapSource Bmp
        {
            get;
            private set;
        }
    }
}