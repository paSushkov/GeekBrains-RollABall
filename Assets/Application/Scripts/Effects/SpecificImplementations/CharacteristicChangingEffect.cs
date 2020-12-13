using LabirinthGame.Stats;

namespace LabirinthGame.Effects
{
    public class CharacteristicChangingEffect: EffectBase 
    {
        private readonly CharacteristicType type;
        private ExtraValue extraValue;
        private Characteristic characteristic;

        public CharacteristicChangingEffect(IEffectApplicable target, CharacteristicType type, float amount, float duration, EffectDuration durationType, EffectType effectType) : base(target, duration, durationType, effectType)
        {
            this.type = type;
            extraValue = new ExtraValue(amount);
        }

        public override void OnApplyEffect()
        {
            if (target is IHaveStats statsOwner)
            {
                if (statsOwner.StatHolder?.TryGetCharacteristic(type, out characteristic) == true)
                {
                    characteristic.AddExtraValue(ref extraValue);
                }
            }
        }

        protected override void OnTickEffect()
        {
        }

        protected override void OnExpireEffect()
        {
            characteristic?.RemoveExtraValue(ref extraValue);
        }
    }
}