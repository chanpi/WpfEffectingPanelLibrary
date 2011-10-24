using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HostApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //effectingPanel1.DrawEffectImage(panel1, panel2, WpfEffectingPanelLibrary.EffectingPanel.EffectType.Fading);
            effectingPanel1.Transition(ref panel1, ref panel2);
        }
    }
}
