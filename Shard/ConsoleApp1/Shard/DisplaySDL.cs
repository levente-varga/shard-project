/*
*
*   This is the implementation of the Simple Directmedia Layer through C#.   This isn't a course on 
*       graphics, so we're not going to roll our own implementation.   If you wanted to replace it with 
*       something using OpenGL, that'd be a pretty good extension to the base Shard engine.
*       
*   Note that it extends from DisplayText, which also uses SDL.  
*   
*   @author Michael Heron
*   @version 1.0
*   
*/

using SDL2;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;

namespace Shard
{
    class Line : Renderable
    {
        private int sx, sy;
        private int ex, ey;
        private int r, g, b, a;

        public int Sx { get => sx; set => sx = value; }
        public int Sy { get => sy; set => sy = value; }
        public int Ex { get => ex; set => ex = value; }
        public int Ey { get => ey; set => ey = value; }
        public int R { get => r; set => r = value; }
        public int G { get => g; set => g = value; }
        public int B { get => b; set => b = value; }
        public int A { get => a; set => a = value; }

        public override void Render(IntPtr renderer)
        {
            SDL.SDL_SetRenderDrawColor(renderer, (byte)R, (byte)G, (byte)B, (byte)A);
            SDL.SDL_RenderDrawLine(renderer, Sx, Sy, Ex, Ey);
        }
    }

    class Circle : Renderable
    {
        int x, y, rad;
        private int r, g, b, a;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int Radius { get => rad; set => rad = value; }
        public int R { get => r; set => r = value; }
        public int G { get => g; set => g = value; }
        public int B { get => b; set => b = value; }
        public int A { get => a; set => a = value; }

        public override void Render(IntPtr renderer)
        {
            int dia = (rad * 2);
            int x = (rad - 1);
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

    class Rectangle : Renderable
    {
        int x, y, w, h;
        private int r, g, b, a;

        public int X { get => x; set => x = value; }
        public int Y { get => y; set => y = value; }
        public int W { get => w; set => w = value; }
        public int H { get => h; set => h = value; }
        public int R { get => r; set => r = value; }
        public int G { get => g; set => g = value; }
        public int B { get => b; set => b = value; }
        public int A { get => a; set => a = value; }

        public override void Render(IntPtr renderer)
        {
            SDL.SDL_SetRenderDrawColor(renderer, (byte)R, (byte)G, (byte)B, (byte)A);
            SDL.SDL_Rect rect = new SDL.SDL_Rect() { x = X, y = Y, w = W, h = H };
            SDL.SDL_RenderDrawRect(renderer, ref rect);
        }
    }


    class DisplaySDL : DisplayText
    {
        private Dictionary<string, IntPtr> spriteBuffer;
        private List<Renderable> toDraw;

        public override void initialize()
        {
            spriteBuffer = new Dictionary<string, IntPtr>();

            base.initialize();
            Debug.getInstance().log($"Initialized window size: {_width} x {_height}");
            toDraw = new List<Renderable>();
        }

        public override IntPtr loadTexture(Transform trans)
        {
            IntPtr ret;
            uint format;
            int access;
            int w;
            int h;

            ret = loadTexture(trans.SpritePath);

            SDL.SDL_QueryTexture(ret, out format, out access, out w, out h);
            trans.SetSize(w, h);

            return ret;

        }

        public override Vector2 GetTextureSize(IntPtr texture)
        {
            int width, height, access;
            uint format;
            SDL.SDL_QueryTexture(texture, out format, out access, out width, out height);
            return new Vector2(width, height);
        }

        public override IntPtr loadTexture(string path)
        {
            IntPtr img;

            if (spriteBuffer.ContainsKey(path))
            {
                return spriteBuffer[path];
            }

            img = SDL_image.IMG_Load(path);

            Debug.getInstance().log("IMG_Load: " + SDL_image.IMG_GetError());

            spriteBuffer[path] = SDL.SDL_CreateTextureFromSurface(renderer, img);

            SDL.SDL_SetTextureBlendMode(spriteBuffer[path], SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            return img;

        }


        public override void addToDraw(Renderable renderable)
        {
            toDraw.Add(renderable);
        }

        public override void removeToDraw(Renderable renderable)
        {
            toDraw.Remove(renderable);
        }

        public override void drawCircle(int x, int y, int rad, int r, int g, int b, int a, int layer = Renderable.MaxLayer)
        {
            Circle circle = new Circle();

            circle.Layer = layer;
            circle.X = x;
            circle.Y = y;
            circle.Radius = rad;

            circle.R = r;
            circle.G = g;
            circle.B = b;
            circle.A = a;

            toDraw.Add(circle);
        }
        public override void drawLine(int x, int y, int x2, int y2, int r, int g, int b, int a, int layer = Renderable.MaxLayer)
        {
            Line line = new Line();

            line.Layer = layer;
            line.Sx = x;
            line.Sy = y;
            line.Ex = x2;
            line.Ey = y2;

            line.R = r;
            line.G = g;
            line.B = b;
            line.A = a;

            toDraw.Add(line);
        }

        public override void display()
        {
            toDraw.Sort((a, b) => a.Layer - b.Layer);

            foreach (Renderable renderable in toDraw)
            {
                renderable.Render(renderer);
            }

            // Show it off.
            base.display();
        }

        public override void clearDisplay()
        {
            toDraw.Clear();

            base.clearDisplay();
        }
    }
}
