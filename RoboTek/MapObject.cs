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
    class MapObject
    {
        List<Image>[] walk_img = new List<Image>[4];
        string[] dirs = { "0", "90", "180", "270" };
        int current_sprite = 0;
        int dir = 0;
        int x = 0;
        int y = 0;

        bool moving = false;

        public MapObject(string name)
        {

            for (int i = 0; i < 4; i++)
            {
                walk_img[i] = new List<Image>();
                int num;

                num = 0;
                while (System.IO.File.Exists(String.Format("Sprites\\{0}\\{0}.walk.pov.{2}.{1:000}.png", name, num, dirs[i])))
                {
                    walk_img[i].Add(Image.FromFile(String.Format("Sprites\\{0}\\{0}.walk.pov.{2}.{1:000}.png", name, num, dirs[i])));
                    num++;
                }
            }

        }

        public void setDir(int new_dir ) 
        {
            dir = new_dir;
            current_sprite = 0;
            moving = true;
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(walk_img[dir][current_sprite], 10, 10);

            if (moving)
            {
                current_sprite++;
                if (walk_img[dir].Count <= current_sprite)
                {
                    current_sprite = 0;
                    moving = false;
                }
            }

        }
    }
}
