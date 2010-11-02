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
    public partial class MapLoader : Form
    {
        string selected_level;
        public MapLoader()
        {
            selected_level = Properties.Settings.Default.level;
            InitializeComponent();
        }

        private void MapLoader_Load(object sender, EventArgs e)
        {
            string[] dirs = System.IO.Directory.GetDirectories("Maps\\");
            for (int i = 0; i < dirs.Count(); i++)
            {
                string name = dirs[i].Substring(dirs[i].LastIndexOf('\\') + 1);
                listBox1.Items.Add(name);
                if ( name == selected_level )
                    listBox1.SelectedIndex = i;
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selected_level = listBox1.SelectedItem.ToString();
            
            Properties.Settings.Default.level = selected_level;
            Properties.Settings.Default.Save();

            this.Close();
        }
    }
}
