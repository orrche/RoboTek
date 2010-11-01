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
        // How do one make consts in C#? 
        static string[] dirs = { "0", "90", "180", "270" };
        static int[,] dir_calc = { { 1, 1 }, { -1, 1 }, { -1, -1 }, { 1, -1 } };
        static int[,] dir_dir = { { 1, 0 }, { 0, -1 }, { -1, 0 }, { 0, 1 } };

        List<Image>[] walk_img = new List<Image>[4];
        int current_sprite = 0;
        int dir = 0;
        int x = 0;
        int y = 0;

        int offset_x = 0;
        int offset_y = 0;

        int image_offset_x = 0;
        int image_offset_y = 0;

        bool moving = false;

        int level = 0;

        Map map;

        public MapObject(Map m, string name)
        {
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

        public void setDir(int new_dir ) 
        {
            if (!moving)
            {
                dir = new_dir;
                current_sprite = 0;
                moving = true;
            }
           
        }

        public void Draw(Graphics g)
        {
            g.DrawImage(walk_img[dir][current_sprite],
                   new Rectangle((x + y) * map.half_tile_width + image_offset_x, (x - y) * map.half_tile_height + offset_y + image_offset_y - level * 22, map.tile_width, walk_img[dir][current_sprite].Height),
                   new Rectangle(walk_img[dir][current_sprite].Width / 2 - map.half_tile_width - offset_x, 0, map.tile_width, walk_img[dir][current_sprite].Height),
                   GraphicsUnit.Pixel);

            g.DrawImage(walk_img[dir][current_sprite],
                   new Rectangle((x + y + 2*dir_calc[dir, 0]) * map.half_tile_width + image_offset_x, (x - y) * map.half_tile_height + offset_y + image_offset_y - level * 22, map.tile_width, walk_img[dir][current_sprite].Height),
                   new Rectangle(walk_img[dir][current_sprite].Width / 2 - map.half_tile_width - offset_x + dir_calc[dir, 0] * map.tile_width, 0, map.tile_width, walk_img[dir][current_sprite].Height),
                   //new Rectangle(walk_img[dir][current_sprite].Width / 2 - map.half_tile_width - offset_x, 0, map.tile_width, walk_img[dir][current_sprite].Height),
                   GraphicsUnit.Pixel);
            
            if (moving)
            {
                current_sprite++;
                offset_x = current_sprite * dir_calc[dir,0]*map.half_tile_width / walk_img[dir].Count;
                offset_y = current_sprite * dir_calc[dir,1]*map.half_tile_height / walk_img[dir].Count;
                if (walk_img[dir].Count <= current_sprite)
                {
                    current_sprite = 0;
                    x += dir_dir[dir, 0];
                    y += dir_dir[dir, 1];
                    map.ReSort(); // Causes problem because we are already in a draw cycle
                    offset_y = offset_x = 0;
                    moving = false;
                }
            }

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
