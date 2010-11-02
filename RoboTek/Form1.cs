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
        Map map = new Map("level1");

        List<TimeSpan> times = new List<TimeSpan>();
        Stopwatch sw;
        double avg = 0;

        public Form1()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
           

            pf.Paint += new PaintEventHandler(pf_Paint);
        }

        void pf_Paint(object sender, PaintEventArgs e)
        {
            if (sw == null)
                sw = Stopwatch.StartNew();
            sw.Reset();
            sw.Start();

            e.Graphics.ResetClip();
            e.Graphics.Clear(Color.White);
            map.Draw(e.Graphics);

            sw.Stop();
            if (times.Count >= 1000.0 / timer1.Interval)
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            pf.Refresh();
            this.Text = string.Format("Gubbe: [{0}, {1}, {2}] frametime/s: {3}ms", map.getPlayer().getX(), map.getPlayer().getY(), map.getPlayer().getLevel(), avg.ToString("N3"));
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            Movable gubben = map.getPlayer();
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
            else if (e.KeyCode == Keys.Enter)
            {
                gubben.Activate();
            }
        }
    }
}
