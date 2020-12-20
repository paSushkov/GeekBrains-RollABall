using LabirinthGame.Stats;

namespace LabirinthGame.Effects
{
    public class StatValueChangingEffect : StatChangingEffect
    {
        private ExtraValue extraValue;

        public StatValueChangingEffect(IEffectApplicable target, StatType type, float amount, float duration,
            EffectDuration durationType, EffectType effectType) : base(target, type, amount, duration, durationType,
            effectType)
        {
            extraValue = new ExtraValue(amount);
        }

        public override void OnApplyEffect()
        {
            base.OnApplyEffect();
            stat?.AddExtraValue(ref extraValue);
        }

        protected override void OnTickEffect()
        {
        }

        protected override void OnExpireEffect()
        {
            if (DurationType != EffectDuration.Permanent)
            {
                stat?.RemoveExtraValue(ref extraValue);
            }
        }
    }
}