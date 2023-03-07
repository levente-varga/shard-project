using SDL2;
using Shard;
using System.Drawing;

namespace SpaceInvaders
{
    class Spaceship : GameObject, InputListener, CollisionHandler
    {
        bool left, right;
        float fireCounter, fireDelay;


        public override void initialize()
        {

            this.Transform.X = 100.0f;
            this.Transform.Y = 800.0f;
            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("player.png");


            fireDelay = 2;
            fireCounter = fireDelay;

            Bootstrap.getInput().addListener(this);

            setPhysicsEnabled();

            MyBody.addRectCollider();

            addTag("Player");


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

        public void handleInput(InputEvent ie)
        {

            if (Bootstrap.getRunningGame().isRunning() == false)
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

        public override void update()
        {
            float amount = (float)(100 * Bootstrap.getDeltaTime());

            fireCounter += (float)Bootstrap.getDeltaTime();

            if (left)
            {
                this.Transform.translate(-1 * amount, 0);
            }

            if (right)
            {
                this.Transform.translate(1 * amount, 0);
            }

            Bootstrap.getDisplay().addToDraw(this);
        }

        public void onCollisionEnter(PhysicsBody x)
        {

        }

        public void onCollisionExit(PhysicsBody x)
        {

            MyBody.DebugColor = Color.Green;
        }

        public void onCollisionStay(PhysicsBody x)
        {
            MyBody.DebugColor = Color.Blue;
        }

        public override string ToString()
        {
            return "Spaceship: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }

    }
}
