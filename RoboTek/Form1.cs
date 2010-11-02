using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace RoboTek
{
    public partial class Form1 : Form
    {
        Movable gubben;
        Map map = new Map();
        Bitmap bufl = null;
        Graphics g = null;

        List<TimeSpan> times = new List<TimeSpan>();
        Stopwatch sw;
        double avg = 0;

        bool kentDraw = true;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
           
            gubben = map.getPlayer();

            pf_Resize(null, null);
            pf.Paint += new PaintEventHandler(pf_Paint);
        }

        void pf_Paint(object sender, PaintEventArgs e)
        {
            if (!kentDraw)
            {
                if (sw == null)
                    sw = Stopwatch.StartNew();
                sw.Reset();
                sw.Start();

                if (bufl != null)
                {
                    e.Graphics.ResetClip();
                    e.Graphics.Clear(Color.White);
                    map.Draw(e.Graphics);

                }
                sw.Stop();
                if (times.Count >= 60)
                {
                    avg = 0;
                    foreach (TimeSpan ts in times)
                    {
                        avg += ts.TotalMilliseconds;
                    }
                    avg /= times.Count;
                    times.Clear();
                }
                times.Add(sw.Elapsed);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!kentDraw)
                pf.Refresh();
            else
            {
                if (sw == null)
                    sw = Stopwatch.StartNew();
                sw.Reset();
                sw.Start();

                if (bufl != null)
                {
                    g.ResetClip();
                    g.Clear(Color.White);
                    map.Draw(g);

                    pf.CreateGraphics().DrawImageUnscaled(bufl, 0, 0);
                }
                sw.Stop();
                if (times.Count >= 60)
                {
                    avg = 0;
                    foreach (TimeSpan ts in times)
                    {
                        avg += ts.TotalMilliseconds;
                    }
                    avg /= times.Count;
                    times.Clear();
                }
                times.Add(sw.Elapsed);
            }
            this.Text = string.Format("Gubbe: [{0}, {1}, {2}] {4} draw: frametime: {3}", gubben.getX(), gubben.getY(), gubben.getLevel(), avg, kentDraw ? "kent" : "stefan");
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
            else if (e.KeyCode == Keys.K)
            {
                kentDraw = !kentDraw;
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
