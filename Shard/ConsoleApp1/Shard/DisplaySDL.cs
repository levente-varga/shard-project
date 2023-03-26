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
    class DisplaySDL : DisplayText
    {
        private Dictionary<string, IntPtr> spriteBuffer;
        private List<Renderable> toDraw;

        public override void Initialize()
        {
            spriteBuffer = new Dictionary<string, IntPtr>();

            base.Initialize();
            Debug.GetInstance().log($"Initialized window size: {_width} x {_height}");
            toDraw = new List<Renderable>();
        }

        public override IntPtr LoadTexture(Transform trans)
        {
            IntPtr ret;
            uint format;
            int access;
            int w;
            int h;

            ret = LoadTexture(trans.SpritePath);

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

        public override IntPtr LoadTexture(string path)
        {
            IntPtr img;

            if (spriteBuffer.ContainsKey(path))
            {
                return spriteBuffer[path];
            }

            img = SDL_image.IMG_Load(path);

            Debug.GetInstance().log("IMG_Load: " + SDL_image.IMG_GetError());

            spriteBuffer[path] = SDL.SDL_CreateTextureFromSurface(renderer, img);

            SDL.SDL_SetTextureBlendMode(spriteBuffer[path], SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            return img;

        }


        public override void AddToDraw(Renderable renderable)
        {
            toDraw.Add(renderable);
        }

        public override void RemoveToDraw(Renderable renderable)
        {
            toDraw.Remove(renderable);
        }

        public override void DrawCircle(int x, int y, int radius, int r, int g, int b, int a, int layer = Renderable.MaxLayer)
        {
            toDraw.Add(new Circle(x, y, radius, r, g, b, a, layer));
        }

        public override void DrawLine(int x, int y, int x2, int y2, int r, int g, int b, int a, int layer = Renderable.MaxLayer)
        {
            toDraw.Add(new Line(x, y, x2, y2, r, g, b, a, layer));
        }

        public override void DrawRectangle(int x, int y, int w, int h, int r, int g, int b, int a, int layer = Renderable.MaxLayer)
        {
            toDraw.Add(new Rectangle(x, y, w, h, false, r, g, b, a, layer));
        }

        public override void Present()
        {
            toDraw.Sort((a, b) => a.Layer - b.Layer);

            //Debug.Log($"Drawing {toDraw.Count} objects");

            foreach (Renderable renderable in toDraw)
            {
                renderable.Render(renderer);
            }

            // Show it off.
            base.Present();
        }

        public override void Clear()
        {
            toDraw.Clear();

            base.Clear();
        }
    }
}
