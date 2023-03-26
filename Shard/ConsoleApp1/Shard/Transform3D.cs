/*
*
*   Our game engine functions in 2D, but all its features except for graphics can mostly be extended
*       from existing data structures.
*       
*   @author Michael Heron
*   @version 1.0
*   
*/

namespace Shard
{
    class Transform3D : Transform
    {
        private double z;
        private double rotationX, rotationY;
        private int scaleZ;

        public Transform3D(GameObject o) : base(o)
        {
        }

        public double Z
        {
            get => z;
            set => z = value;
        }

        public int ScaleZ
        {
            get => scaleZ;
            set => scaleZ = value;
        }

        public double RotationX { get => rotationX; set => rotationX = value; }
        public double RotationY { get => rotationY; set => rotationY = value; }
    }
}
