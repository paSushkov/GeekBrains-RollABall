﻿using LabirinthGame.Managers;
using LabirinthGame.Player;
using LabirinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabirinthGame.Tech.Input
{
    public class GameProcessInputController : IInputController, IPlayerLoop
    {
        #region Public data

        public IPlayer playerController;

        #endregion

        
        #region Private data

        private IInputListener _inputListener;
        private IPlayerLoopProcessor _playerLoopProcessor;
            
        #endregion


        #region IInputController implementation

        public void Initialize()
        {
            _inputListener = MasterManager.Instance.LinksManager.InputListener;
            _playerLoopProcessor = MasterManager.Instance.LinksManager.PlayerLoopProcessor;
            PlayerLoopSubscriptionController.Initialize(this, _playerLoopProcessor);
        }

        public void Shutdown()
        {
            PlayerLoopSubscriptionController.Shutdown();
            _inputListener = null;
            playerController = null;
        }

        public void Start()
        {
            playerController = MasterManager.Instance.LinksManager.ActivePlayer;
            PlayerLoopSubscriptionController.SubscribeToLoop();
        }

        public void Stop()
        {
            PlayerLoopSubscriptionController.UnsubscribeFromLoop();
        }

        public void PauseFor()
        {
            throw new System.NotImplementedException();
        }

        #endregion



        #region IPlayerLoop implementation
        
        public IPlayerLoopSubscriptionController PlayerLoopSubscriptionController { get; } =
            new PlayerLoopSubscriptionController();

        
        public void ProcessUpdate(float deltaTime)
        {
            playerController.MoveDirection = InputToDirection();
        }

        public void ProcessFixedUpdate(float fixedDeltaTime)
        {
        }

        public void ProcessLateUpdate(float fixedDeltaTime)
        {
        }
        
        #endregion

        
        #region Private methods

        private Vector3 InputToDirection()
        {
            var direction = new Vector3(_inputListener.Vertical, 0, -_inputListener.Horizontal);
            return Vector3.ClampMagnitude(direction, 1f);
        }

        #endregion

    }
}