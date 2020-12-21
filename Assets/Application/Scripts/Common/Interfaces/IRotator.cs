using LabyrinthGame.Tech.PlayerLoop;

namespace LabyrinthGame.Common.Interfaces
{
    public interface IRotator : IPlayerLoop
    {
        void RotateInUpdate(IRotatable rotatable);
        void StopRotateInUpdate(IRotatable rotatable);
        
        void RotateInFixedUpdate(IRotatable rotatable);
        void StopRotateInFixedUpdate(IRotatable rotatable);

        void Shutdown();
        void Initialize(IPlayerLoopProcessor playerLoopProcessor);
    }
}