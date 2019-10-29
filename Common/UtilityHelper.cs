using System;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using static Common.ExceptionHelper;
namespace Common
{
    /// <summary>
    /// 通用方法帮助类
    /// </summary>
    public sealed class UtilityHelper
    {
        #region 将BiteMapImage 转成byte[]
        /// <summary>
        /// 将BiteMapImage 转成byte[]
        /// </summary>
        /// <param name="bmp">BitmapImage</param>
        /// <returns>byte[]</returns>
        public static byte[] BitMapImageToByteArray(BitmapImage bmp)
        {
            if (bmp == null) return null;
            Stream smarket = bmp.StreamSource;
            if (smarket == null || smarket.Length == 0) return null;
            byte[] bytearray = null;
            CatchException(() =>
            {
                smarket.Position = 0;
                using (BinaryReader br = new BinaryReader(smarket))
                    bytearray = br.ReadBytes((int)smarket.Length);
            });
            return bytearray;
        }
        #endregion

        #region 将BitMapSource 转成 BitMapImage
        /// <summary>
        /// 将BitMapSoruce 转成 BitMapImage
        /// </summary>
        /// <param name="source">BitmapSource</param>
        /// <returns>BitmapImage</returns>
        public static BitmapImage BitMapSoruceToBitMapImage(BitmapSource source)
        {
            if (source == null) return null;
            BitmapImage bImg = null;
            CatchException(() =>
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                MemoryStream memoryStream = new MemoryStream();
                bImg = new BitmapImage();
                encoder.Frames.Add(BitmapFrame.Create(source));
                encoder.Save(memoryStream);
                bImg.BeginInit();
                bImg.StreamSource = new MemoryStream(memoryStream.ToArray());
                bImg.EndInit();
                memoryStream.Close();
            });
            return bImg;
        }
        #endregion

        #region 将byte[]转成Bitmap
        /// <summary>
        /// byte[]转成Bitmap
        /// </summary>
        /// <param name="bytes">byte[]</param>
        /// <returns>Bitmap</returns>
        public static Bitmap ConvertByteToImg(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            Bitmap img = null;
            CatchException(() =>
            {
                /*
                 * 使用using(MemoryStream ms = new MemoryStream(bytes)){img = new Bitmap(ms);}
                 * 出现 Method  异常对象名称： Int32 SelectActiveFrame(System.Drawing.Imaging.FrameDimension, Int32)异常
                 */
                MemoryStream ms = new MemoryStream(bytes);
                ms.Seek(0, SeekOrigin.Begin);
                img = new Bitmap(ms);
            });
            return img;
        }
        #endregion

        #region 将byte[]转换为Zip压缩包
        /// <summary>
        /// 将byte[]转换为Zip压缩包
        /// </summary>
        /// <param name="zipByte">文件二进制字节数组</param>
        /// <param name="zipName">文件包名称</param>
        /// <returns>Task</returns>
        public static async Task ConvertByteToZip(byte[] zipByte, string zipName)
        {
            if (zipByte == null || zipByte.Length == 0) return;
            await Task.Run(() =>
            {
                CatchException(() =>
                {
                    string fileName = $"{zipName}.zip";
                    string downLoadPath = $"{AppDomain.CurrentDomain.BaseDirectory}DownFiles\\";
                    string downLoadFilePath = downLoadPath;
                    if (!Directory.Exists(downLoadPath))
                        Directory.CreateDirectory(downLoadPath);
                    downLoadPath += fileName;
                    using (FileStream fs = new FileStream(downLoadPath, FileMode.Create, FileAccess.Write))
                        fs.Write(zipByte, 0, zipByte.Length);
                    Process.Start(downLoadFilePath);
                });
            });
        }
        #endregion

        #region 文件转成字节
        /// <summary>
        /// 文件转成字节
        /// </summary>
        /// <param name="fullpath">文件路径</param>
        /// <returns>byte[]</returns>
        public static byte[] ConvertFileToByte(string fullpath)
        {
            if (VerifyHelper.IsEmptyOrNullOrWhiteSpace(fullpath) || File.Exists(fullpath) == false) return null;
            byte[] imagebytes = null;
            CatchException(() =>
            {
                FileStream fs = new FileStream(fullpath, FileMode.Open, FileAccess.Read);
                using (BinaryReader br = new BinaryReader(fs))
                {
                    imagebytes = new byte[fs.Length];
                    imagebytes = br.ReadBytes(Convert.ToInt32(fs.Length));
                }
            });
            return imagebytes;
        }
        #endregion

        #region 图片转成字节数组
        /// <summary>
        ///  Image转成字节
        /// </summary>
        /// <param name="image">System.Drawing.Image 控件</param>
        /// <returns>byte[]</returns>
        public static byte[] ConvertImgToByte(Image image)
        {
            if (image == null) return null;
            byte[] imgBytes = null;
            CatchException(() =>
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                    imgBytes = new byte[ms.Length];
                    ms.Position = 0;
                    ms.ReadAsync(imgBytes, 0, Convert.ToInt32(ms.Length));
                }
            });
            return imgBytes;
        }
        #endregion

        #region 获取屏幕大小
        /// <summary>
        /// 获取屏幕大小
        /// Item1-mainSize
        /// Item2-mainLocation
        /// Item3-subSize
        /// Item4-subLocation
        /// Item5-totalSize
        /// </summary>
        public static (Size, Point, Size, Point, Size) GetWorkingAreaSize(double widthRate = 0.9, double heightRate = 0.9)
        {
            using (System.Drawing.Graphics graphics = System.Drawing.Graphics.FromHwnd(IntPtr.Zero))
            {
                // 获取屏幕Dpi
                float dpiX = graphics.DpiX;
                float dpiY = graphics.DpiY;
            }

            Rectangle rect = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            Size totalSize = new Size(rect.Size.Width, rect.Size.Height);
            Size mainSize = new Size((int)(totalSize.Width * 0.98), (int)(totalSize.Height * 0.98));
            Point mainLocation = new Point((totalSize.Width - mainSize.Width) / 2, (totalSize.Height - mainSize.Height) / 2);
            Size subSize = new Size((int)(totalSize.Width * widthRate), (int)(totalSize.Height * heightRate));
            Point subLocation = new Point((totalSize.Width - subSize.Width) / 2, (totalSize.Height - subSize.Height) / 2);
            return (mainSize, mainLocation, subSize, subLocation, totalSize);
        }

        #endregion 获取屏幕大小

        #region 操作配置文件

        /// <summary>
        /// 获取Configuration 中的setting值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <returns>string</returns>
        public static string GetConfigurationKeyValue(string key) => ConfigurationManager.AppSettings[key];

        /// <summary>
        /// 设置Configuration 中的setting值
        /// </summary>
        /// <param name="key">关键字</param>
        /// <param name="value">值</param>
        /// <returns>bool</returns>
        public static bool SetConfigurationKeyValue(string key, string value)
        {
            try
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings"); // 重新加载新的配置文件
                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteException("SetConfigurationKeyValue：" + ex.Message);
            }
            return false;
        }

        #endregion 操作配置文件

        #region MD5操作

        // 创建Key
        public static string GenerateKey()
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }

        /// <summary>
        /// MD5解密
        /// </summary>
        /// <param name="pToDecrypt">字符串</param>
        /// <param name="sKey">md5Key</param>
        /// <returns>string</returns>
        public static string MD5Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// 加密密码
        /// </summary>
        /// <param name="pwd">密码</param>
        /// <returns>string</returns>
        public static string MD5Encrypt(string pwd)
        {
            byte[] upwd = ASCIIEncoding.ASCII.GetBytes(pwd);
            using MD5 md5 = MD5.Create();
            byte[] mdpwdByte = md5.ComputeHash(upwd);
            string mdpwdString = ASCIIEncoding.ASCII.GetString(mdpwdByte);
            return mdpwdString;
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="pToEncrypt">字符串</param>
        /// <param name="sKey">md5Key</param>
        /// <returns>string</returns>
        public static string MD5Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        #endregion MD5操作

        #region 获取本地IP

        /// <summary>
        /// 获取本机Ip
        /// 注意IPv6或者Ipv4
        /// </summary>
        /// <returns>string</returns>
        public static string GetLocalIP()
        {
            string localIP = null;
            // 获取本地IP
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in ipEntry.AddressList)
            {
                if (ip.AddressFamily.Equals(AddressFamily.InterNetwork))
                {
                    if (ip != null) { localIP = ip.ToString(); }
                    break;
                }
            }
            if (string.IsNullOrEmpty(localIP)) { throw new Exception("读取IP为空"); }
            return localIP;
        }

        #endregion 获取本地IP

        #region 程序启动路径
        /// <summary>
        /// 程序启动路径
        /// </summary>
        public static string AppLaunchPath { get; } = AppDomain.CurrentDomain.BaseDirectory;
        #endregion

        #region 枚举操作
        public static string GetDescription(Enum value)
        {
            if (value == null)
                throw new Exception("参数异常");

            string description = value.ToString();
            var fieldInfo = value.GetType().GetField(description);
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            return description;
        }
        #endregion

        #region 激活已打开的进程
        /// <summary>
        /// 激活已打开的进程
        /// </summary>
        public static void RaiseOtherProcess()
        {
            Process proc = Process.GetCurrentProcess();
            foreach (Process otherProc in
                Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName))
            {
                if (proc.Id != otherProc.Id)
                {
                    IntPtr hWnd = otherProc.MainWindowHandle;
                    if (IsIconic(hWnd))
                    {
                        ShowWindowAsync(hWnd, 9);
                    }
                    SetForegroundWindow(hWnd);
                    break;
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        #endregion 激活已打开窗口
    }
}