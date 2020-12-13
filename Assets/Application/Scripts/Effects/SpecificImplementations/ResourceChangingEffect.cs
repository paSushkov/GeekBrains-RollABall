using LabirinthGame.Stats;

namespace LabirinthGame.Effects
{
    public abstract class ResourceChangingEffect : EffectBase
    {
        protected readonly CharacterResourceType type = CharacterResourceType.Undefined;
        protected CharacterResource _resource = null;
        protected float _amount;

        public ResourceChangingEffect(IEffectApplicable target, CharacterResourceType type, float amount,
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
                statsOwner.StatHolder?.TryGetResource(type, out _resource);
            }
        }
    }
}