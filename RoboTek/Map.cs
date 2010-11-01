using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace RoboTek
{
    class Map
    {
        public int tile_width = 84;
        public int tile_height = 42;

        public int half_tile_width;
        public int half_tile_height;

        bool draw_guide_lines = false;
        bool sort_queued = false;
        bool in_draw = false;

        Pen pen = new Pen(Brushes.Black);
        MapObject dude;

        List<MapObject> objs = new List<MapObject>();
        public Map()
        {

            half_tile_height = tile_height / 2;
            half_tile_width = tile_width / 2;
            dude = new Movable(this, "gubbe");

            objs.Add(dude);

            for (int x = 0; x < 3; x++)
            {

                int level = 0;
                for (int i = 0; i < x + 1; i++)
                {
                    MapObject wall = new MapObject(this, "wall");
                    wall.setPos(4 + x, 0, i);
                    objs.Add(wall);

                }
            }

            ReSort();

        }

        public void addObj(MapObject obj)
        {
            objs.Add(obj);
        }

        public List<MapObject> at(int x, int y)
        {
            List<MapObject> at_objs = new List<MapObject>();

            for (int i = 0; i < objs.Count; i++)
            {
                if (objs[i].at(x, y))
                    at_objs.Add(objs[i]);
            }

            return at_objs;

        }

        public void ReSort()
        {
            if (in_draw)
                sort_queued = true;
            else
                objs.Sort();
        }

        public MapObject getPlayer()
        {
            return dude;
        }
        public void Draw(Graphics g)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].Update();                
            }

            in_draw = true;
            if (draw_guide_lines)
            {
                for (int y = -40; y < 40; y++)
                {
                    g.DrawLine(pen, 0, 22 + y * tile_height, tile_width * 20, 22 - tile_height * 20 + y * tile_height);
                    g.DrawLine(pen, 0, 22 + y * tile_height, tile_width * 20, 22 + tile_height * 20 + y * tile_height);
                }
            }

            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].Draw(g);
            }
            in_draw = false;
            if (sort_queued)
                ReSort();
        }
    }
}
