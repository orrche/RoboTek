﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

namespace RoboTek
{
    class Map
    {
        public int tile_width = 84;
        public int tile_height = 42;
        public int level_height = 22;

        public int half_tile_width;
        public int half_tile_height;

        bool draw_guide_lines = false;
        bool sort_queued = false;
        bool in_draw = false;

        Pen pen = new Pen(Brushes.Black);
        Player dude;

        List<MapObject> objs = new List<MapObject>();
        public Map(string name)
        {

            half_tile_height = tile_height / 2;
            half_tile_width = tile_width / 2;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Maps\\" + name + "\\main.xml");


            for (int dataNodes = 0; dataNodes < xmlDoc.ChildNodes.Count; dataNodes++)
            {

                XmlNode dataNode = xmlDoc.ChildNodes[dataNodes];
                if (dataNode.Name == "map")
                {
                    for (int childCount = 0; childCount < dataNode.ChildNodes.Count; childCount++)
                    {
                        XmlNode mapobj = dataNode.ChildNodes[childCount];
                        
                        if (mapobj.Name == "mapobject")
                        {
                            bool walkable = false;
                            if ( mapobj.Attributes["walkable"] != null && mapobj.Attributes["walkable"].InnerText == "true")
                                walkable = true;


                            MapObject obj = new MapObject(this, mapobj.Attributes["name"].InnerText, walkable);
                            obj.setPos(Convert.ToInt32(mapobj.Attributes["x"].InnerText),
                                Convert.ToInt32(mapobj.Attributes["y"].InnerText),
                                Convert.ToInt32(mapobj.Attributes["level"].InnerText));
                            objs.Add(obj);
                        }
                        if (mapobj.Name == "player")
                        {
                            Player obj = new Player(this, mapobj.Attributes["name"].InnerText);
                            obj.setPos(Convert.ToInt32(mapobj.Attributes["x"].InnerText),
                                Convert.ToInt32(mapobj.Attributes["y"].InnerText),
                                Convert.ToInt32(mapobj.Attributes["level"].InnerText));
                            objs.Add(obj);

                            if (mapobj.Attributes["name"].InnerText == "gubbe")
                                dude = obj;
                        }
                        if (mapobj.Name == "lightpad")
                        {
                            LightPad obj = new LightPad(this, mapobj.Attributes["name"].InnerText);
                            obj.setPos(Convert.ToInt32(mapobj.Attributes["x"].InnerText),
                                Convert.ToInt32(mapobj.Attributes["y"].InnerText),
                                Convert.ToInt32(mapobj.Attributes["level"].InnerText));
                            objs.Add(obj);

                        }
                    }
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

        public Player getPlayer()
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
