using SDL2;
using Shard;
using System.Drawing;

namespace NewGame
{
    class Item : GameObject, InputListener, CollisionHandler
    {
        bool up, down, turnLeft, turnRight;


        public override void initialize()
        {

            this.Transform.X = 500.0f;
            this.Transform.Y = 500.0f;
            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("asteroid.png");

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

            MyBody.addRectCollider();

            addTag("Item");


        }

        public void fireBullet()
        {
            Bootstrap.getSound().playSound ("fire.wav");
        }

        public void handleInput(InputEvent inp, string eventType)
        {


            if (eventType == "KeyUp")
            {
                if (inp.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                {
                    fireBullet();
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
            Bootstrap.getDisplay().addToDraw(this);
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

        public override string ToString()
        {
            return "Item: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }

    }
}
