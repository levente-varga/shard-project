using SDL2;
using Shard;
using System.Drawing;

namespace NewGame
{
    class Item : GameObject, InputListener, CollisionHandler
    {
        bool up, down, turnLeft, turnRight;


        public override void Initialize()
        {

            this.Transform.X = 500.0f;
            this.Transform.Y = 500.0f;
            this.Transform.SpritePath = Bootstrap.GetAssetManager().GetAssetPath("ball.png");
            this.Layer = 1;

            Bootstrap.GetInput().AddListener(this);

            up = false;
            down = false;

            SetPhysicsEnabled();

            MyBody.AddRectCollider();

            AddTag("Item");


        }

        public void fireBullet()
        {
        }

        public void HandleInput(InputEvent ie)
        {


            if (ie.Type == InputEventType.KeyUp)
            {
                if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_SPACE)
                {
                    fireBullet();
                }
            }
        }

        public override void PhysicsUpdate()
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

        public override void Update()
        {
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
            return "Item: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Width + ", " + Transform.Height + "]";
        }

    }
}
