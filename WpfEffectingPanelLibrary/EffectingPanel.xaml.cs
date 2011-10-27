using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        public enum EffectType { Fading, WideStretch, Random, None };

        private WEPImageCapture imageCapture = null;
        private ArrayList effectList = null;
        private Random random = null;

        public EffectingPanel()
        {
            InitializeComponent();
            
            this.Visibility = Visibility.Visible;

            // 画面キャプチャ専用クラス
            imageCapture = new WEPImageCapture();
            // エフェクト効果を行うクラスのインスタンスを生成
            CreateEffectInstances();

            random = new Random();
        }

        private void CreateEffectInstances()
        {
            if (effectList != null)
            {
                effectList.RemoveRange(0, effectList.Count);
            }
            else
            {
                effectList = new ArrayList();
            }

            effectList.Add(new WEPFadingEffect());
            effectList.Add(new WEPWideStretchEffect());
        }

        public void Transition(ref System.Windows.Forms.Panel current, ref System.Windows.Forms.Panel next)
        {
            Transition(ref current, ref next, EffectType.Random);
        }

        public void Transition(ref System.Windows.Forms.Panel current, ref System.Windows.Forms.Panel next, EffectType type)
        {
            BitmapSource currentBitmapSource = null;
            BitmapSource nextBitmapSource = null;
            ImageBrush currentImage = null;
            ImageBrush nextImage = null;

            WEPDefaultEffect effect = null;

            try
            {
                // 現在のPanelの画像を取得
                currentBitmapSource = GetBitmapSource(current, false);

                // 次に表示するPanleの画像を取得
                string nextBitmapPath = next.Name + ".bmp";
                if (System.IO.File.Exists(nextBitmapPath))
                {
                    System.Drawing.Imaging.BitmapData bitmapData = null;
                    Bitmap nextBitmap = null;
                    try
                    {
                        nextBitmap = new Bitmap(nextBitmapPath);
                        bitmapData = nextBitmap.LockBits(
                            new System.Drawing.Rectangle(0, 0, nextBitmap.Width, nextBitmap.Height),
                            System.Drawing.Imaging.ImageLockMode.ReadOnly,
                            System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        nextBitmapSource = BitmapSource.Create(
                            nextBitmap.Width, nextBitmap.Height, 96, 96, PixelFormats.Bgra32, null,
                            bitmapData.Scan0, nextBitmap.Width * nextBitmap.Height * 4, bitmapData.Stride);
                    }
                    catch (SystemException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        if (bitmapData != null)
                        {
                            nextBitmap.UnlockBits(bitmapData);
                        }
                    }
                }
                else
                {
                    nextBitmapSource = GetBitmapSource(next, true);         // 初回のみ
                }

                if (type == EffectType.Random)
                {
                    type = (EffectType)random.Next(effectList.Count);
                }

                if (type == EffectType.None)
                {
                    effect = new WEPDefaultEffect();
                }
                else
                {
                    effect = effectList[(int)type] as WEPDefaultEffect;
                }

                Console.WriteLine(type.ToString());

                currentImage = new ImageBrush(currentBitmapSource);
                nextImage = new ImageBrush(nextBitmapSource);
                canvas.Width = current.Width;
                canvas.Height = current.Height;
                Console.WriteLine(canvas.Width.ToString() + " " + canvas.Height.ToString());

                //this.Visibility = Visibility.Visible;                     // effectスタート
                current.Visible = false;

                effect.DrawEffectImage(currentImage, nextImage, ref next, ref canvas);

                //this.Visibility = Visibility.Hidden;                        // effect終わり

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
