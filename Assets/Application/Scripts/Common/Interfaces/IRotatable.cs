using LabirinthGame.Managers;
using UnityEngine;

namespace LabirinthGame.Common.Interfaces
{
    public interface IRotatable
    {
        Transform RotationalTransform { get; }
        Vector3 RotationSpeed { get; }

        void Register(IRotator rotator);
        void Unregister(IRotator rotator);
    }
}