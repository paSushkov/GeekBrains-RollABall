using LabirinthGame.Common.Handlers;

namespace LabirinthGame.Tech.Input
{
    public class AxisInput
    {
        private event AxisInputHandler _broadcaster;

        public void AddListener(AxisInputHandler handler)
        {
            _broadcaster += handler;
        }
        public void RemoveListener(AxisInputHandler handler)
        {
            _broadcaster -= handler;
        }

        public void Broadcast(float value)
        {
            _broadcaster?.Invoke(value);
        }
    }
}