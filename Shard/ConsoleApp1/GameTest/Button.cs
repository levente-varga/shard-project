using SDL2;
using Shard;
using System.Drawing;

namespace GameTest
{
    class Button : GameObject, InputListener
    {
        public delegate void OnPressEventHandler(double value);
        public event OnPressEventHandler onPress;

        public override void initialize()
        {

            this.Transform.X = 500.0f;
            this.Transform.Y = 500.0f;
            this.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("spaceship.png");


            Bootstrap.getInput().addListener(this);

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

            addTag("Spaceship");


        }

        public void fireBullet()
        {
            
        }

        public void handleInput(InputEvent ie)
        {



            
        }

        public override void update()
        {
            Bootstrap.getDisplay().addToDraw(this);
        }

        public override string ToString()
        {
            return "Spaceship: [" + Transform.X + ", " + Transform.Y + ", " + Transform.Wid + ", " + Transform.Ht + "]";
        }

    }
}
