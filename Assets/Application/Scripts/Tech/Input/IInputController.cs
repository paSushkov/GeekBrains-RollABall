namespace LabirinthGame.Tech.Input
{
    public interface IInputController
    {
        void Initialize();
        void Shutdown();
        void Start();
        void Stop();
        void PauseFor();
    }
}