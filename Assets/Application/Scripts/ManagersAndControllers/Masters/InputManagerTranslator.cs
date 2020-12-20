using System;
using System.Collections.Generic;
using LabirinthGame.Common.Handlers;
using LabirinthGame.Tech.Input;
using LabirinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabirinthGame.Managers
{
    [CreateAssetMenu(menuName = "Sushkov/Managers/InputManagerTranslator")]
    public class InputManagerTranslator : ScriptableObject, IInputTranslator, IPlayerLoop 
    {
        private readonly Dictionary<string, AxisInput> _axesInputs = new Dictionary<string, AxisInput>();
        private readonly List<string> _blacklist = new List<string>();

        #region IInputTranslator implementation

        public void Initialize(IPlayerLoopProcessor loopProcessor)
        {
            PlayerLoopSubscriptionController?.Initialize(this, loopProcessor);
            PlayerLoopSubscriptionController?.SubscribeToLoop();
        }

        public void Shutdown()
        {
            PlayerLoopSubscriptionController?.UnsubscribeFromLoop();
        }

        public void SubscribeToAxisInput(string axisName, AxisInputHandler handler)
        {
            if (_blacklist.Contains(axisName))
                return;

            try
            {
                Input.GetAxis(axisName);
            }
            catch (ArgumentException)
            {
                _blacklist.Add(axisName);
                Debug.LogError($"Requested input from unregistered axis. \"{axisName}\" is added to black ist");
                return;
            }
            
            if (!_axesInputs.ContainsKey(axisName))
                _axesInputs.Add(axisName, new AxisInput());
            
            if (_axesInputs.TryGetValue(axisName, out var axisInput))
                axisInput.AddListener(handler);
        }

        public void UnsubscribeFromAxisInput(string axisName, AxisInputHandler handler)
        {
            if (_axesInputs.TryGetValue(axisName, out var axisInput))
                axisInput.RemoveListener(handler);
        }

        #endregion

        #region IPlayerLoop implementation

        public IPlayerLoopSubscriptionController PlayerLoopSubscriptionController { get; private set; } =
            new PlayerLoopSubscriptionController();

        public void ProcessUpdate(float deltaTime)
        {
            foreach (var axis in _axesInputs)
            {
                axis.Value.Broadcast(Input.GetAxis(axis.Key));
            }
        }

        public void ProcessFixedUpdate(float fixedDeltaTime)
        {
        }

        public void ProcessLateUpdate(float fixedDeltaTime)
        {
        }

        #endregion
    }
}