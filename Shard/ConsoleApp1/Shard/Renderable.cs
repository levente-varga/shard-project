using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    abstract class Renderable
    {
        int layer;

        public int Layer { 
            get => layer;
            set => layer = (value >= 0 ? value <= MaxLayer ? value : MaxLayer : 0); 
        }

        public const int MaxLayer = 255;

        public abstract void Render(IntPtr renderer);
    }
}
