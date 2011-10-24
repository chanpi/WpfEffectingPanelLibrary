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

        private WEPImageCapture imageCapture = null;
        private ArrayList effectList = null;
        private Random random = null;

        public EffectingPanel()
        {
            InitializeComponent();
            imageCapture = new WEPImageCapture();
        }

        public void Transition(ref System.Windows.Forms.Panel current, ref System.Windows.Forms.Panel next)
        {
            Transition(ref current, ref next, EffectType.Random);
        }

        // 1. Windows FormのPanelを受取り、Panelの画像をキャプチャしてBitmapオブジェクトを取得
        // 2. BitmapオブジェクトをWPFで扱えるようにBitmapSourceオブジェクトに変換
        // 3. BitmapSourceでエフェクトを実行
        // 4. EffectingPanelをHiddenにする
        public void Transition(ref System.Windows.Forms.Panel current, ref System.Windows.Forms.Panel next, EffectType type)
        {
            BitmapSource currentBitmapSource = null;
            BitmapSource nextBitmapSource = null;
            EPDefaultEffect effect = null;

            try
            {
                currentBitmapSource = GetBitmapSource(current, false);      // 遷移前Panelをキャプチャ

                string nextBitmapPath = next.Name + ".bmp";

                if (System.IO.File.Exists(nextBitmapPath))
                {
                    nextBitmapSource = new BitmapImage();       // BitmapSource（abstruct）とBitmapImageは継承関係にある
                    ((BitmapImage)nextBitmapSource).BeginInit();
                    ((BitmapImage)nextBitmapSource).UriSource = new Uri(next.Name, UriKind.RelativeOrAbsolute);
                    ((BitmapImage)nextBitmapSource).EndInit();
                }
                else
                {
                    nextBitmapSource = GetBitmapSource(next, true);         // 初回のみ
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

                effect.DrawEffectImage(currentBitmapSource, nextBitmapSource, this);

                next.Visible = true;
                next.Refresh();

                this.Visibility = Visibility.Hidden;   // effect終わり

                Console.WriteLine("*** Effect end ***");

            }
            catch (SystemException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public BitmapSource GetBitmapSource(System.Windows.Forms.Panel panel, Boolean firstTime)
        {
            Bitmap currentBitmap = imageCapture.GetCapturedImage(panel, panel.Name + ".bmp", firstTime);
            BitmapSource currentBitmapSource = ToBitmapSource(currentBitmap);
            currentBitmap.Dispose();

            return currentBitmapSource;
        }

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DeleteObject(IntPtr hObject);

        static BitmapSource ToBitmapSource(Bitmap bitmap)
        {
            IntPtr ptr = bitmap.GetHbitmap();

            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                ptr, IntPtr.Zero, Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(ptr);

            return bitmapSource;
        }
    }
}
