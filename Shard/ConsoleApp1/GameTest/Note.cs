using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    internal class Note : GameObject
    {
        Point position;
        double positionBeat;
        int size;
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

        public Note(double positionBeat, Point position, int size, double visibilityPeriodBeat = 2)
        {
            this.position = position;
            this.positionBeat = positionBeat;
            this.size = size;
            this.VisibilityPeriodBeat = visibilityPeriodBeat;
            updates = new List<double>();
        }

        public override void update()
        {
            double diffBeat = positionBeat - currentBeat;
            if (0 > diffBeat || diffBeat > visibilityPeriodBeat) return;

            double ratio = diffBeat / visibilityPeriodBeat;

            Bootstrap.getDisplay().drawCircle(position.X, position.Y, (int)(ratio * size), 255, 255, 255, (int)((1 - ratio) * 255));

            double time = Bootstrap.TimeElapsed;
            if (updates.Count == 0 || updates[0] != time) updates.Add(time);
            while (updates[0] < time - 1) updates.RemoveAt(0);
            Bootstrap.getDisplay().showText($"{updates.Count}", 10, 100, 12, 255, 255, 255);
        }
    }
}
