using SDL2;
using Shard;
using System;
using System.Drawing;

namespace GameTest
{
    class Button : GameObject
    {
        public delegate void OnPressEventHandler();
        public event OnPressEventHandler OnClick;

        Color backgroundColor;
        Color backgroundHoverColor;
        Color textColor;
        Color textHoverColor;
        string text;
        int fontSize;
        bool hovered = false;

        public Button(int x, int y, int width, int height, string text, int fontSize, Color backgroundColor, Color backgroundHoverColor, Color textColor, Color textHoverColor)
        {
            Transform.X = x;
            Transform.Y = y;
            Transform.Width = width;
            Transform.Height = height;
            this.text = text;
            this.fontSize = fontSize;
            this.backgroundColor = backgroundColor;
            this.backgroundHoverColor = backgroundHoverColor;
            this.textColor = textColor;
            this.textHoverColor = textHoverColor;
        }

        public override void Initialize()
        {
            //this.Transform.SpritePath = Bootstrap.GetAssetManager().GetAssetPath("");

            Bootstrap.GetInput().AddListener(this);
        }

        public override void HandleInput(InputEvent ie)
        {
            //Debug.Log($"Button HandleInput");
            switch (ie.Type)
            {
                case InputEventType.MouseDown:
                    if (IsHovered(ie.X, ie.Y))
                    {
                        Debug.Log($"Button pressed!");
                        OnClick();
                    }
                    break;
                case InputEventType.MouseMotion:
                    hovered = IsHovered(ie.X, ie.Y);
                    break;
            }
        }

        public override void Update()
        {
            //Debug.Log($"Button update");

            //if (Transform == null) return;

            //Bootstrap.GetDisplay().AddToDraw(this);
            Bootstrap.GetDisplay().AddToDraw(new Shard.Rectangle((int)Transform.X, (int)Transform.Y, Transform.Width, Transform.Height, true, hovered ? backgroundHoverColor : backgroundColor));

            Bootstrap.GetDisplay().ShowText(text, Transform.X + Transform.Width / 2, Transform.Y + Transform.Height / 2, fontSize, hovered ? textHoverColor : textColor, TextAlignment.Center, TextAlignment.Center);
        }

        public override void Render(IntPtr renderer)
        {
            //Debug.Log($"Button render");
        }

        private bool IsHovered(Point mousePosition) => IsHovered(mousePosition.X, mousePosition.Y);
        private bool IsHovered(int x, int y)
        {
            //if (Transform == null) return false;

            return (Transform.X <= x && x < Transform.X + Transform.Width
                 && Transform.Y <= y && y < Transform.Y + Transform.Height);
        }
    }
}
