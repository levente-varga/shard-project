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
