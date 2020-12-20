using LabirinthGame.Effects;
using LabirinthGame.Stats;
using UnityEngine;

namespace LabirinthGame.Collectibles
{
    public class StatChangingCollectible : CollectibleBase
    {
        private StatType type = StatType.Undefined;
        private float amount = 50f;
        private float duration = 5f;
        private bool changeRegeneration;

        public StatChangingCollectible(StatType statToChange, float amount, float duration, bool changeRegeneration)
        {
            type = statToChange;
            this.amount = amount;
            this.duration = duration;
            this.changeRegeneration = changeRegeneration;
        }

        protected override void OnCollectEffect(Collider other)
        {
            if (other.TryGetComponent(out IEffectApplicable target))
            {
                EffectBase effect;
                if (changeRegeneration)
                    effect = new StatRegenerationChangingEffect(target, type, amount, duration, duration>0 ? EffectDuration.Timed : EffectDuration.Permanent, amount>0 ? EffectType.Positive : EffectType.Negative);
                else
                effect = new StatValueChangingEffect(target, type, amount, duration, duration>0 ? EffectDuration.Timed : EffectDuration.Permanent, amount>0 ? EffectType.Positive : EffectType.Negative);

                target.ApplyEffect(effect);
                base.OnCollectEffect(other);
            }
        }
    }
}