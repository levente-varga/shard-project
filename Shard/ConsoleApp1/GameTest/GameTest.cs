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

            int displayWidth = Bootstrap.getDisplay().getWidth();
            int displayHeight = Bootstrap.getDisplay().getHeight();
            Random random = new Random();

            for (int i = 0; i < 307; i += 4)
            {
                notes.Add(new Note(i, new Point(
                    random.Next(displayWidth / 2) + displayWidth / 4,
                    random.Next(displayHeight / 2) + displayHeight / 4)));

                notes.Add(new Note(i + 1.5, new Point(
                    random.Next(displayWidth / 2) + displayWidth / 4,
                    random.Next(displayHeight / 2) + displayHeight / 4)));

                notes.Add(new Note(i + 3, new Point(
                    random.Next(displayWidth / 2) + displayWidth / 4,
                    random.Next(displayHeight / 2) + displayHeight / 4)));
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
                    break;
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
