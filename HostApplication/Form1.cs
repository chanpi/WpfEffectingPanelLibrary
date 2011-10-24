using System;
using System.Collections;
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
        private ArrayList panelList = null;
        private int panelIndex = 0;
        private int panelCount = 0;

        public Form1()
        {
            InitializeComponent();

            //effectingPanel1.DrawEffectImage(panel1, panel2, WpfEffectingPanelLibrary.EffectingPanel.EffectType.Fading);
            effectingPanel1.Transition(ref panel1, ref panel2);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Move(object sender, EventArgs e)
        {

        }

        private void Form1_ResizeEnd(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {

        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}
