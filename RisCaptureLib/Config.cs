using System.Windows;
using System.Windows.Media;

namespace RisCaptureLib
{
    internal static class Config
    {
        //截取区域背景色
        public static Brush MaskWindowBackground = new SolidColorBrush(Color.FromArgb(120, 255, 255, 255));
        //截取区域边框色
        public static Brush SelectionBorderBrush = new SolidColorBrush(Colors.Red);
        public static Thickness SelectionBorderThickness = new Thickness(2.0);


        public static Brush RectLabelBackground = new SolidColorBrush(Colors.BlueViolet);
        public static Brush RectLabelForeground = new SolidColorBrush(Colors.WhiteSmoke);

    }
}