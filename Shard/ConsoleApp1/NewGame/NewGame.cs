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
            Bootstrap.getDisplay().showText($"Position: {Bootstrap.getSound().MusicPosition} / {Bootstrap.getSound().MusicLength}", 10, 30, 12, 255, 255, 255);

            double beatPerMinute = 131.0;
            double beatPerSecond = beatPerMinute / 60;
            double offsetSeconds = 0.64;
            double beat = Bootstrap.getSound().MusicPosition * beatPerSecond - offsetSeconds;

            Bootstrap.getDisplay().showText($"Beat: {(int)beat + (int)(beat * 100) / 25 % 4 * 25 * 0.01}", 10, 50, 12, 255, 255, 255);

        }

        public override int getTargetFrameRate()
        {
            return 100;

        }
        public void createItem()
        {
            GameObject item = new Item();

            background = new GameObject();
            background.Transform.SpritePath = getAssetManager().getAssetPath("background2.jpg");
            background.Transform.X = 0;
            background.Transform.Y = 0;
            background.Visible = true;
            background.Layer = 0;
        }

        public override void initialize()
        {
            Bootstrap.getInput().addListener(this);
            createItem();

            Bootstrap.getSound().PlayMusic("clocks.wav");

        }

        public void handleInput(InputEvent ie)
        {
            switch (ie.Type)
            {
                case InputEventType.MouseDown:
                    if (ie.Button == 1)
                    {
                        Console.WriteLine("Pressing button " + ie.Button);
                    }

                    if (ie.Button == 3)
                    {
                        Console.WriteLine("Pressing button " + ie.Button);
                    }
                    break;
            }
        }
    }
}
