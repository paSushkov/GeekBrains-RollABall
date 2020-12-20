using LabirinthGame.Tech.PlayerLoop;

namespace LabirinthGame.Common.Interfaces
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