using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboTek
{
    class Player : Movable
    {
        public Player(Map m, string name)
            : base(m, name)
        {

        }

        public override void Activate()
        {
            List <MapObject> objs = map.at(x,y);
            for (int i = 0; i < objs.Count; i++)
            {
                if ( objs[i] != this )
                    objs[i].Activate();
            }
        }


        public override bool Activatable()
        {
            return true;
        }
    }
}
