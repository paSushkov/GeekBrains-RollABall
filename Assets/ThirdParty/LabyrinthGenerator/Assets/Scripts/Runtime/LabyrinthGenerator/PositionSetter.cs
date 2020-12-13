using UnityEngine;


namespace LabyrinthGenerator
{
    public static class PositionSetter
    {
        public static Vector3 Set(Vector3 currentPosition, float stepX, float stepZ, Direction directionX, Direction directionZ = Direction.None)
        {
            var newPosition = currentPosition;
            switch (directionX)
            {
                case Direction.Up:
                    newPosition.z += stepZ;
                    break;
                case Direction.Down:
                    newPosition.z -= stepZ;
                    break;
                case Direction.Left:
                    newPosition.x -= stepX;
                    break;
                case Direction.Right:
                    newPosition.x += stepX;
                    break;
                case Direction.None:
                    break;
            }

            if (directionZ != Direction.None)
                newPosition = Set(newPosition, stepX, stepZ, directionZ);

            return newPosition;
        }
    }
}
