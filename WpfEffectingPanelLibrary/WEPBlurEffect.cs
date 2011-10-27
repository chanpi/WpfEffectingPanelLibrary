using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfEffectingPanelLibrary
{
    public class WEPBlurEffect : WEPDefaultEffect
    {
        public override void DrawEffectImage(ImageBrush currentImage, ImageBrush nextImage,
            ref System.Windows.Forms.Panel nextPanel, ref Canvas canvas)
        {
            this.canvas = canvas;
            this.nextImage = nextImage;
            this.nextPanel = nextPanel;
            this.canvas.Background = currentImage;

        }
    }
}
