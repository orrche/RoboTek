using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboTek
{
    partial class Challange
    {
        /* 
         * Använd dessa kommandon för att styra gubben
         *  forward();
         *  jump();
         *  turn_left();
         *  turn_right();
         *  activate();   // För att aktivera/avaktivera en blå yta
         *  f1();     // Anropar dessa funktioner och återgår sedan 
         *  f2();
         * 
         * Använd funktionerna f1 och f2 för att optimera koden.
         * Inte tillåtet att skapa egna funktioner
         */

        void f1()
        {
        }

        void f2()
        {
            forward();
            forward();
            turn_left();
            forward();
            forward();

        }

        public void Run()
        {
            f2();
        }
    }
}
