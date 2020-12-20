using LabirinthGame.Stats;

namespace LabirinthGame.Effects
{
    public class StatRegenerationChangingEffect : StatChangingEffect
    {
        public StatRegenerationChangingEffect(IEffectApplicable target, StatType type, float amount,
            float duration, EffectDuration durationType, EffectType effectType) : base(target, type, amount, duration,
            durationType, effectType)
        {
        }

        public override void OnApplyEffect()
        {
            base.OnApplyEffect();
            if (stat != null && stat is RegenerativeStat regenerative)
            {
                regenerative.CurrentRegenerationAmount += _amount;
            }
        }

        protected override void OnTickEffect()
        {
        }

        protected override void OnExpireEffect()
        {
            if (stat != null && stat is RegenerativeStat regenerative)
            {
                regenerative.CurrentRegenerationAmount -= _amount;
            }

        }
    }
}