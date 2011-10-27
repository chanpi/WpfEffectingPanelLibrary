using System;
using System.Windows;

using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WpfEffectingPanelLibrary
{
    public class WEPFadingEffect : WEPDefaultEffect
    {
        public override void DrawEffectImage(ImageBrush currentImage, ImageBrush nextImage,
            ref System.Windows.Forms.Panel nextPanel, ref Canvas canvas)
        {
            DoubleAnimation animation = null;

            if (storyboard == null)
            {
                storyboard = new Storyboard();
                storyboard.Name = "Fading";
            }
            storyboard.Completed -= animation2_Completed;
            storyboard.Completed += animation1_Completed;

            this.canvas = canvas;
            this.nextImage = nextImage;
            this.nextPanel = nextPanel;
            this.canvas.Background = currentImage;

            animation = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(1000),
                //RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = false
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
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
                To = 1,
                Duration = TimeSpan.FromMilliseconds(1000),
                //RepeatBehavior = RepeatBehavior.Forever,
                AutoReverse = false
            };

            Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
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
