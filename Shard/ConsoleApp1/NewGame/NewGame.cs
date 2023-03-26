using NewGame;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Shard
{
    class NewGame : Game, InputListener
    {
        GameObject background;
        public override void Update()
        {
            Bootstrap.GetDisplay().ShowText($"Position: {Bootstrap.GetSound().MusicPosition} / {Bootstrap.GetSound().MusicLength}", 10, 30, 12, 255, 255, 255, 255);

            double beatPerMinute = 131.0;
            double beatPerSecond = beatPerMinute / 60;
            double offsetSeconds = 0.64;
            double beat = Bootstrap.GetSound().MusicPosition * beatPerSecond - offsetSeconds;

            Bootstrap.GetDisplay().ShowText($"Beat: {(int)beat + (int)(beat * 100) / 25 % 4 * 25 * 0.01}", 10, 50, 12, 255, 255, 255, 255);

        }

        public override int GetTargetFrameRate()
        {
            return 100;

        }
        public void createItem()
        {
            GameObject item = new Item();

            background = new GameObject();
            background.Transform.SpritePath = GetAssetManager().GetAssetPath("background2.jpg");
            background.Transform.X = 0;
            background.Transform.Y = 0;
            background.Visible = true;
            background.Layer = 0;
        }

        public override void Initialize()
        {
            Bootstrap.GetInput().AddListener(this);
            createItem();

            Bootstrap.GetSound().PlayMusic("clocks.wav");

        }

        public void HandleInput(InputEvent ie)
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
