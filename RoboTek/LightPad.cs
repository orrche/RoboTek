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
    class LightPad : MapObject
    {
        // This really needs to be done better with a rewrite of MapObjects way of handling it.
        protected List<Image>[] off_img;
        protected List<Image>[] on_img = new List<Image>[4];
        public LightPad(Map m, string name)
            : base(m, name, true)
        {
            off_img = walk_img;

            on_img[0] = new List<Image>();
            on_img[0].Add(Image.FromFile(String.Format("Sprites\\{0}\\{0}.on.pov.{2}.{1:000}.png", name, 0, 0)));
        }

        public override void Activate()
        {
            if (walk_img == off_img)
                walk_img = on_img;
            else
                walk_img = off_img;

            base.Activate();
        }

        public override bool Activatable()
        {
            return true;
        }
    }
}
