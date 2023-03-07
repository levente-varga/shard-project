using GameTest;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Shard
{
    class GameTest : Game, InputListener
    {
        GameObject background;
        List<GameObject> asteroids;
        List<Note> notes;

        double beatPerMinute;
        double beatPerSecond;
        double offsetSeconds;
        double beat;

        public override void initialize()
        {
            Bootstrap.getInput().addListener(this);
            createBackground();

            asteroids = new List<GameObject>();
            notes = new List<Note>();

            for (int i = 0; i < 100; i++)
            {
                notes.Add(new Note(i * 2, new Point(Bootstrap.getDisplay().getWidth() / 2, Bootstrap.getDisplay().getHeight() / 2), 50, 25, 3));
            }

            beatPerMinute = 131.0;
            beatPerSecond = beatPerMinute / 60;
            offsetSeconds = 0.64;

            Bootstrap.getSound().PlayMusic("clocks.wav");
        }

        public override void update()
        {
            beat = Bootstrap.getSound().MusicPosition * beatPerSecond - offsetSeconds;

            notes.ForEach(note => { note.CurrentBeat = beat; });
            
            Bootstrap.getDisplay().showText("FPS: " + Bootstrap.getSecondFPS() + " / " + Bootstrap.getFPS(), 10, 10, 12, 255, 255, 255);
            Bootstrap.getDisplay().showText($"Position: {Bootstrap.getSound().MusicPosition} / {Bootstrap.getSound().MusicLength}", 10, 30, 12, 255, 255, 255);

            Bootstrap.getDisplay().showText($"Beat: {(int)beat + (int)(beat * 100) / 25 % 4 * 25 * 0.01 }", 10, 50, 12, 255, 255, 255);
        }

        public override int getTargetFrameRate()
        {
            return 75;

        }
        public void createBackground()
        {
            GameObject ship = new Spaceship();
            Random rand = new Random();
            int offsetx = 0, offsety = 0;

            GameObject asteroid;

            background = new GameObject();
            background.Transform.SpritePath = getAssetManager().getAssetPath ("background2.jpg");
            background.Transform.X = 0;
            background.Transform.Y = 0;
            background.Visible = true;
            background.Layer = 0;
        }

        public void handleInput(InputEvent ie)
        {
            switch (ie.Type)
            {
                case InputEventType.MouseDown:
                    Console.WriteLine ("Pressing button " + ie.Button);

                    if (ie.Button == 1)
                    {
                        Asteroid asteroid = new Asteroid();
                        asteroid.Transform.X = ie.X;
                        asteroid.Transform.Y = ie.Y;
                        asteroids.Add (asteroid);
                    }
                    
                    if (ie.Button == 3)
                    {
                        foreach (GameObject ast in asteroids) {
                            ast.ToBeDestroyed = true;
                        }

                        asteroids.Clear();
                    }
                    break;
            }
        }
    }
}
