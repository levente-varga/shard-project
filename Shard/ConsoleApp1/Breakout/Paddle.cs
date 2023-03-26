using SDL2;

using Shard;

namespace GameBreakout
{
    class Paddle : GameObject, InputListener, CollisionHandler
    {
        bool left, right;
        int wid;


        public override void Initialize()
        {

            this.Transform.X = 500.0f;
            this.Transform.Y = 800.0f;
            this.Transform.SpritePath = Bootstrap.GetAssetManager().GetAssetPath("test.png");
            this.Transform.Scaley = 0.5f;
            this.Transform.Scalex = 1.5f;


            Bootstrap.GetInput().AddListener(this);

            left = false;
            right = false;

            SetPhysicsEnabled();

            MyBody.Mass = 1000;
            MyBody.MaxForce = 20;
            MyBody.Drag = 0.1f;

            MyBody.AddRectCollider();

            AddTag("Paddle");

            wid = Bootstrap.GetDisplay().GetWidth();
        }

        public void HandleInput(InputEvent ie)
        {
            switch (ie.Type)
            {
                case InputEventType.KeyDown:
                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                    {
                        right = true;
                    }

                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                    {
                        left = true;
                    }
                    break;

                case InputEventType.KeyUp:
                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                    {
                        right = false;
                    }

                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                    {
                        left = false;
                    }
                    break;
            }
        }

        public override void Update()
        {
            Bootstrap.GetDisplay().AddToDraw(this);
        }

        public override void PhysicsUpdate()
        {

            double boundsx;

            if (left)
            {
                MyBody.AddForce(this.Transform.Forward, -1 * 2000f);
            }


            if (right)
            {
                MyBody.AddForce(this.Transform.Forward, 2000f);
            }


            if (this.Transform.X < 0)
            {
                this.Transform.Translate(-1 * Transform.X, 0);
            }


            boundsx = wid - (this.Transform.X + this.Transform.Width);

            if (boundsx < 0)
            {
                this.Transform.Translate(boundsx, 0);
            }





            Bootstrap.GetDisplay().AddToDraw(this);
        }

        public void OnCollisionEnter(PhysicsBody x)
        {
        }

        public void OnCollisionExit(PhysicsBody x)
        {

        }

        public void OnCollisionStay(PhysicsBody x)
        {
        }

        public override string ToString()
        {
            return "Paddle: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Width + ", " + Transform.Height + "]";
        }

    }
}
