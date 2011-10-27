//﻿using System;
//using System.Collections.Generic;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;

using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//using System.Drawing;
//using System.Runtime.InteropServices;
//using System.Windows.Interop;

using System.Threading;

namespace WpfEffectingPanelLibrary
{
    public class WEPDefaultEffect
    {
        protected System.Windows.Forms.Panel nextPanel = null;

        protected Canvas canvas = null;
        protected Storyboard storyboard = null;
        protected ImageBrush nextImage = null;

        //public virtual void DrawEffectImage(BitmapSource current, BitmapSource next, EffectingPanel effectingPanel)
        public virtual void DrawEffectImage(ImageBrush currentImage, ImageBrush nextImage, 
            ref System.Windows.Forms.Panel nextPanel, ref Canvas canvas)
        {
            this.nextPanel = nextPanel;
            this.canvas = canvas;
            this.nextImage = nextImage;
        }
    }
}
