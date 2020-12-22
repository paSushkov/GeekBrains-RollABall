using System.Collections.Generic;
using System.Linq;
using LabyrinthGame.Common;
using LabyrinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabyrinthGame.Managers
{
    public class MasterMonoBehaviour : Singleton<MasterMonoBehaviour>, IPlayerLoopProcessor
    {
        [SerializeField] private MasterManager masterManager = null;
        [SerializeField] private Transform canvas = null;
        private event PlayerLoopProcess UpdateHandlers = null;
        private event PlayerLoopProcess FixedUpdateHandlers = null;
        private event PlayerLoopProcess LateUpdateHandlers = null;
        private readonly Dictionary<IPlayerLoopSubscriptionController, IPlayerLoop> _loopSubprocessors
            = new Dictionary<IPlayerLoopSubscriptionController, IPlayerLoop>();

        private Vector3[] test;

        #region Unity events

        private void Awake()
        {
            if (masterManager)
                masterManager.Initialize(this, canvas);
            test = masterManager.LinksHolder.Labyrinth.CellsPositions;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            masterManager.Shutdown();
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;

            foreach (var test1 in test)
            {
            Debug.DrawRay(test1, Vector3.up*8);
            }
            
            foreach (var subprocessor in _loopSubprocessors)
            {
                subprocessor.Value.ProcessUpdate(deltaTime);
            }
            
            UpdateHandlers?.Invoke(deltaTime);
        }

        private void FixedUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;
            foreach (var subprocessor in _loopSubprocessors)
            {
                subprocessor.Value.ProcessFixedUpdate(deltaTime);
            }

            FixedUpdateHandlers?.Invoke(deltaTime);
        }     
        
        private void LateUpdate()
        {
            var deltaTime = Time.deltaTime;
            foreach (var subprocessor in _loopSubprocessors)
            {
                subprocessor.Value.ProcessLateUpdate(deltaTime);
            }

            FixedUpdateHandlers?.Invoke(deltaTime);
        }

        #endregion


        #region Public methods

        #endregion


        #region IPlayerLoopProcessor implementation

        public void SubscribeToLoop(IPlayerLoop subProcessor)
        {
            Register(subProcessor);
        }

        public void SubscribeUpdate(PlayerLoopProcess process)
        {
            UpdateHandlers += process;
        }

        public void UnsubscribeFromLoop(IPlayerLoop subProcessor)
        {
            Unregister(subProcessor);
        }

        public void UnsubscribeUpdate(PlayerLoopProcess process)
        {
            UpdateHandlers -= process;
        }


        public void SubscribeFixedUpdate(PlayerLoopProcess process)
        {
            FixedUpdateHandlers += process;
        }

        public void UnsubscribeFixedUpdate(PlayerLoopProcess process)
        {
            FixedUpdateHandlers -= process;
        }

        public void SubscribeLateUpdate(PlayerLoopProcess process)
        {
            LateUpdateHandlers += process;
        }

        public void UnsubscribeLateUpdate(PlayerLoopProcess process)
        {
            LateUpdateHandlers -= process;
        }

        public void Shutdown()
        {
            var subscribers = _loopSubprocessors.Keys.ToArray();
            
            for (var i = 0; i < subscribers.Length; i++)
            {
                subscribers[i].UnsubscribeFromLoop();
            }
        }

        #endregion


        #region Private methods
        
        private void Register(IPlayerLoop loopSubprocessor)
        {
            var loopSubprocessorSubscribtionController = loopSubprocessor?.PlayerLoopSubscriptionController;

            if (loopSubprocessor != null && loopSubprocessorSubscribtionController != null &&
                !_loopSubprocessors.ContainsKey(loopSubprocessorSubscribtionController))
            {
                _loopSubprocessors.Add(loopSubprocessorSubscribtionController, loopSubprocessor);
            }
        }

        private void Unregister(IPlayerLoop loopSubprocessor) 
        {
            var loopSubprocessorSubscribtionController = loopSubprocessor?.PlayerLoopSubscriptionController;

            if (loopSubprocessor != null && loopSubprocessorSubscribtionController != null &&
                !_loopSubprocessors.ContainsKey(loopSubprocessorSubscribtionController))
            {
                _loopSubprocessors.Remove(loopSubprocessorSubscribtionController);
            }
        }

        #endregion
    }
}