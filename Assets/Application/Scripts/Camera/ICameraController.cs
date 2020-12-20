using LabirinthGame.Common.Interfaces;
using UnityEngine;

namespace LabirinthGame.Camera
{
    public interface ICameraController : IMovable, ITracker
    {
        Transform CameraTransform { get; }
        void LookAt(Vector3 point);
    }
}