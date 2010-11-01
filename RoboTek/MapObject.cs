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
    class MapObject : IComparable
    {

        static string[] dirs = { "0", "90", "180", "270" };
        protected List<Image>[] walk_img = new List<Image>[4];
        protected int current_sprite = 0;
        protected int dir = 0;
        protected int x = 0;
        protected int y = 0;

        protected int offset_x = 0;
        protected int offset_y = 0;

        protected int image_offset_x = 0;
        protected int image_offset_y = 0;


        protected int level = 0;

        protected Map map;
        protected String name;

        public MapObject(Map m, string name)
        {
            this.name = name;
            map = m;
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

            image_offset_x = -walk_img[0][0].Width / 2+1;
            image_offset_y = -walk_img[0][0].Height / 2-44; // This is sooo bad

        }

        public string getName()
        {
            return name;
        }

        public int getX()
        {
            return x;
        }

        public int getY()
        {
            return y;
        }

        public void setPos(int nx, int ny, int nlevel)
        {
            level = nlevel;
            x = nx;
            y = ny;
        }

        public virtual void setDir(int new_dir ) 
        {
            dir = new_dir;
           
        }

        public virtual void Update()
        {
        }
        public virtual void Draw(Graphics g)
        {
            g.DrawImage(walk_img[dir][current_sprite],
                   new Rectangle((x + y) * map.half_tile_width + image_offset_x, (x - y) * map.half_tile_height + offset_y + image_offset_y - level * 22, map.tile_width, walk_img[dir][current_sprite].Height),
                   new Rectangle(walk_img[dir][current_sprite].Width / 2 - map.half_tile_width - offset_x, 0, map.tile_width, walk_img[dir][current_sprite].Height),
                   GraphicsUnit.Pixel);

        }


        int IComparable.CompareTo(object obj)
        {
            MapObject mo = (MapObject)obj;
            int ydif = mo.y-y;
            if ( ydif != 0)
                return ydif;
            
            int xdif = x - mo.x;
            if (xdif != 0)
                return xdif;

            return level - mo.level;
        }

    }
}
