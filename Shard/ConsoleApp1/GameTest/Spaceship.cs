using SDL2;
using Shard;
using System.Drawing;

namespace GameTest
{
    class Spaceship : GameObject, InputListener, CollisionHandler
    {
        bool up, down, turnLeft, turnRight;


        public override void initialize()
        {

            this.Transform.X = 500.0f;
            this.Transform.Y = 500.0f;
            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("spaceship.png");
            Layer = 1;


            Bootstrap.getInput().addListener(this);

            up = false;
            down = false;

            setPhysicsEnabled();

            MyBody.Mass = 1;
            MyBody.MaxForce = 10;
            MyBody.AngularDrag = 0.01f;
            MyBody.Drag = 0f;
            MyBody.StopOnCollision = false;
            MyBody.ReflectOnCollision = false;
            MyBody.ImpartForce = false;
            MyBody.Kinematic = false;


            //           MyBody.PassThrough = true;
            //            MyBody.addCircleCollider(0, 0, 5);
            //            MyBody.addCircleCollider(0, 34, 5);
            //            MyBody.addCircleCollider(60, 18, 5);
            //     MyBody.addCircleCollider();

            MyBody.addRectCollider();

            addTag("Spaceship");


        }

        public void fireBullet()
        {
            Bullet b = new Bullet();

            b.setupBullet(this, this.Transform.Centre.X, this.Transform.Centre.Y);

            b.Transform.rotate(this.Transform.Rotz);

            Bootstrap.getSound().PlaySound("fire.wav");
        }

        public void handleInput(InputEvent ie)
        {
            switch (ie.Type)
            {
                case InputEventType.KeyDown:
                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W)
                    {
                        up = true;
                    }

                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                    {
                        down = true;
                    }

                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                    {
                        turnRight = true;
                    }

                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                    {
                        turnLeft = true;
                    }
                    break;

                case InputEventType.KeyUp:
                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_W)
                    {
                        up = false;
                    }

                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                    {
                        down = false;
                    }

                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                    {
                        turnRight = false;
                    }

                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_A)
                    {
                        turnLeft = false;
                    }
                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                    {
                        fireBullet();
                    }
                    break;
            }
        }

        public override void physicsUpdate()
        {

            if (turnLeft)
            {
                MyBody.addTorque(-0.3f);
            }

            if (turnRight)
            {
                MyBody.addTorque(0.3f);
            }

            if (up)
            {

                MyBody.addForce(this.Transform.Forward, 0.5f);

            }

            if (down)
            {
                MyBody.addForce(this.Transform.Forward, -0.2f);
            }


        }

        public override void update()
        {
            Bootstrap.getDisplay().addToDraw(this);
        }

        public void onCollisionEnter(PhysicsBody x)
        {
            if (x.Parent.checkTag("Bullet") == false)
            {
                MyBody.DebugColor = Color.Red;
            }
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
