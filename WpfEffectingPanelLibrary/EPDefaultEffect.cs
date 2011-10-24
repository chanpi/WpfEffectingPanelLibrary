﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
    public class EPDefaultEffect
    {
        public virtual void DrawEffectImage(Bitmap current, Bitmap next, EffectingPanel effectingPanel)
        {

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
