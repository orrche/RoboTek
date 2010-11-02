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
    partial class Challange
    {
        Map _map;
        Form1 _form1;
        public bool cheating = false;


        public List<int> line_numbers = new List<int>();
        public Challange(Map m, Form1 f1)
        {
            _map = m;
            _form1 = f1;

        }
        public void _Counter()
        {
            StackTrace st = new StackTrace(true);

            int x = st.GetFrames().Count() - 1;
            int n = 2;
            string function_name = "";
            while (function_name != "Run")
            {
                function_name = st.GetFrame(n).GetMethod().Name;
                int linenumber = st.GetFrame(n).GetFileLineNumber();
                if (!line_numbers.Contains(linenumber))
                {
                    line_numbers.Add(linenumber);
                    //MessageBox.Show("Found " + function_name + " called at: " + linenumber + " in: "+ st.GetFrame(n).GetFileName());
                }
                if (!(function_name == "Run" || function_name == "f1" || function_name == "f2"))
                {
                    if ( !cheating )
                        MessageBox.Show("CHEATING !\r\nYour only allowed to use predefined functions");
                    cheating = true;
                }
                n++;

            }
            

        }
        public void _wait()
        {
            while (!_map.getPlayer().isIdle())
            {
                System.Threading.Thread.Sleep(80);
            }
        }

        public void forward()
        {
            _wait();
            _Counter();
            _map.getPlayer().moveForward();
        }

        public void turn_left()
        {
            _wait();
            _Counter();
            _map.getPlayer().setDir((_map.getPlayer().getDir()+3) % 4);
        }

        public void turn_right()
        {
            _wait();
            _Counter();
            _map.getPlayer().setDir((_map.getPlayer().getDir() + 1) % 4);
        }

        public void jump()
        {
            _wait();
            _Counter();
            _map.getPlayer().Jump();
        }

        public void activate()
        {
            _wait();
            _Counter();
            _map.getPlayer().Activate();
        }
    }
}
