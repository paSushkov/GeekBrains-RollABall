using LabirinthGame.Common.Interfaces;
using LabirinthGame.Effects;
using LabirinthGame.Stats;
using LabirinthGame.Tech.Input;
using LabirinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabirinthGame.Player
{
    public abstract class PlayerBase : IPlayer
    {
        #region Private data

        protected Vector3 moveInput;
        protected EffectController effectController;
        protected IInputListener _inputListener;
        private bool isSubscribedForInput;
        private bool isSubscribedForSpeedChange;

        #endregion


        #region Public methods

        public virtual void Initialize(Transform modelTransform, IPlayerLoopProcessor playerLoopProcessor, Stat speedStat, IInputListener inputListener)
        {
            _inputListener = inputListener;
            ModelTransform = modelTransform;
            
            StatHolder.AddStat(StatType.Speed, speedStat);
            SelfMoveSpeed = speedStat.CurrentValue;
            SubscribeForSpeedChange();
            PlayerLoopSubscriptionController.Initialize(this, playerLoopProcessor);
            PlayerLoopSubscriptionController.SubscribeToLoop();
            if (effectController == null)
                effectController = new EffectController(this);
        }

        public virtual void Shutdown()
        {
            ModelTransform = null;
            UnsubscribeFromSpeedChange();
            PlayerLoopSubscriptionController.Shutdown();
            OnPositionChange = null;
            effectController.RemoveAllInstantly();
            StatHolder.Clear();
        }


        #endregion

        #region IPlayer implementation

        public Transform ModelTransform { get; private set; }


        #region IMovable implementation

        public float SelfMoveSpeed { get; private set; }

        public void MoveTo(Vector3 position)
        {
            ModelTransform.position = position;
        }

        public abstract void MoveTowards(Vector3 position, float speed);

        public abstract void Move(Vector3 direction, float speed);

        #endregion


        #region ITrackable implementation

        public event PositionChangeProcessor OnPositionChange;

        public Vector3 Position => ModelTransform ? ModelTransform.position : Vector3.zero;

        #endregion


        #region IHaveStats implementation

        public StatHolder StatHolder { get; } = new StatHolder();

        #endregion


        #region IEffectApplicable implementation

        public void ApplyEffect(EffectBase effect)
        {
            effectController.ApplyEffect(effect);
        }

        #endregion


        #region IPlayerLoop implementation

        public IPlayerLoopSubscriptionController PlayerLoopSubscriptionController { get; }
            = new PlayerLoopSubscriptionController();

        public virtual void ProcessUpdate(float deltaTime)
        {
        }

        public virtual void ProcessFixedUpdate(float fixedDeltaTime)
        {
            Debug.Log("ProcessFixedUpdate BASE");
            if (ModelTransform.hasChanged)
            {
                OnPositionChange?.Invoke(ModelTransform.position);
                ModelTransform.hasChanged = false;
            }

            StatHolder.ProcessFixedUpdate(fixedDeltaTime);

        }

        public void ProcessLateUpdate(float fixedDeltaTime)
        {
        }

        #endregion


        #endregion

        #region Private methods

        protected void ProcessSpeedChange(float newSpeed)
        {
            SelfMoveSpeed = newSpeed;
        }

        private void SubscribeForSpeedChange()
        {
            if (!isSubscribedForSpeedChange && StatHolder.TryGetStat(StatType.Speed, out var speed))
            {
                speed.CurrentChanged += ProcessSpeedChange;
                isSubscribedForSpeedChange = true;
            }
        }

        private void UnsubscribeFromSpeedChange()
        {
            if (isSubscribedForSpeedChange && StatHolder.TryGetStat(StatType.Speed, out var speed))
            {
                speed.CurrentChanged -= ProcessSpeedChange;
            }

            isSubscribedForSpeedChange = false;
        }

        #endregion
    }
}