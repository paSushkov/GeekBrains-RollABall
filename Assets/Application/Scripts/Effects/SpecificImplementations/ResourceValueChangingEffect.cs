using LabirinthGame.Stats;

namespace LabirinthGame.Effects
{
    public class ResourceValueChangingEffect : ResourceChangingEffect
    {
        public ResourceValueChangingEffect(IEffectApplicable target, CharacterResourceType type, float amount, float duration, EffectDuration durationType, EffectType effectType) : base(target, type, amount, duration, durationType, effectType)
        {
        }

        public override void OnApplyEffect()
        {
            base.OnApplyEffect();
            if (_resource != null)
            {
                _resource.CurrentValue += _amount;
            }
        }
        protected override void OnTickEffect()
        {
        }

        protected override void OnExpireEffect()
        {
            if (DurationType != EffectDuration.Permanent && _resource != null)
            {
                _resource.CurrentValue -= _amount;
            }
        }
    }
}