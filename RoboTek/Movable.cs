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
    class Movable : MapObject
    {

        // How do one make consts in C#? 
        static int[,] dir_calc = { { 1, 1 }, { -1, 1 }, { -1, -1 }, { 1, -1 } };
        static int[,] dir_dir = { { 1, 0 }, { 0, -1 }, { -1, 0 }, { 0, 1 } };

        Ghost ghost;

        protected bool moving = false;

        public Movable(Map m, string name)
            : base(m, name)
        {
            ghost = new Ghost(this, m);
            map.addObj(ghost);
        }



        public virtual void DrawSecond(Graphics g)
        {
            g.DrawImage(walk_img[dir][current_sprite],
                   new Rectangle((x + y + 2 * dir_calc[dir, 0]) * map.half_tile_width + image_offset_x, (x - y) * map.half_tile_height + offset_y + image_offset_y - level * 22, map.tile_width, walk_img[dir][current_sprite].Height),
                   new Rectangle(walk_img[dir][current_sprite].Width / 2 - map.half_tile_width - offset_x + dir_calc[dir, 0] * map.tile_width, 0, map.tile_width, walk_img[dir][current_sprite].Height),
                   GraphicsUnit.Pixel);
        }
        public override void setDir(int new_dir)
        {
            
            if (!moving)
            {
                dir = new_dir;
                current_sprite = 0;
                if (map.at(x + dir_dir[dir, 0], y + dir_dir[dir, 1]).Count == 0)
                {
                    moving = true;
                    ghost.setPos(x + dir_dir[dir, 0], y + dir_dir[dir, 1], level);
                    map.ReSort();
                }
            } 
        }
        public override void Update()
        {

            if (moving)
            {
                current_sprite++;
                offset_x = current_sprite * dir_calc[dir, 0] * map.half_tile_width / walk_img[dir].Count;
                offset_y = current_sprite * dir_calc[dir, 1] * map.half_tile_height / walk_img[dir].Count;
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

    }
}
