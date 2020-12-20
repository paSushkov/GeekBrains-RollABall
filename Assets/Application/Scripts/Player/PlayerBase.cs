using LabirinthGame.Common.Interfaces;
using LabirinthGame.Effects;
using LabirinthGame.Stats;
using LabirinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabirinthGame.Player
{
    public abstract class PlayerBase : IPlayer
    {
        #region Private data

        protected EffectController effectController;
        private bool isSubscribedForInput;
        private bool isSubscribedForSpeedChange;

        #endregion


        #region IPlayer implementation

        public Vector3 MoveDirection { get; set; }
        
        public virtual void Initialize(Transform gameTransform, IPlayerLoopProcessor playerLoopProcessor, Stat speedStat)
        {
            GameTransform = gameTransform;
            StatHolder.AddStat(StatType.Speed, speedStat);
            MoveSpeed = speedStat.CurrentValue;
            SubscribeForSpeedChange();
            PlayerLoopSubscriptionController.Initialize(this, playerLoopProcessor);
            PlayerLoopSubscriptionController.SubscribeToLoop();
            if (effectController == null)
                effectController = new EffectController(this);
        }

        public virtual void Shutdown()
        {
            GameTransform = null;
            UnsubscribeFromSpeedChange();
            PlayerLoopSubscriptionController.Shutdown();
            OnPositionChange = null;
            effectController.RemoveAllInstantly();
            StatHolder.Clear();
        }

        #region IMovable implementation

        public float MoveSpeed { get; private set; }

        public void MoveTo(Vector3 position)
        {
            GameTransform.position = position;
        }

        public abstract void MoveTowards(Vector3 position, float speed);

        public abstract void Move(Vector3 direction, float speed);

        #endregion


        #region ITrackable implementation

        public event PositionChangeProcessor OnPositionChange;

        public Vector3 Position => GameTransform ? GameTransform.position : Vector3.zero;

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
            if (GameTransform.hasChanged)
            {
                OnPositionChange?.Invoke(GameTransform.position);
                GameTransform.hasChanged = false;
            }

            StatHolder.ProcessFixedUpdate(fixedDeltaTime);

        }

        public void ProcessLateUpdate(float fixedDeltaTime)
        {
        }

        #endregion

        
        #region IHaveTransform implementation

        public Transform GameTransform { get; private set; }

        #endregion
        
        #endregion

        
        #region Private methods

        protected void ProcessSpeedChange(float newSpeed)
        {
            MoveSpeed = newSpeed;
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