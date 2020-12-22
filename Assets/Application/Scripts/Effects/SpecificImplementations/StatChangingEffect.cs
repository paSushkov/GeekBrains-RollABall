using LabyrinthGame.Stats;
using UnityEngine;

namespace LabyrinthGame.Effects
{
    public abstract class StatChangingEffect : EffectBase
    {
        protected readonly StatType affectStatType = StatType.Undefined;
        protected Stat stat = null;
        protected float _amount;

        public StatType AffectStatType => affectStatType;
        public float Amount => _amount;


        public StatChangingEffect(StatType affectStatType, float amount, float duration, EffectDuration durationType,
            EffectType effectType, Sprite icon) : base(duration, durationType, effectType, icon)
        {
            this.affectStatType = affectStatType;
            _amount = amount;
        }

        public override void OnApplyEffect(IEffectApplicable effectTarget)
        {
            base.OnApplyEffect(effectTarget);
            if (target is IHaveStats statsOwner)
            {
                statsOwner.StatHolder?.TryGetStat(AffectStatType, out stat);
            }
        }
    }
}