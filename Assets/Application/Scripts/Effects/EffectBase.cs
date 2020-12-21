using LabyrinthGame.Managers;
using UnityEngine;

namespace LabyrinthGame.Effects
{
    public abstract class EffectBase
    {
        #region Private data

        protected readonly float initialDuration;
        protected readonly EffectDuration durationType;
        protected readonly EffectType effectType;
        protected IEffectApplicable target;
        protected Sprite _effectIcon;

        #endregion


        #region Properties

        public float RemainingDuration { get; protected set; }
        public float InitialDuration => initialDuration;
        public EffectDuration DurationType => durationType;
        public EffectType EffectType => effectType;
        public Sprite EffectIcon => _effectIcon;

        public bool DurationExpired
        {
            get
            {
                if (durationType == EffectDuration.Permanent)
                    return false;
                // To avoid float check for equality to 0
                return !(RemainingDuration > 0f);
            }
        }

        #endregion


        public EffectBase(float duration, EffectDuration durationType, EffectType effectType, Sprite effectIcon)
        {
            RemainingDuration = initialDuration = duration;
            this.durationType = durationType;
            this.effectType = effectType;
            _effectIcon = effectIcon;
        }

        public void DoTick(float deltaTime)
        {
            RemainingDuration -= deltaTime;
            OnTickEffect();
            if (DurationExpired)
                OnExpireEffect();
        }

        public virtual void OnApplyEffect(IEffectApplicable effectTarget)
        {
            target = effectTarget;
        }

        protected abstract void OnTickEffect();
        protected abstract void OnExpireEffect();
    }
}