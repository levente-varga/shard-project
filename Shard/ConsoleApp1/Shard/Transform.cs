/*
*
*   The transform class handles position, independent of physics and forces (although the physics
*       system will make use of the rotation and translation functions here).  Essentially this class
*       is a game object's location (X, Y), rotation and scale.  Usefully it also calculates the 
*       centre of an object as well as relative directions such as forwards and right.  If you want 
*       backwards and left, multiply forward or right by -1.
*       
*   @author Michael Heron
*   @version 1.0
*   
*/


using System;
using System.Numerics;

namespace Shard
{

    class Transform
    {
        private GameObject owner;
        private float x, y;
        private float lx, ly;
        private float rotationZ;
        private int width, height;
        private float scaleX, scaleY;
        private string spritePath;
        private Vector2 forward;
        private Vector2 right, centre;

        public Vector2 GetLastDirection()
        {
            float dx, dy;
            dx = (X - Lx);
            dy = (Y - Ly);

            return new Vector2(-dx, -dy);
        }

        public Transform(GameObject ow)
        {
            Owner = ow;
            forward = new Vector2();
            right = new Vector2();
            centre = new Vector2();

            scaleX = 1.0f;
            scaleY = 1.0f;

            x = 0;
            y = 0;

            lx = 0;
            ly = 0;

            Rotate(0);
        }

        public void SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;
            RecalculateCentre();
        }

        public void RecalculateCentre()
        {

            centre.X = (float)(x + ((this.Width * scaleX) / 2));
            centre.Y = (float)(y + ((this.Height * scaleY) / 2));

        }

        public void Translate(double nx, double ny)
        {
            Translate ((float)nx, (float)ny);
        }



        public void Translate(float nx, float ny)
        {
            Lx = X;
            Ly = Y;

            x += (float)nx;
            y += (float)ny;


            RecalculateCentre();
        }

        public void Translate(Vector2 vect)
        {
            Translate(vect.X, vect.Y);
        }



        public void Rotate(float dir)
        {
            rotationZ += (float)dir;

            rotationZ %= 360;

            float angle = (float)(Math.PI * rotationZ / 180.0f);
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            forward.X = cos;
            forward.Y = sin;


            right.X = -1 * forward.Y;
            right.Y = forward.X;
        }


        public float X
        {
            get => x;
            set => x = value;
        }
        public float Y
        {
            get => y;
            set => y = value;
        }

        public float RotationZ
        {
            get => rotationZ;
            set => rotationZ = value;
        }


        public string SpritePath
        {
            get => spritePath;
            set => spritePath = value;
        }
        public ref Vector2 Forward { get => ref forward; }
        public int Width 
        { 
            get => width; 
            set
            {
                width = value;
                RecalculateCentre();
            }
        }
        public int Height 
        { 
            get => height;
            set 
            { 
                height = value;
                RecalculateCentre();
            }
        }
        public ref Vector2 Right { get => ref right; }
        internal GameObject Owner { get => owner; set => owner = value; }
        public Vector2 Centre 
        { 
            get => centre;
            set
            {
                centre = value;
                x = centre.X - width * scaleX / 2;
                y = centre.Y - height  * scaleY / 2;
            }
        }
        public float Scalex { get => scaleX; set => scaleX = value; }
        public float Scaley { get => scaleY; set => scaleY = value; }
        public float Lx { get => lx; set => lx = value; }
        public float Ly { get => ly; set => ly = value; }
    }
}
