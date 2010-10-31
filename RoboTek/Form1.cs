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
        MapObject gubben;
        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            gubben = new MapObject("gubbe");
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Bitmap bufl = new Bitmap(pf.Width, pf.Height);
            using (Graphics g = Graphics.FromImage(bufl))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, pf.Width, pf.Height));
                gubben.Draw(g);
                pf.CreateGraphics().DrawImageUnscaled(bufl, 0, 0);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                gubben.setDir(3);
            }
            else if (e.KeyCode == Keys.Right)
            {
                gubben.setDir(0);
            }
            else if (e.KeyCode == Keys.Down)
            {
                gubben.setDir(1);
            }
            else if (e.KeyCode == Keys.Left)
            {
                gubben.setDir(2);
            }
        }
    }
}
