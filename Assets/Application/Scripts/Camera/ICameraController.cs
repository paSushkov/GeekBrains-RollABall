using Application.Scripts.Common.Interfaces;
using LabirinthGame.Common.Interfaces;
using LabirinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabirinthGame.Camera
{
    public interface ICameraController : IMovable, ITracker, IPlayerLoop, IHaveTransform
    {
        void LookAt(Vector3 point);
        void Initialize(IPlayerLoopProcessor loopProcessor, Transform gameTransform);
        void Shutdown();
    }
}