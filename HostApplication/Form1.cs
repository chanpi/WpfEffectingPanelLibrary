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

            panelList = new ArrayList();
            panelList.Add(panel1);
            panelList.Add(panel2);

            panel2.Visible = false;
            panelCount = panelList.Count;

            // WPFのエフェクト用UserControlは不可視で最前面にしておく
            //elementHost1.BringToFront();
            //elementHost1.Visible = false;
            elementHost1.SendToBack();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // 遷移前、遷移後のPanelとエフェクトのタイプを指定する
            Panel current = panelList[panelIndex] as Panel;
            if (++panelIndex >= panelCount)
            {
                panelIndex = 0;
            }
            Panel next = panelList[panelIndex] as Panel;

            // エフェクト時にelementHost1をVisibleにする必要がある
            //elementHost1.Visible = true;
            elementHost1.BringToFront();
            effectingPanel1.Transition(ref current, ref next);
            //effectingPanel1.Transition(ref current, ref next, WpfEffectingPanelLibrary.EffectingPanel.EffectType.WideStretch);
            elementHost1.SendToBack();
            //elementHost1.Visible = false;
        }

        #region EffecingPanelの描画を補助するイベントメソッド（Timer関連）

        // Resize開始
        private void Form1_Resize(object sender, EventArgs e)
        {
            TimerStop();
        }

        // Resizeや移動の完了
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            TimerStart();
            //myPanel.SetSize(this);
        }

        // 最大化・最小化に対応(Form1_ResizeEndでは最大化・最小化イベントに対応できない)
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            TimerStart();
            //effectingPanel1.SetSize(this);
        }

        // フォームの移動開始
        private void Form1_Move(object sender, EventArgs e)
        {
            TimerStop();
        }

        // Formが描画された瞬間Moveイベントが発生し、Form1_Moveが呼ばれTimerはストップするため、再度有効化する
        private void Form1_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            //elementHost1.BringToFront();
            //System.Threading.Thread.Sleep(500);
            //elementHost1.Visible = true;
            //elementHost1.Refresh();
            //Console.WriteLine("Visible");
            //System.Threading.Thread.Sleep(500);
            //elementHost1.Visible = false;
            //elementHost1.Refresh();
            //Console.WriteLine("Hidden");
            //System.Threading.Thread.Sleep(500);
            //elementHost1.Visible = true;
            //elementHost1.Refresh();
            //Console.WriteLine("Visible");


            TimerStart();
        }

        private void TimerStart()
        {
            if (!timer1.Enabled)
            {
                timer1.Start();
            }
        }

        private void TimerStop()
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
            }
        }
        #endregion
    }
}
