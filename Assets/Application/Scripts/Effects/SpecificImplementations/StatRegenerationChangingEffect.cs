using System;
using LabyrinthGame.Stats;
using UnityEngine;

namespace LabyrinthGame.Effects
{
    [Serializable]
    public class StatRegenerationChangingEffect : StatChangingEffect
    {
        public StatRegenerationChangingEffect(StatType affectStatType, float amount,
            float duration, EffectDuration durationType, EffectType effectType, Sprite icon) : base(affectStatType, amount, duration,
            durationType, effectType, icon)
        {
        }

        public override void OnApplyEffect(IEffectApplicable effectTarget)
        {
            base.OnApplyEffect(effectTarget);
            if (stat != null && stat is RegenerativeStat regenerative)
            {
                regenerative.CurrentRegenerationAmount += Amount;
            }
        }

        protected override void OnTickEffect()
        {
        }

        protected override void OnExpireEffect()
        {
            if (stat != null && stat is RegenerativeStat regenerative)
            {
                regenerative.CurrentRegenerationAmount -= Amount;
            }
        }
    }
}