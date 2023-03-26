using SDL2;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    abstract class Shape : Renderable
    {
        protected int x, y;
        protected Color color;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int R { get => color.R; set => color = Color.FromArgb(value, color.G, color.B, color.A); }
        public int G { get => color.G; set => color = Color.FromArgb(color.R, value, color.B, color.A); }
        public int B { get => color.B; set => color = Color.FromArgb(color.R, color.G, value, color.A); }
        public int A { get => color.A; set => color = Color.FromArgb(color.R, color.G, color.B, value); }
        public Color Color { get => color; set => color = value; }

        public Shape(int x, int y, Color color, int layer)
        {
            this.x = x;
            this.y = y;
            this.color = color;
            this.Layer = layer;
        }

        public abstract override void Render(IntPtr renderer);
    }

    class Line : Shape
    {
        private int x2, y2;

        public int X2 { get => x2; set => x2 = value; }
        public int Y2 { get => y2; set => y2 = value; }

        public Line(int x, int y, int x2, int y2, int r = 255, int g = 255, int b = 255, int a = 255, int layer = MaxLayer) 
            : this(x, y, x2, y2, Color.FromArgb(a, r, g, b), layer) { }
        public Line(int x, int y, int x2, int y2, Color color, int layer = MaxLayer) : base(x, y, color, layer)
        {
            X2 = x2;
            Y2 = y2;
        }

        public override void Render(IntPtr renderer)
        {
            SDL.SDL_SetRenderDrawColor(renderer, (byte)R, (byte)G, (byte)B, (byte)A);
            SDL.SDL_RenderDrawLine(renderer, X, Y, X2, Y2);
        }
    }

    class Rectangle : Shape
    {
        int w, h;
        bool filled = false;

        public int Width { get => w; set => w = value; }
        public int Height { get => h; set => h = value; }
        public bool Filled { get => filled; set => filled = value; }

        public Rectangle(int x, int y, int w, int h, bool filled, int r = 255, int g = 255, int b = 255, int a = 255, int layer = MaxLayer) 
            : this(x, y, w, h, filled, Color.FromArgb(a, r, g, b), layer) { }
        public Rectangle(int x, int y, int w, int h, bool filled, Color color, int layer = MaxLayer) : base(x, y, color, layer)
        {
            Width = w;
            Height = h;
            Filled = filled;
        }

        public override void Render(IntPtr renderer)
        {
            SDL.SDL_SetRenderDrawColor(renderer, (byte)R, (byte)G, (byte)B, (byte)A);
            SDL.SDL_Rect rect = new SDL.SDL_Rect() { x = X, y = Y, w = Width, h = Height };
            if (filled)
            {
                SDL.SDL_RenderFillRect(renderer, ref rect);
            }
            else
            {
                SDL.SDL_RenderDrawRect(renderer, ref rect);
            }
        }
    }

    class Circle : Shape
    {
        int radius;

        public int Radius { get => radius; set => radius = value; }

        public Circle(int x, int y, int radius, int r = 255, int g = 255, int b = 255, int a = 255, int layer = MaxLayer)
            : this(x, y, radius, Color.FromArgb(a, r, g, b), layer) { }
        public Circle(int x, int y, int radius, Color color, int layer = MaxLayer) : base(x, y, color, layer)
        {
            Radius = radius;
        }

        public override void Render(IntPtr renderer)
        {
            int dia = (radius * 2);
            int x = (radius - 1);
            int y = 0;
            int tx = 1;
            int ty = 1;
            int error = (tx - dia);

            SDL.SDL_SetRenderDrawColor(renderer, (byte)R, (byte)G, (byte)B, (byte)A);

            // We draw an octagon around the point, and then turn it a bit.  Do 
            // that until we have an outline circle.  If you want a filled one, 
            // do the same thing with an ever decreasing radius.
            while (x >= y)
            {
                SDL.SDL_RenderDrawPoint(renderer, X + x, Y - y);
                SDL.SDL_RenderDrawPoint(renderer, X + x, Y + y);
                SDL.SDL_RenderDrawPoint(renderer, X - x, Y - y);
                SDL.SDL_RenderDrawPoint(renderer, X - x, Y + y);
                SDL.SDL_RenderDrawPoint(renderer, X + y, Y - x);
                SDL.SDL_RenderDrawPoint(renderer, X + y, Y + x);
                SDL.SDL_RenderDrawPoint(renderer, X - y, Y - x);
                SDL.SDL_RenderDrawPoint(renderer, X - y, Y + x);

                if (error <= 0)
                {
                    y += 1;
                    error += ty;
                    ty += 2;
                }

                if (error > 0)
                {
                    x -= 1;
                    tx += 2;
                    error += (tx - dia);
                }
            }
        }
    }
}
