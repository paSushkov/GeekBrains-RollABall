using LabyrinthGame.Common.Handlers;
using LabyrinthGame.Tech.PlayerLoop;

namespace LabyrinthGame.Tech.Input
{
    public interface IInputTranslator
    {
        void SubscribeToAxisInput(string axisName, AxisInputHandler handler);
        void UnsubscribeFromAxisInput(string axisName, AxisInputHandler handler);
        void Initialize(IPlayerLoopProcessor loopProcessor);
        void Shutdown();
    }
}