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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Threading;

namespace WpfEffectingPanelLibrary
{
    /// <summary>
    /// UserControl1.xaml の相互作用ロジック
    /// </summary>
    public partial class EffectingPanel : UserControl
    {
        public enum EffectType { Blur, DropShadow, /*Emboss, OuterGlow,*/ Random, None };

        private WEPImageCapture imageCapture = null;
        private ArrayList effectList = null;
        private Random random = null;

        private Storyboard storyboard = null;
        private DispatcherTimer effectTimer = null;

        private System.Windows.Forms.Panel nextPanel = null;

        public EffectingPanel()
        {
            InitializeComponent();
            
            this.Visibility = Visibility.Visible;

            // 画面キャプチャ専用クラス
            imageCapture = new WEPImageCapture();
            // エフェクト効果を行うクラスのインスタンスを生成
            CreateEffectInstances();
            // エフェクトの時間を管理するタイマー
            SetEffectTimer();

            random = new Random();
        }

        private void SetEffectTimer()
        {
            if (effectTimer == null)
            {
                effectTimer = new DispatcherTimer(DispatcherPriority.Normal);
                effectTimer.Interval = new TimeSpan(0, 0, 2);
                effectTimer.Tick += new EventHandler(effectTimer_Tick);
            }
        }

        private void effectTimer_Tick(object sender, EventArgs e)
        {
            effectTimer.Stop();
            if (nextPanel != null)
            {
                nextPanel.Visible = true;
                nextPanel.Refresh();
            }
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

            // TODO!!!!!!!!!!!!!!!!!!!
            effectList.Add(new WEPBlurEffect());
            effectList.Add(new WEPBlurEffect());
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
            WEPDefaultEffect effect = null;

            try
            {
                currentBitmapSource = GetBitmapSource(current, false);      // 遷移前Panelをキャプチャ

                string nextBitmapPath = next.Name + ".bmp";

                if (System.IO.File.Exists(nextBitmapPath))
                {
                    nextBitmapSource = new BitmapImage();                   // BitmapSource（abstruct）とBitmapImageは継承関係にある
                    ((BitmapImage)nextBitmapSource).BeginInit();
                    ((BitmapImage)nextBitmapSource).UriSource = new Uri(next.Name, UriKind.RelativeOrAbsolute);
                    ((BitmapImage)nextBitmapSource).EndInit();
                }
                else
                {
                    nextBitmapSource = GetBitmapSource(next, true);         // 初回のみ
                }

                //this.Visibility = Visibility.Visible;                       // effectスタート
                effectTimer.Start();
                current.Visible = false;

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

                ImageBrush currentImage = new ImageBrush(currentBitmapSource);
                ImageBrush nextImage = new ImageBrush(nextBitmapSource);
                canvas.Width = current.Width;
                canvas.Height = current.Height;
                //canvas.Background = currentImage;

                //effect.DrawEffectImage(currentBitmapSource, nextBitmapSource, this);

                nextPanel = next;
                canvasEffectSample(currentImage, nextImage);

                //next.Visible = true;
                //next.Refresh();

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

        // TODO
        private void canvas_Loaded(object sender, RoutedEventArgs e)
        {

        }

        //private void canvasEffectSample(ImageBrush currentImage, ImageBrush nextImage)
        //{
        //    if (storyboard == null)
        //    {
        //        storyboard = new Storyboard();
        //        storyboard.Name = "Fading";
        //        storyboard.Completed += storyboard1_Completed;
        //    }

        //    DoubleAnimation animation;

        //    canvas.Background = currentImage;
        //    g_nextImage = nextImage;

        //    animation = new DoubleAnimation
        //    {
        //        From = 1,
        //        To = 0,
        //        Duration = TimeSpan.FromMilliseconds(1000),
        //        //RepeatBehavior = RepeatBehavior.Forever,
        //        AutoReverse = false
        //    };

        //    Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
        //    storyboard.Children.Add(animation);

        //    canvas.BeginStoryboard(storyboard);

        //    effectTimer.Start();
        //}

        //private ImageBrush g_nextImage;

        //private void storyboard1_Completed(object sender, EventArgs e)
        //{
        //    Console.WriteLine("next!!!!!!!!!!!!!");
        //    DoubleAnimation animation;

        //    canvas.Background = g_nextImage;

        //    animation = new DoubleAnimation
        //    {
        //        From = 0,
        //        To = 1,
        //        Duration = TimeSpan.FromMilliseconds(1000),
        //        //RepeatBehavior = RepeatBehavior.Forever,
        //        AutoReverse = false
        //    };

        //    Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
        //    storyboard.Children.Add(animation);

        //    canvas.BeginStoryboard(storyboard);
        //}

        private void canvasEffectSample(ImageBrush currentImage, ImageBrush nextImage)
        {
            if (storyboard == null)
            {
                storyboard = new Storyboard();
                storyboard.Name = "Fading";
            }

            DoubleAnimation animation1;
            DoubleAnimation animation2;

            canvas.Background = currentImage;

            animation1 = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(2000),
                //RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = false
            };
            animation2 = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(2000),
                //RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = false
            };

            Storyboard.SetTargetProperty(animation1, new PropertyPath("Opacity"));
            Storyboard.SetTargetProperty(animation2, new PropertyPath("Opacity"));
            storyboard.Children.Add(animation1);

            canvas.BeginStoryboard(storyboard);

            effectTimer.Start();
        }
    }
}
