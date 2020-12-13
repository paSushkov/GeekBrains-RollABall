using UnityEngine;

namespace LabirinthGame.Common.Interfaces
{
    public interface IMovable
    {
        void MoveTo(Vector3 position);

        void MoveTowards(Vector3 position, float speed);
        
        void Move(Vector3 direction, float speed);
    }
}