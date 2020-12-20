using LabirinthGame.Stats;

namespace LabirinthGame.Effects
{
    public abstract class StatChangingEffect : EffectBase
    {
        protected readonly StatType type = StatType.Undefined;
        protected Stat stat = null;
        protected float _amount;

        public StatChangingEffect(IEffectApplicable target, StatType type, float amount,
            float duration, EffectDuration durationType, EffectType effectType) : base(target, duration, durationType,
            effectType)
        {
            this.type = type;
            _amount = amount;
        }

        public override void OnApplyEffect()
        {
            if (target is IHaveStats statsOwner)
            {
                statsOwner.StatHolder?.TryGetStat(type, out stat);
            }
        }
    }
}