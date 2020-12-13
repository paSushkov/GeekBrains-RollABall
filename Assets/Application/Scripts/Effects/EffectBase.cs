namespace LabirinthGame.Effects
{
    public abstract class EffectBase
    {
        #region Private data

        protected readonly float initialDuration;
        protected readonly EffectDuration durationType;
        protected readonly EffectType effectType;
        protected readonly IEffectApplicable target;

        #endregion


        #region Properties

        public float RemainingDuration { get; protected set; }
        public float InitialDuration => initialDuration;
        public EffectDuration DurationType => durationType;
        public EffectType EffectType => effectType;

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


        public EffectBase(IEffectApplicable target, float duration, EffectDuration durationType, EffectType effectType)
        {
            this.target = target;
            RemainingDuration = initialDuration = duration;
            this.durationType = durationType;
            this.effectType = effectType;
        }

        public void DoTick(float deltaTime)
        {
            RemainingDuration -= deltaTime;
            OnTickEffect();
            if (DurationExpired)
                OnExpireEffect();
        }

        public abstract void OnApplyEffect();
        protected abstract void OnTickEffect();
        protected abstract void OnExpireEffect();
    }
}