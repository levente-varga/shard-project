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
            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("ball.png");
            this.Layer = 1;

            Bootstrap.getInput().addListener(this);

            up = false;
            down = false;

            setPhysicsEnabled();

            MyBody.addRectCollider();

            addTag("Item");


        }

        public void fireBullet()
        {
        }

        public void handleInput(InputEvent ie)
        {


            if (ie.Type == InputEventType.KeyUp)
            {
                if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
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
