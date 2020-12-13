using LabirinthGame.Stats;

namespace LabirinthGame.Effects
{
    public class ResourceRegenerationChangingEffect : ResourceChangingEffect
    {
        public ResourceRegenerationChangingEffect(IEffectApplicable target, CharacterResourceType type, float amount,
            float duration, EffectDuration durationType, EffectType effectType) : base(target, type, amount, duration,
            durationType, effectType)
        {
        }

        public override void OnApplyEffect()
        {
            base.OnApplyEffect();
            if (_resource != null && _resource is RegenerativeCharacterResource regenerative)
            {
                regenerative.CurrentRegenerationAmount += _amount;
            }
        }

        protected override void OnTickEffect()
        {
        }

        protected override void OnExpireEffect()
        {
            if (_resource != null && _resource is RegenerativeCharacterResource regenerative)
            {
                regenerative.CurrentRegenerationAmount -= _amount;
            }

        }
    }
}