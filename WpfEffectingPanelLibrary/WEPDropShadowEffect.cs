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
    public class WEPDropShadowEffect : WEPDefaultEffect
    {
        public override void DrawEffectImage(BitmapSource current, BitmapSource next, EffectingPanel effectingPanel)
        {
            //effectingPanel.Effect = null;

            //// 現在の画像
            //effectingPanel.image1.Source = current;

            //DropShadowEffect dropShadowEffect = new DropShadowEffect();
            ////dropShadowEffect.Color = ;

            //effectingPanel.Effect = dropShadowEffect;
            //Thread.Sleep(500);

            //// 次の画像
            //effectingPanel.image1.Source = next;
            //Thread.Sleep(500);
        }
    }
}
