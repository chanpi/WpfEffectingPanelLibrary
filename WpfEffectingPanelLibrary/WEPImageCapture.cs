using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WpfEffectingPanelLibrary
{
    public class WEPImageCapture
    {
        public WEPImageCapture()
        {
        }

        public Bitmap GetPreviousCapturedImage(Panel panel, string filePath, Boolean firstTime)
        {
            Rectangle rectangle;
            Bitmap bitmap = null;
            ArrayList controls = null;

            try
            {
                rectangle = panel.RectangleToScreen(panel.Bounds);
                bitmap = new Bitmap(rectangle.Width, rectangle.Height, PixelFormat.Format32bppArgb);
                if (firstTime)
                {
                    panel.DrawToBitmap(bitmap, panel.Bounds);   // 再帰的にコンテナ及びコントロールをキャプチャ

                    controls = GetAllControls(panel);
                    controls.Reverse(); // 背面から
                    foreach (Control c in controls)
                    {
                        Rectangle rectangle2 = c.Bounds;
                        Control control = c;
                        while (control.Bounds.Location != panel.Bounds.Location)
                        {
                            rectangle2.X += control.Parent.Bounds.Location.X;
                            rectangle2.Y += control.Parent.Bounds.Location.Y;
                            control = control.Parent;
                        }
                        c.DrawToBitmap(bitmap, rectangle2);
                    }
                }
                else
                {
                    CaptureControls(panel, ref bitmap);
                }
                bitmap.Save(filePath, ImageFormat.Bmp);    // 保存する場合
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return bitmap;
        }

        private ArrayList GetAllControls(Control top)
        {
            ArrayList arrayList = new ArrayList();

            foreach (Control c in top.Controls)
            {
                arrayList.AddRange(GetAllControls(c));
                arrayList.Add(c);
            }
            return arrayList;
        }

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private extern static bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        /// <summary>
        /// コントロールのイメージを取得する
        /// </summary>
        /// <param name="ctrl">キャプチャするコントロール</param>
        /// <returns>取得できたイメージ</returns>
        public Bitmap CaptureControls(Control control, ref Bitmap bitmap)
        {
            Graphics g;
            IntPtr hdc;

            g = Graphics.FromImage(bitmap);
            hdc = g.GetHdc();
            PrintWindow(control.Handle, hdc, 0);
            g.ReleaseHdc(hdc);
            g.Dispose();
            return bitmap;
        }
    }
}
