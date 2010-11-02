using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RoboTek
{
    public partial class Form1 : Form
    {
        Movable gubben;
        Map map = new Map();
        Bitmap bufl = null;
        Graphics g = null;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
           
            gubben = map.getPlayer();

            pf_Resize(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (bufl != null)
            {
                g.ResetClip();
                g.Clear(Color.White);
                map.Draw(g);
                pf.CreateGraphics().DrawImageUnscaled(bufl, 0, 0);
            }
            this.Text = string.Format("Gubbe: [{0}, {1}, {2}]", gubben.getX(), gubben.getY(), gubben.getLevel());
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                gubben.setDir(3);
                gubben.moveForward();
            }
            else if (e.KeyCode == Keys.Right)
            {
                gubben.setDir(0);
                gubben.moveForward();
            }
            else if (e.KeyCode == Keys.Down)
            {
                gubben.setDir(1);
                gubben.moveForward();
            }
            else if (e.KeyCode == Keys.Left)
            {
                gubben.setDir(2);
                gubben.moveForward();
            }
            else if (e.KeyCode == Keys.Space)
            {
                gubben.Jump();
            }
        }

        private void pf_Resize(object sender, EventArgs e)
        {
            if (bufl != null)
                bufl.Dispose();
            bufl = new Bitmap(pf.Width, pf.Height);
            g = Graphics.FromImage(bufl);
        }
    }
}
