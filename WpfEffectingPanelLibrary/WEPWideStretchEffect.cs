using System;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WpfEffectingPanelLibrary
{
    public class WEPWideStretchEffect : WEPDefaultEffect
    {
        private double canvasWidth = 0;

        public override void DrawEffectImage(ImageBrush currentImage, ImageBrush nextImage,
            ref System.Windows.Forms.Panel nextPanel, ref Canvas canvas)
        {
            DoubleAnimation animation = null;

            if (storyboard == null) 
            {
                storyboard = new Storyboard();
                storyboard.Name = "WideStretch";
            }
            storyboard.Completed -= animation2_Completed;
            storyboard.Completed += animation1_Completed;

            this.canvas = canvas;
            this.nextImage = nextImage;
            this.nextPanel = nextPanel;
            this.canvas.Background = currentImage;

            canvasWidth = canvas.Width;

            animation = new DoubleAnimation
            {
                From = canvasWidth,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(1000)
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Width"));
            storyboard.Children.Add(animation);

            this.canvas.BeginStoryboard(storyboard);
        }

        private void animation1_Completed(object sender, EventArgs e)
        {
            DoubleAnimation animation;

            storyboard.Completed -= animation1_Completed;
            storyboard.Completed += animation2_Completed;

            canvas.Background = this.nextImage;

            animation = new DoubleAnimation
            {
                From = 0,
                To = canvasWidth,
                Duration = TimeSpan.FromMilliseconds(1000),
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Width"));
            storyboard.Children.Add(animation);

            canvas.BeginStoryboard(storyboard);
        }

        private void animation2_Completed(object sender, EventArgs e)
        {
            if (nextPanel != null)
            {
                nextPanel.Visible = true;
                nextPanel.Refresh();
            }
        }
    }
}

