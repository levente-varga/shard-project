using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    internal class Note : GameObject, InputListener
    {
        double positionBeats;
        int markerStartSize;
        double fadeInDurationBeats;
        double fadeOutDurationBeats;
        double currentBeat;
        bool fired = false;

        public double CurrentBeat
        {
            set => currentBeat = value;
        }

        public double FadeInDurationBeats
        {
            set => fadeInDurationBeats = value > 0 ? value : 1;
        }

        public double FadeOutDurationBeats
        {
            set => fadeOutDurationBeats = value > 0 ? value : 1;
        }

        public Note(double positionBeat, Point position, int markerStartSize = 50, double fadeInDurationBeats = 2, double fadeOutDurationBeats = 0.5)
        {
            Transform.X = position.X;
            Transform.Y = position.Y;
            this.positionBeats = positionBeat;
            this.markerStartSize = markerStartSize;
            this.FadeInDurationBeats = fadeInDurationBeats;
            this.FadeOutDurationBeats = fadeOutDurationBeats;
            Layer = 3;

            Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("note.png");

            Debug.Log($"Note at {Transform.X}, {Transform.Y} with a size of {Transform.Wid} x {Transform.Ht} was created");

            Bootstrap.getInput().addListener(this);
        }

        public override void update()
        {
            double fadeInStart = positionBeats - fadeInDurationBeats;
            double fadeOutEnd  = positionBeats + fadeOutDurationBeats;
            Visible = fadeInStart < currentBeat && currentBeat < fadeOutEnd;
            
            if (!Visible) return;

            double diffBeat = currentBeat - positionBeats;

            if (diffBeat < 0) // Music is BEFORE this note
            {   
                double ratio = -diffBeat / fadeInDurationBeats;
                Alpha = (int)(255 * Math.Pow((1 - ratio), 2));
                Bootstrap.getDisplay().drawCircle(
                    (int)Transform.Centre.X, 
                    (int)Transform.Centre.Y, 
                    (int)(Transform.Wid / 2 + ratio * (markerStartSize - Transform.Wid / 2)),
                    255, 204, 85, Alpha);
            }
            else
            {
                double ratio = diffBeat / fadeOutDurationBeats;
                Alpha = (int)(255 * Math.Pow((1 - ratio), 2));
            }

            Bootstrap.getDisplay().addToDraw(this);
        }

        private void Fire()
        {
            if (fired) return;
            fired = true;
            Debug.Log("Hit!");
        }

        public void handleInput(InputEvent ie)
        {
            switch (ie.Type)
            {
                case InputEventType.MouseDown:
                    if (!Visible) break;
                    if (Math.Abs(ie.X - Transform.Centre.X) < Transform.Wid / 2 && Math.Abs(ie.Y - Transform.Centre.Y) < Transform.Ht / 2) {
                        Fire();
                    }
                    break;
            }
        }
    }
}
