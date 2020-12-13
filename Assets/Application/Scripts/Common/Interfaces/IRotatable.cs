using LabirinthGame.Managers;
using UnityEngine;

namespace LabirinthGame.Common.Interfaces
{
    public interface IRotatable
    {
        Transform RotationalTransform { get; }
        Vector3 RotationSpeed { get; }

        void Register(MasterRotator master);
        void UnRegister(MasterRotator master);
    }
}