using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboTek
{
    partial class Challange
    {
        void f1()
        {

            forward();
            forward();

        }

        void f2()
        {
            f1();
        }


        public void Run()
        {
            f2();

            Console.WriteLine("wtf");
        }
    }
}
