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
        public List<int> line_numbers = new List<int>();
        public Challange(Map m, Form1 f1)
        {
            _map = m;
            _form1 = f1;

        }
        public void Counter()
        {
            StackTrace st = new StackTrace(true);

            int x = st.GetFrames().Count() - 1;
            int n = 1;
            string function_name = "";
            function_name = st.GetFrame(n).GetMethod().Name;
            int linenumber = st.GetFrame(n).GetFileLineNumber();
            while (function_name != "Run")
            {
                if (!line_numbers.Contains(linenumber))
                {
                    line_numbers.Add(linenumber);
                    //MessageBox.Show("Found " + function_name + " called at: " + linenumber);
                }
                n++;
                function_name = st.GetFrame(n).GetMethod().Name;
                linenumber = st.GetFrame(n).GetFileLineNumber();

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
            Counter();
            _map.getPlayer().moveForward();
        }

    }
}
