/*
*
*   The baseline functionality for getting text to work via SDL.   You could write your own text 
*       implementation (and we did that earlier in the course), but bear in mind DisplaySDL is built
*       upon this class.
*   @author Michael Heron
*   @version 1.0
*   
*/

using SDL2;
using System;
using System.Collections.Generic;

namespace Shard
{
    enum TextAlignment
    {
        Start, Center, End
    }

    // We'll be using SDL2 here to provide our underlying graphics system.
    class TextDetails
    {
        string text;
        double x, y;
        SDL.SDL_Color col;
        int size;
        IntPtr font;
        IntPtr lblText;
        TextAlignment alignmentHorizontal;
        TextAlignment alignmentVertical;

        public TextDetails(string text, double x, double y, SDL.SDL_Color col, int spacing, TextAlignment alignmentHorizontal, TextAlignment alignmentVertical)
        {
            this.text = text;
            this.x = x;
            this.y = y;
            this.col = col;
            this.size = spacing;
            this.alignmentHorizontal = alignmentHorizontal;
            this.alignmentVertical = alignmentVertical;
        }

        public TextAlignment AlignmentHorizontal
        {
            get => alignmentHorizontal;
            set => alignmentHorizontal = value;
        }
        public TextAlignment AlignmentVertical
        {
            get => alignmentVertical;
            set => alignmentVertical = value;
        }
        public string Text
        {
            get => text;
            set => text = value;
        }
        public double X
        {
            get => x;
            set => x = value;
        }
        public double Y
        {
            get => y;
            set => y = value;
        }
        public SDL.SDL_Color Col
        {
            get => col;
            set => col = value;
        }
        public int Size
        {
            get => size;
            set => size = value;
        }
        public IntPtr Font { get => font; set => font = value; }
        public IntPtr LblText { get => lblText; set => lblText = value; }
    }

    class DisplayText : Display
    {
        protected IntPtr _window, renderer;
        uint _format;
        int _access;
        private List<TextDetails> myTexts;
        private Dictionary<string, IntPtr> fontLibrary;
        public override void Clear()
        {
            foreach (TextDetails td in myTexts)
            {
                SDL.SDL_DestroyTexture(td.LblText);
            }

            myTexts.Clear();
            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
            SDL.SDL_RenderClear(renderer);

        }

        public IntPtr LoadFont(string path, int size)
        {
            string key = path + "," + size;

            if (fontLibrary.ContainsKey(key))
            {
                return fontLibrary[key];
            }

            fontLibrary[key] = SDL_ttf.TTF_OpenFont(path, size);
            return fontLibrary[key];
        }

        private void Update()
        {


        }

        private void Draw()
        {

            foreach (TextDetails td in myTexts)
            {

                SDL.SDL_Rect sRect;

                sRect.x = (int)td.X;
                sRect.y = (int)td.Y;
                sRect.w = 0;
                sRect.h = 0;
                
                SDL_ttf.TTF_SizeText(td.Font, td.Text, out sRect.w, out sRect.h);
                
                switch (td.AlignmentHorizontal)
                {
                    case TextAlignment.Start:
                        sRect.x = (int)td.X;
                        break;
                    case TextAlignment.Center:
                        sRect.x = (int)(td.X - sRect.w / 2.0);
                        break;
                    case TextAlignment.End:
                        sRect.x = (int)td.X - sRect.w;
                        break;
                }

                switch (td.AlignmentVertical)
                {
                    case TextAlignment.Start:
                        sRect.y = (int)td.Y;
                        break;
                    case TextAlignment.Center:
                        sRect.y = (int)(td.Y - sRect.h / 2.0);
                        break;
                    case TextAlignment.End:
                        sRect.y = (int)td.Y - sRect.h;
                        break;
                }


                SDL.SDL_RenderCopy(renderer, td.LblText, IntPtr.Zero, ref sRect);

            }

            SDL.SDL_RenderPresent(renderer);

        }

        public override void Present()
        {

            Update();
            Draw();
        }

        public override void SetFullscreen()
        {
            SDL.SDL_SetWindowFullscreen(_window,
                 (uint)SDL.SDL_WindowFlags.SDL_WINDOW_FULLSCREEN_DESKTOP);
        }

        public override void Initialize()
        {
            fontLibrary = new Dictionary<string, IntPtr>();

            SetSize(1280, 864);

            SDL.SDL_Init(SDL.SDL_INIT_EVERYTHING);
            SDL_ttf.TTF_Init();
            _window = SDL.SDL_CreateWindow("Shard Game Engine",
                SDL.SDL_WINDOWPOS_CENTERED,
                SDL.SDL_WINDOWPOS_CENTERED,
                GetWidth(),
                GetHeight(),
                0);


            renderer = SDL.SDL_CreateRenderer(_window,
                -1,
                SDL.SDL_RendererFlags.SDL_RENDERER_ACCELERATED);


            SDL.SDL_SetRenderDrawBlendMode(renderer, SDL.SDL_BlendMode.SDL_BLENDMODE_BLEND);

            SDL.SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);

            SDL.SDL_GetWindowSize(_window, out _width, out _height);

            myTexts = new List<TextDetails>();
        }



        public override void ShowText(string text, double x, double y, int size, int r, int g, int b, int a, TextAlignment alignmentHorizontal = TextAlignment.Start, TextAlignment alignmentVertical = TextAlignment.Start)
        {
            int nx, ny, w = 0, h = 0;

            IntPtr font = LoadFont("Fonts/calibri.ttf", size);
            SDL.SDL_Color col = new SDL.SDL_Color();

            col.r = (byte)r;
            col.g = (byte)g;
            col.b = (byte)b;
            col.a = (byte)a;

            if (font == IntPtr.Zero)
            {
                Debug.GetInstance().log("TTF_OpenFont: " + SDL.SDL_GetError());
            }

            TextDetails td = new TextDetails(text, x, y, col, 12, alignmentHorizontal, alignmentVertical);

            td.Font = font;

            IntPtr surf = SDL_ttf.TTF_RenderText_Blended(td.Font, td.Text, td.Col);
            IntPtr lblText = SDL.SDL_CreateTextureFromSurface(renderer, surf);
            SDL.SDL_FreeSurface(surf);

            SDL.SDL_Rect sRect;

            sRect.x = (int)x;
            sRect.y = (int)y;
            sRect.w = w;
            sRect.h = h;

            SDL.SDL_QueryTexture(lblText, out _format, out _access, out sRect.w, out sRect.h);

            td.LblText = lblText;

            myTexts.Add(td);


        }
        public override void ShowText(char[,] text, double x, double y, int size, int r, int g, int b, int a, TextAlignment alignmentHorizontal = TextAlignment.Start, TextAlignment alignmentVertical = TextAlignment.Start)
        {
            string str = "";
            int row = 0;

            for (int i = 0; i < text.GetLength(0); i++)
            {
                str = "";
                for (int j = 0; j < text.GetLength(1); j++)
                {
                    str += text[j, i];
                }


                ShowText(str, x, y + (row * size), size, r, g, b, a, alignmentHorizontal, alignmentVertical);
                row += 1;

            }

        }
    }
}
