using Application.Scripts.Common.Interfaces;
using LabyrinthGame.Common.Interfaces;
using LabyrinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabyrinthGame.CameraNS
{
    public interface ICameraController : IMovable, ITracker, IPlayerLoop, IHaveTransform
    {
        void LookAt(Vector3 point);
        void Initialize(IPlayerLoopProcessor loopProcessor, Transform gameTransform);
        void Shutdown();
    }
}