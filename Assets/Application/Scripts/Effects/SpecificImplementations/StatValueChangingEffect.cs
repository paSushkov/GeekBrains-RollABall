using LabyrinthGame.Stats;
using UnityEngine;

namespace LabyrinthGame.Effects
{
    public class StatValueChangingEffect : StatChangingEffect
    {
        private ExtraValue extraValue;

        public StatValueChangingEffect(StatType affectStatType, float amount, float duration,
            EffectDuration durationType, EffectType effectType, Sprite icon) : base(affectStatType, amount, duration, durationType,
            effectType, icon)
        {
            extraValue = new ExtraValue(amount);
        }

        public override void OnApplyEffect(IEffectApplicable effectTarget)
        {
            base.OnApplyEffect(effectTarget);
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