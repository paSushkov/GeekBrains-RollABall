using LabirinthGame.Common.Interfaces;
using LabirinthGame.Effects;
using LabirinthGame.Stats;
using UnityEngine;

namespace LabirinthGame.Player
{
    public abstract class PlayerBase : MonoBehaviour, IMovable, ITrackable, IHaveStats, IEffectApplicable
    {
        #region Delegates and events

        public event PositionChangeProcessor OnPositionChange;

        #endregion
        

        #region PrivateData

        protected float moveSpeed;
        [SerializeField]
        protected StatHolder statHolder = new StatHolder();
        private bool isSubscribedForSpeedChange;
        protected EffectController effectController;

        #endregion
        
        
        #region Properties
        
        public Transform CachedTransform { get; private set; }
        
        #endregion


        #region Unity events

        protected virtual void Awake()
        {
            AwakeInitialize();
        }

        protected virtual void OnDestroy()
        {
            DestroyShutdown();
        }

        protected virtual void FixedUpdate()
        {
            if (CachedTransform.hasChanged)
            {
                OnPositionChange?.Invoke(CachedTransform.position);
                CachedTransform.hasChanged = false;
            }
            effectController.ProcessUpdate(Time.fixedDeltaTime);
            statHolder.ProcessUpdate(Time.fixedDeltaTime);
        }

        #endregion


        #region Stats subscription management

        private void SubscribeForSpeedChange()
        {
            if (!isSubscribedForSpeedChange && statHolder.TryGetCharacteristic(CharacteristicType.Speed, out var speed))
            {
                speed.CurrentChanged += ProcessSpeedChange;
                isSubscribedForSpeedChange = true;

            }
        }
        
        private void UnsubscribeFromSpeedChange()
        {
            if (isSubscribedForSpeedChange && statHolder.TryGetCharacteristic(CharacteristicType.Speed, out var speed))
            {
                speed.CurrentChanged -= ProcessSpeedChange;
            }
            isSubscribedForSpeedChange =false;
        }


        #endregion


        #region Private methods

        protected virtual void AwakeInitialize()
        {
            effectController = new EffectController(this);
            statHolder.Initialize();
            CachedTransform = transform;
            SubscribeForSpeedChange();

            if (statHolder.TryGetCharacteristic(CharacteristicType.Speed, out var speedStat))
                moveSpeed = speedStat.CurrentValue;
            else
                Debug.LogWarning($"{this.name} trying to find SPEED characteristic, but cant find it!", gameObject);
        }

        protected virtual void DestroyShutdown()
        {
            UnsubscribeFromSpeedChange();
        }

        protected void ProcessSpeedChange(float newSpeed)
        {
            moveSpeed = newSpeed;
        }

        #endregion
        

        #region IMovable implementation

        public void MoveTo(Vector3 position)
        {
            CachedTransform.position = position;
        }

        public abstract void MoveTowards(Vector3 position, float speed);
        
        public abstract void Move(Vector3 direction, float speed);

        #endregion

        
        #region ITrackable implementation

        public Vector3 GetPosition()
        {
            return CachedTransform.position;
        }

        #endregion

        #region IHaveStats implementation

        public StatHolder StatHolder => statHolder;

        #endregion


        #region IEffectApplicable implementation

        public void ApplyEffect(EffectBase effect)
        {
            effectController.ApplyEffect(effect);
        }
        
        #endregion
    }
}