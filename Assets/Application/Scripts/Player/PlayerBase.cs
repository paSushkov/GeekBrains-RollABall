using LabyrinthGame.Common.Interfaces;
using LabyrinthGame.Effects;
using LabyrinthGame.Managers;
using LabyrinthGame.Stats;
using LabyrinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabyrinthGame.Player
{
    public abstract class PlayerBase : IPlayer
    {
        #region Private data

        private EffectController effectController;
        private bool isSubscribedForInput;
        private bool isSubscribedForSpeedChange;

        #endregion


        #region IPlayer implementation

        public Vector3 MoveDirection { get; set; }
        
        public virtual void Initialize(Transform gameTransform, IPlayerLoopProcessor playerLoopProcessor, StatsDictionary stats)
        {
            GameTransform = gameTransform;
            RegisterAsTransformOwner();
            MaxMoveSpeed = 50f;
            InitializeStats(stats);
            
            if (StatHolder.TryGetStat(StatType.Speed, out var speed))
            {
                MoveSpeed = speed.CurrentValue;
                MaxMoveSpeed = speed.MaxValue;
                MaxMoveSpeedSqr = MaxMoveSpeed * MaxMoveSpeed;
                SubscribeForSpeedChange();
            }

            PlayerLoopSubscriptionController.Initialize(this, playerLoopProcessor);
            PlayerLoopSubscriptionController.SubscribeToLoop();
            if (effectController == null)
                effectController = new EffectController(this);
        }

        public virtual void Shutdown()
        {
            DisposeTransform();
            GameTransform = null;
            UnsubscribeCurrentSpeedChange();
            PlayerLoopSubscriptionController.Shutdown();
            OnPositionChange = null;
            effectController.RemoveAllInstantly();
            StatHolder.Clear();
        }

        #region IMovable implementation

        public float MoveSpeed { get; private set; }
        public float MaxMoveSpeed { get; private set; }
        public float MaxMoveSpeedSqr { get; private set; }

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
            effectController.ProcessFixedUpdate(fixedDeltaTime);
            StatHolder.ProcessFixedUpdate(fixedDeltaTime);

        }

        public void ProcessLateUpdate(float fixedDeltaTime)
        {
        }

        #endregion

        
        #region IHaveTransform implementation

        public Transform GameTransform { get; private set; }
        public void RegisterAsTransformOwner()
        {
            MasterManager.Instance.LinksHolder.RegisterTransform(this, GameTransform);
        }

        public void DisposeTransform()
        {
            MasterManager.Instance.LinksHolder.DismissTransform(this);
        }

        #endregion
        
        #endregion

        
        #region Private methods

        protected void ProcessSpeedChange(float newSpeed)
        {
            MoveSpeed = newSpeed;
        }
        
        protected void ProcessMaxSpeedChange(float newMin, float newMax)
        {
            MaxMoveSpeed = newMax;
            MaxMoveSpeedSqr = newMax*newMax;
        }

        private void SubscribeForSpeedChange()
        {
            if (!isSubscribedForSpeedChange && StatHolder.TryGetStat(StatType.Speed, out var speed))
            {
                speed.MinMaxChanged += ProcessMaxSpeedChange;
                speed.CurrentChanged += ProcessSpeedChange;
                isSubscribedForSpeedChange = true;
            }
        }

        private void UnsubscribeCurrentSpeedChange()
        {
            if (isSubscribedForSpeedChange && StatHolder.TryGetStat(StatType.Speed, out var speed))
            {
                speed.MinMaxChanged -= ProcessMaxSpeedChange;
                speed.CurrentChanged -= ProcessSpeedChange;
            }

            isSubscribedForSpeedChange = false;
        }

        private void InitializeStats(StatsDictionary stats)
        {
            foreach (var statSet in stats)
            {
                var statType = statSet.Key;
                var statDef = statSet.Value;
                Stat stat; 
                stat = statDef.Regenerative ? new RegenerativeStat(statDef.Amount, statDef.RegenerativeForce) : new Stat(statDef.Amount);    
                StatHolder.AddStat(statType, stat);
            }
        }

        #endregion
        
    }
}