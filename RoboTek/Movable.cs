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

        protected static Region[] sub_clip_region = new Region[2];
        int end_level;

        public Movable(Map m, string name)
            : base(m, name)
        {
            ghost = new Ghost(this, m);
            map.addObj(ghost);

            if (sub_clip_region[0] == null)
            {
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddLines(new Point[] { new Point(map.half_tile_width, 0), new Point(0, map.half_tile_height), new Point(map.half_tile_width, map.tile_height), new Point(map.tile_width, map.half_tile_height), new Point(map.tile_width, -200), new Point(map.half_tile_width, -200) });
                System.Drawing.Drawing2D.GraphicsPath path2 = new System.Drawing.Drawing2D.GraphicsPath();
                path2.AddLines(new Point[] { new Point(-map.half_tile_width, 0), new Point(0, map.half_tile_height), new Point(-map.half_tile_width, map.tile_height), new Point(-map.tile_width, map.half_tile_height), new Point(-map.tile_width, -200), new Point(-map.half_tile_width, -200) });

                System.Drawing.Drawing2D.GraphicsPath path3 = new System.Drawing.Drawing2D.GraphicsPath();
                path3.AddLines(new Point[] { new Point(-200, -200), new Point(0, map.half_tile_height), new Point(-map.half_tile_width, map.tile_height), new Point(-map.tile_width, map.half_tile_height), new Point(-map.tile_width, -200), new Point(-map.half_tile_width, -200) });
                
                sub_clip_region[0] = new Region(path2);
                sub_clip_region[1] = new Region(path);

                //sub_clip_region[0] = sub_clip_region[1] = clip_region;
            }
        }



        public virtual void DrawSecond(Graphics g)
        {
            Region cliping_reg = null;
            if (dir_calc[dir, 0] < 0)
                cliping_reg = sub_clip_region[0];
            else
                cliping_reg = sub_clip_region[1];

            cliping_reg.Translate((x + y) * map.half_tile_width, (x - y) * map.half_tile_height - level * map.level_height);
            g.Clip = cliping_reg;
            g.DrawImageUnscaled(walk_img[dir][current_sprite],
                (x + y) * map.half_tile_width + image_offset_x + offset_x,
                (x - y) * map.half_tile_height + offset_y + image_offset_y - level * map.level_height
            );
            cliping_reg.Translate(-(x + y) * map.half_tile_width, -((x - y) * map.half_tile_height - level * map.level_height));
        }
        public override void setDir(int new_dir)
        {

            if (!moving)
            {
                dir = new_dir;
            } 
        }

        public virtual void moveForward()
        {
            if (!moving)
            {
                current_sprite = 0;
                bool possible = true;
                List<MapObject> objs_at_dest = map.at(x + dir_dir[dir, 0], y + dir_dir[dir, 1]);

                // Change this if to possible = false, later when there is complete maps.
                if (objs_at_dest.Count == 0 && level == 0)
                    possible = true;
                else
                    possible = false;

                for (int i = 0; i < objs_at_dest.Count; i++)
                {
                    if (objs_at_dest[i].getLevel() == level || 
                        objs_at_dest[i].getLevel() == level + 1 || 
                        objs_at_dest[i].getLevel() == level + 2 || 
                        objs_at_dest[i].getLevel() == level + 3)
                    {
                        possible = false;
                        break;
                    }
                    if (objs_at_dest[i].getLevel() == level - 1)
                        possible = true;
                }
                if (possible)
                {
                    moving = true;
                    ghost.setPos(x + dir_dir[dir, 0], y + dir_dir[dir, 1], level);
                    map.ReSort();
                }
            } 
        }

        public virtual void Jump()
        {
            if (!moving)
            {
                current_sprite = 0;
                bool possible = true;
                int target_lvl = -99;
                List<MapObject> objs_at_dest = map.at(x + dir_dir[dir, 0], y + dir_dir[dir, 1]);
                for (int i = 0; i < objs_at_dest.Count; i++)
                {
                    int obj_level = objs_at_dest[i].getLevel();
                    if (obj_level == level + 1)
                    {
                        possible = false;
                        break;
                    }
                    if (obj_level < level + 1)
                    {
                        if (obj_level+1 > target_lvl)
                            target_lvl = obj_level+1;
                    }
                }
                if (objs_at_dest.Count == 0)
                {
                    if (level == 0)
                        possible = false;
                    target_lvl = 0;    
                }
                if (possible)
                {
                    if (target_lvl < level)
                        end_level = target_lvl;
                    else
                        end_level = level = target_lvl;
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
                    level = end_level;
                    map.ReSort();
                    offset_y = offset_x = 0;
                    moving = false;
                }
            }
        }

    }
}
