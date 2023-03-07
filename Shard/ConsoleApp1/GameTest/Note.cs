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
        double positionBeat;
        int markerEndSize;
        int markerStartSize;
        double visibilityPeriodBeat;
        double currentBeat;

        List<double> updates;

        public double CurrentBeat
        {
            set => currentBeat = value;
        }

        public double VisibilityPeriodBeat
        {
            set => visibilityPeriodBeat = value > 0 ? value : 1;
        }

        public Note(double positionBeat, Point position, int markerStartSize = 50, int markerEndSize = 25, double visibilityPeriodBeat = 2)
        {
            Transform.X = position.X;
            Transform.Y = position.Y;
            this.positionBeat = positionBeat;
            this.markerStartSize = markerStartSize;
            this.markerEndSize = markerEndSize;
            this.VisibilityPeriodBeat = visibilityPeriodBeat;
            Layer = 3;
            updates = new List<double>();

            Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("note.png"); ;
        }

        public override void update()
        {
            double diffBeat = positionBeat - currentBeat;
            if (0 > diffBeat || diffBeat > visibilityPeriodBeat) return;

            double ratio = diffBeat / visibilityPeriodBeat;

            Bootstrap.getDisplay().addToDraw(this);
            Bootstrap.getDisplay().drawCircle((int)Transform.Centre.X, (int)Transform.Centre.Y, (int)(markerEndSize + ratio * (markerStartSize - markerEndSize)), 255, 255, 255, (int)((1 - ratio) * 255));

            double time = Bootstrap.TimeElapsed;
            if (updates.Count == 0 || updates[0] != time) updates.Add(time);
            while (updates[0] < time - 1) updates.RemoveAt(0);
        }

        public void handleInput(InputEvent ie)
        {
            switch (ie.Type)
            {
                case InputEventType.MouseDown:
                    if (Math.Abs(ie.X - Transform.Centre.X) < Transform.Wid && Math.Abs(ie.Y - Transform.Centre.Y) < Transform.Ht) {
                        Debug.Log("Hit!");
                    }
                    break;
            }
        }
    }
}
