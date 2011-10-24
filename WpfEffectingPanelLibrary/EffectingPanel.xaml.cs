using System;
using System.Collections;
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

using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace WpfEffectingPanelLibrary
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class EffectingPanel : UserControl
    {
        public enum EffectType { Fading, Rotating, L2RSliding, Random, None };

        private ArrayList effectList = null;
        private Random random = null;

        public EffectingPanel()
        {
            InitializeComponent();
        }

        public void Transition(ref System.Windows.Forms.Panel current, ref System.Windows.Forms.Panel next)
        {
            Transition(ref current, ref next, EffectType.Random);
        }

        public void Transition(ref System.Windows.Forms.Panel current, ref System.Windows.Forms.Panel next, EffectType type)
        {
            WEPImageCapture imageCapture = new WEPImageCapture();

            Bitmap currentBitmap = null;
            Bitmap nextBitmap = null;
            EPDefaultEffect effect = null;

            try
            {
                currentBitmap = imageCapture.GetPreviousCapturedImage(current, current.Name + ".bmp", false);    // 遷移前Panelをキャプチャ
                nextBitmap = null;

                string nextBitmapPath = next.Name + ".bmp";

                if (System.IO.File.Exists(nextBitmapPath))
                {
                    nextBitmap = new Bitmap(nextBitmapPath);
                }
                else
                {
                    nextBitmap = imageCapture.GetPreviousCapturedImage(next, nextBitmapPath, true);              // 初回のみ
                }

                //this.BringToFront();

                this.Visibility = Visibility.Visible;   // effectスタート
                current.Visible = false;

                if (type == EffectType.Random)
                {
                    type = (EffectType)random.Next(effectList.Count);
                }

                if (type == EffectType.None)
                {
                    effect = new EPDefaultEffect();
                }
                else
                {
                    effect = effectList[(int)type] as EPDefaultEffect;
                }

                effect.DrawEffectImage(currentBitmap, nextBitmap, this);

                next.Visible = true;
                next.Refresh();

                this.Visibility = Visibility.Hidden;   // effect終わり

                currentBitmap.Dispose();
                nextBitmap.Dispose();
            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //public void DrawEffectImage(System.Windows.Forms.Panel current, System.Windows.Forms.Panel next, EffectType type)
        //{
        //    WEPImageCapture imageCapture = new WEPImageCapture();
        //    Bitmap currentBitmap = imageCapture.GetPreviousCapturedImage(current, current.Name + ".bmp", true);
        //    BitmapSource currentBitmapSource = ToBitmapSource(currentBitmap);

        //    this.image1.Source = currentBitmapSource;

        //    currentBitmap.Dispose();
        //}

        //[DllImport("gdi32.dll")]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool DeleteObject(IntPtr hObject);

        //static BitmapSource ToBitmapSource(Bitmap bitmap)
        //{
        //    IntPtr ptr = bitmap.GetHbitmap();

        //    BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
        //        ptr, IntPtr.Zero, Int32Rect.Empty,
        //        BitmapSizeOptions.FromEmptyOptions());

        //    DeleteObject(ptr);

        //    return bitmapSource;
        //}
    }
}
