namespace LabirinthGame.Tech.Input
{
    public interface IUserInputManager
    {
        void Initialize(IInputController gameController, IInputController menuController);
        void Shutdown();
        IInputController GetUIInputController();
        IInputController GetGameActiveController(InputControllerChanged inputChangeHandler = null);
    }
}