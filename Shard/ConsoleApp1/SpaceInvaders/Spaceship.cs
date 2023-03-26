using SDL2;
using Shard;
using System.Drawing;

namespace SpaceInvaders
{
    class Spaceship : GameObject, InputListener, CollisionHandler
    {
        bool left, right;
        float fireCounter, fireDelay;


        public override void Initialize()
        {

            this.Transform.X = 100.0f;
            this.Transform.Y = 800.0f;
            this.Transform.SpritePath = Bootstrap.GetAssetManager().GetAssetPath("player.png");


            fireDelay = 2;
            fireCounter = fireDelay;

            Bootstrap.GetInput().AddListener(this);

            SetPhysicsEnabled();

            MyBody.AddRectCollider();

            AddTag("Player");


        }

        public void fireBullet()
        {
            if (fireCounter < fireDelay)
            {
                return;
            }

            Bullet b = new Bullet();

            b.setupBullet(this.Transform.Centre.X, this.Transform.Centre.Y);
            b.Dir = -1;
            b.DestroyTag = "Invader";

            fireCounter = 0;

        }

        public void HandleInput(InputEvent ie)
        {

            if (Bootstrap.GetRunningGame().IsRunning() == false)
            {
                return;
            }
            
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

                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                    {
                        fireBullet();
                    }
                    break;
            }
        }

        public override void Update()
        {
            float amount = (float)(100 * Bootstrap.GetDeltaTime());

            fireCounter += (float)Bootstrap.GetDeltaTime();

            if (left)
            {
                this.Transform.Translate(-1 * amount, 0);
            }

            if (right)
            {
                this.Transform.Translate(1 * amount, 0);
            }

            Bootstrap.GetDisplay().AddToDraw(this);
        }

        public void OnCollisionEnter(PhysicsBody x)
        {

        }

        public void OnCollisionExit(PhysicsBody x)
        {

            MyBody.DebugColor = Color.Green;
        }

        public void OnCollisionStay(PhysicsBody x)
        {
            MyBody.DebugColor = Color.Blue;
        }

        public override string ToString()
        {
            return "Spaceship: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Width + ", " + Transform.Height + "]";
        }

    }
}
