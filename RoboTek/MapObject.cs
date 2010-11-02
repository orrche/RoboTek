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
        protected bool walkable;

        protected static Region clip_region = null;
        protected static Region ground_clip_region = null; // For convenience when the artist (me) doesn't manage to follow the rules

        protected Region clip = null;
        public MapObject(Map m, string name)
        {
            init(m, name, false);
        }

        public MapObject(Map m, string name, bool is_walkable)
        {
            init(m, name, is_walkable);
        }

        public void init(Map m, string name, bool is_walkable)
        {
            walkable = is_walkable;
            this.name = name;
            map = m;

            // Initializing the clip region 
            if (clip_region == null)
            {
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddLines(new Point[] { new Point(-map.half_tile_width, 0), new Point(0, map.half_tile_height), new Point(map.half_tile_width, 0), new Point(map.half_tile_width, -200), new Point(-map.half_tile_width, -200) });
                clip_region = new Region(path);

                // Assuming that the ground thing isn't set either
                System.Drawing.Drawing2D.GraphicsPath path2 = new System.Drawing.Drawing2D.GraphicsPath();
                path2.AddLines(new Point[] { new Point(-map.half_tile_width, 0), new Point(0, map.half_tile_height), new Point(map.half_tile_width, 0), new Point(map.half_tile_width, -map.level_height), new Point(0, -map.level_height - map.half_tile_height) , new Point(-map.half_tile_width, -map.level_height) });
                ground_clip_region = new Region(path2);
            }

            if (walkable)
                clip = ground_clip_region;
            else
                clip = clip_region;

            // Loading the sprites
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

            image_offset_x = -(walk_img[0][0].Width / 2);
            image_offset_y = -walk_img[0][0].Height / 2 - 44; // This is sooo bad

        }
        
        public int getLevel()
        {
            return level;
        }

        public bool at(int x, int y)
        {
            return this.x == x && this.y == y;
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

        public virtual void Activate()
        {

        }

        public virtual bool Activatable()
        {
            return false;
        }

        public virtual void Update()
        {
        }
        public virtual void Draw(Graphics g)
        {
            clip.Translate((x + y) * map.half_tile_width, (x - y) * map.half_tile_height - level * map.level_height);
            g.Clip = clip;
            
            g.DrawImageUnscaled(walk_img[dir][current_sprite],
                (x + y) * map.half_tile_width + image_offset_x + offset_x,
                (x - y) * map.half_tile_height + offset_y + image_offset_y - level * map.level_height
            );
            clip.Translate(-(x + y) * map.half_tile_width, -((x - y) * map.half_tile_height - level * map.level_height));

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
