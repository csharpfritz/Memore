using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using OpenMacroBoard.SDK;

namespace MemoRE.Console
{
    public static class IconLoader
    {
        public static KeyBitmap LoadIconByName(string name, bool active)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"MemoRE.Console.icons.{name}";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (var bitmap = (Bitmap)Image.FromStream(stream))
            {
                var raw = ConvertBitmapToRgb24(bitmap);
                if (!active)
                {
                    for (int i = 0; i < raw.Length; i++)
                        raw[i] /= 2;
                }
                return new KeyBitmap(72, 72, raw);
            }
        }

        private static byte[] ConvertBitmapToRgb24(Bitmap bitmap)
        {
            const int iconSize = 72;

            if (bitmap.Width != iconSize || bitmap.Height != iconSize)
                throw new NotSupportedException("Unsupported bitmap dimensions");

            BitmapData data = null;
            try
            {
                data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

                var managedRGB = new byte[iconSize * iconSize * 3];

                if (data.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Marshal.Copy(data.Scan0, managedRGB, 0, managedRGB.Length);
                    return managedRGB;
                }
                else if (data.PixelFormat == PixelFormat.Format32bppArgb)
                {
                    var tempRgb32 = new byte[iconSize * iconSize * 4];
                    Marshal.Copy(data.Scan0, tempRgb32, 0, tempRgb32.Length);

                    var len = iconSize * iconSize;
                    for (int i = 0; i < len; i++)
                    {
                        var pt = i * 3;
                        var ps = i * 4;

                        double alpha = (double)tempRgb32[ps + 3] / 255f;
                        managedRGB[pt + 0] = (byte)Math.Round(tempRgb32[ps + 0] * alpha);
                        managedRGB[pt + 1] = (byte)Math.Round(tempRgb32[ps + 1] * alpha);
                        managedRGB[pt + 2] = (byte)Math.Round(tempRgb32[ps + 2] * alpha);
                    }

                    return managedRGB;
                }
                else
                {
                    throw new NotSupportedException("Unsupported pixel format");
                }
            }
            finally
            {
                if (data != null)
                    bitmap.UnlockBits(data);
            }
        }
    }
}
