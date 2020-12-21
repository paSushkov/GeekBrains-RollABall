using UnityEngine;

namespace LabyrinthGame.Common.Interfaces
{
    public interface IRotatable
    {

        Transform RotationalTransform { get; }
        Vector3 RotationSpeed { get; }
        IRotator RotationOperator { get;}

        void StartRotating(IRotator rotator);
        void StopRotating();
    }
}