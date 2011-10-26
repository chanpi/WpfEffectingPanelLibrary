using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Threading;

namespace WpfEffectingPanelLibrary
{
    public class WEPBlurEffect : WEPDefaultEffect
    {
        public override void DrawEffectImage(BitmapSource current, BitmapSource next, EffectingPanel effectingPanel)
        {
            //ImageBrush imageBrush = new ImageBrush();
            //imageBrush.ImageSource = current;
            //effectingPanel.MyRectangle.Fill = imageBrush;

            //effectingPanel.Effect = null;

            //// 現在の画像
            ////effectingPanel.image1.Source = current;

            //BlurEffect blurEffect = new BlurEffect();
            //blurEffect.Radius = 20;
            //blurEffect.KernelType = KernelType.Box;

            //effectingPanel.Effect = blurEffect;
            Thread.Sleep(1000);

            // 次の画像
            //effectingPanel.image1.Source = next;
            Thread.Sleep(1000);
        }
    }
}
