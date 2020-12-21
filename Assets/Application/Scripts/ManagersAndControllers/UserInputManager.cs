using LabyrinthGame.Tech.Input;
using UnityEngine;

namespace LabyrinthGame.Managers
{
    // TODO: ad different states to game process. like pause / menu active / in lobby and etc.

    [CreateAssetMenu(menuName = "Sushkov/Managers/UserInputManager")]
    public class UserInputManager : ScriptableObject, IUserInputManager
    {
        private event InputControllerChanged GameControllerChanged;

        #region Private data

        private IInputController _gameActiveController;
        private IInputController _menuActiveController;

        #endregion


        #region Properties

        private IInputController GameActiveController
        {
            get => _gameActiveController;
            set
            {
                _gameActiveController = value;
                GameControllerChanged?.Invoke(_gameActiveController);
            }
        }
        public IInputController MenuActiveController
        {
            get => _menuActiveController;
            set => _menuActiveController = value;
        }

        #endregion


        #region IUserInputManager imlementation

        public void Initialize(IInputController gameController, IInputController menuController)
        {
            GameActiveController = gameController;
            GameActiveController?.Initialize();
            MenuActiveController = menuController;
            MenuActiveController?.Initialize();
        }

        public void Shutdown()
        {
            GameActiveController?.Shutdown();
            GameActiveController = null;
            MenuActiveController?.Shutdown();
            MenuActiveController = null;
            GameControllerChanged = null;
        }
        
        public IInputController GetGameActiveController(InputControllerChanged inputChangeHandler = null)
        {
            if (inputChangeHandler != null)
                GameControllerChanged += inputChangeHandler;
            return GameActiveController;
        }

        public IInputController GetUIInputController()
        {
            throw new System.NotImplementedException();
        }
        
        #endregion
        
    }
}