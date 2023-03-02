using NewGame;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Shard
{
    class NewGame : Game, InputListener
    {
        GameObject background;
        public override void update()
        {

        }

        public override int getTargetFrameRate()
        {
            return 100;

        }

        public void createShip()
        {

            GameObject ship = new Spaceship();
            Random rand = new Random();
            int offsetx = 0, offsety = 0;

        }

        public override void initialize()
        {
            Bootstrap.getInput().addListener(this);
            GameObject background = new Background();
            createShip();

        }

        public void handleInput(InputEvent inp, string eventType)
        {

            if (eventType == "MouseDown")
            {
                Console.WriteLine("Pressing button " + inp.Button);
            }

            if (eventType == "MouseDown" && inp.Button == 1)
            {
                Console.WriteLine("Pressing button " + inp.Button);
            }

            if (eventType == "MouseDown" && inp.Button == 3)
            {
                Console.WriteLine("Pressing button " + inp.Button);
            }


        }
    }
}
