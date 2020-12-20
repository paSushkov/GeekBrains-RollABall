using LabirinthGame.Common.Handlers;
using LabirinthGame.Tech.PlayerLoop;

namespace LabirinthGame.Tech.Input
{
    public interface IInputTranslator
    {
        void SubscribeToAxisInput(string axisName, AxisInputHandler handler);
        void UnsubscribeFromAxisInput(string axisName, AxisInputHandler handler);
        void Initialize(IPlayerLoopProcessor loopProcessor);
        void Shutdown();
    }
}