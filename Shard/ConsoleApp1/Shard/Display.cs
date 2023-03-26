/*
*
*   The abstract display class setting out the consistent interface all display implementations need.  
*   @author Michael Heron
*   @version 1.0
*   
*/

using SDL2;
using System;
using System.Drawing;
using System.Numerics;

namespace Shard
{
    abstract class Display
    {
        protected int _height, _width;

        public virtual void DrawLine(int x, int y, int x2, int y2, int r, int g, int b, int a, int layer = Renderable.MaxLayer)
        {
        }

        public virtual void DrawLine(int x, int y, int x2, int y2, Color col, int layer = Renderable.MaxLayer)
        {
            DrawLine(x, y, x2, y2, col.R, col.G, col.B, col.A, layer);
        }

        public virtual void DrawCircle(int x, int y, int rad, int r, int g, int b, int a, int layer = Renderable.MaxLayer)
        {
        }

        public virtual void DrawCircle(int x, int y, int rad, Color col, int layer = Renderable.MaxLayer)
        {
            DrawCircle(x, y, rad, col.R, col.G, col.B, col.A, layer);
        }

        public virtual void DrawFilledCircle(int x, int y, int rad, Color col, int layer = Renderable.MaxLayer)
        {
            DrawFilledCircle(x, y, rad, col.R, col.G, col.B, col.A, layer);
        }

        public virtual void DrawFilledCircle(int x, int y, int rad, int r, int g, int b, int a, int layer = Renderable.MaxLayer)
        {
            while (rad > 0)
            {
                DrawCircle(x, y, rad, r, g, b, a, layer);
                rad -= 1;
            }
        }

        public virtual void DrawRectangle(int x, int y, int x2, int y2, int r, int g, int b, int a, int layer = Renderable.MaxLayer)
        {
        }

        public virtual void DrawRectangle(int x, int y, int x2, int y2, Color col, int layer = Renderable.MaxLayer)
        {
            DrawRectangle(x, y, x2, y2, col.R, col.G, col.B, col.A, layer);
        }

        public void ShowText(string text, double x, double y, int size, Color col, TextAlignment alignmentHorizontal = TextAlignment.Start, TextAlignment alignmentVertical = TextAlignment.Start)
        {
            ShowText(text, x, y, size, col.R, col.G, col.B, col.A, alignmentHorizontal, alignmentVertical);
        }



        public virtual void SetFullscreen()
        {
        }

        public virtual IntPtr LoadTexture(string path)
        {
            return IntPtr.Zero;
        }

        public virtual IntPtr LoadTexture(Transform transform)
        {
            return IntPtr.Zero;
        }

        public virtual Vector2 GetTextureSize(IntPtr texture)
        {
            return new Vector2();
        }

        public virtual void AddToDraw(Renderable renderable)
        {
        }

        public virtual void RemoveToDraw(Renderable renderable)
        {
        }
        public int GetHeight()
        {
            return _height;
        }

        public int GetWidth()
        {
            return _width;
        }

        public virtual void SetSize(int w, int h)
        {
            _height = h;
            _width = w;
        }

        public abstract void Initialize();
        public abstract void Clear();
        public abstract void Present();

        public abstract void ShowText(string text, double x, double y, int size, int r, int g, int b, int a, TextAlignment alignmentHorizontal = TextAlignment.Start, TextAlignment alignmentVertical = TextAlignment.Start);
        public abstract void ShowText(char[,] text, double x, double y, int size, int r, int g, int b, int a, TextAlignment alignmentHorizontal = TextAlignment.Start, TextAlignment alignmentVertical = TextAlignment.Start);
    }
}
