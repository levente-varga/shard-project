using SDL2;
using Shard;
using System.Drawing;

namespace NewGame
{
    class Background : GameObject, InputListener, CollisionHandler
    {

        public override void initialize()
        {

            this.Transform.X = 0;
            this.Transform.Y = 0;
            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("background.jpg");


            Bootstrap.getInput().addListener(this);

            up = false;
            down = false;

            setPhysicsEnabled();

            MyBody.Mass = 0;
            MyBody.MaxForce = 0;
            MyBody.AngularDrag = 0f;
            MyBody.Drag = 0f;
            MyBody.StopOnCollision = false;
            MyBody.ReflectOnCollision = false;
            MyBody.ImpartForce = false;
            MyBody.Kinematic = false;

            MyBody.addRectCollider();

            addTag("Background");


        }


        public void handleInput(InputEvent inp, string eventType)
        {


            if (eventType == "KeyUp")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                {

                }
            }
        }

        public override void physicsUpdate()
        {

            if (turnLeft)
            {
            }

            if (turnRight)
            {
            }

            if (up)
            {
            }

            if (down)
            {
            }


        }

        public override void update()
        {
        }

        public void onCollisionEnter(PhysicsBody x)
        {

        }

        public void onCollisionExit(PhysicsBody x)
        {

        }

        public void onCollisionStay(PhysicsBody x)
        {
        }



    }
}