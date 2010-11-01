using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboTek
{
    class Ghost : MapObject
    {
        Movable parent;
        public Ghost(Movable obj, Map m)
            : base(m, obj.getName())
        {
            parent = obj;
        }

        public override void Update()
        {
            
        }

        public override void Draw(System.Drawing.Graphics g)
        {
            parent.DrawSecond(g);
        }
    }
}
