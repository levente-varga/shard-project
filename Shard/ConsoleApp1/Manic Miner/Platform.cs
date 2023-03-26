using Shard;
using System;

namespace ManicMiner
{
    class Platform : GameObject, CollisionHandler
    {
        private int moveDirX, moveDirY;
        private int maxY, minY;
        private int maxX, minX;
        private int moveDist;
        private int moveSpeed;

        private int origX, origY;
        public int MoveDist { get => moveDist; set => moveDist = value; }
        public int MoveDirX { get => moveDirX; set => moveDirX = value; }
        public int MoveDirY { get => moveDirY; set => moveDirY = value; }
        public int MoveSpeed { get => moveSpeed; set => moveSpeed = value; }

        public override void Initialize()
        {
            SetPhysicsEnabled();
            MyBody.AddRectCollider();
            MyBody.Mass = 10;
            MyBody.Kinematic = true;
            

            this.Transform.SpritePath = Bootstrap.GetAssetManager().GetAssetPath("platform.png");



        }

        public void setPosition(int x, int y, int dist, int speed) {
            origX = x;
            origY = y;

            MoveDist = dist;

            minY = origY - MoveDist;
            maxY = origY;

            maxX = origX + MoveDist;
            minX = origX;

            MoveSpeed = speed;

            Transform.Translate (x, y);
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

        public override void Update()
        {

            if (moveDirY != 0)
            {
                Transform.Translate(0, moveSpeed * moveDirY * Bootstrap.GetDeltaTime());

                if (Transform.Y > maxY) {
                    MoveDirY = -1;
                }
            
                if (Transform.Y < minY) {
                    MoveDirY = 1;

                }
            }


            if (moveDirX != 0)
            {
                Transform.Translate(moveSpeed * moveDirX * Bootstrap.GetDeltaTime(), 0);

                if (Transform.X > maxX)
                {
                    MoveDirX = -1;
                }

                if (Transform.X < minX)
                {
                    MoveDirX = 1;

                }
            }



            Bootstrap.GetDisplay().AddToDraw(this);
        }

    }
}
