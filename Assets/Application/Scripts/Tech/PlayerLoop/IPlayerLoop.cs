using Application.Scripts.Common.Interfaces;

namespace LabyrinthGame.Tech.PlayerLoop
{
    public interface IPlayerLoop
    {
        IPlayerLoopSubscriptionController PlayerLoopSubscriptionController { get; }
        void ProcessUpdate(float deltaTime);
        void ProcessFixedUpdate(float fixedDeltaTime);
        void ProcessLateUpdate(float fixedDeltaTime);
    }
}