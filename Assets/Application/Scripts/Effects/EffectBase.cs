using System;
using LabyrinthGame.SerializebleData;
using LabyrinthGame.Stats;
using UnityEngine;

namespace LabyrinthGame.Effects
{
    [Serializable]
    public class EffectBase
    {
        #region Private data

        [SerializeField] protected float initialDuration;
        [SerializeField] protected EffectDuration durationType;
        [SerializeField] protected EffectType effectType;
        protected IEffectApplicable target;
        protected Sprite _effectIcon;

        #endregion


        #region Properties

        public float RemainingDuration { get; protected set; }
        public float InitialDuration => initialDuration;
        public EffectDuration DurationType => durationType;
        public EffectType EffectType => effectType;
        public Sprite EffectIcon => _effectIcon;

        public bool DurationExpired
        {
            get
            {
                if (durationType == EffectDuration.Permanent)
                    return false;
                // To avoid float check for equality to 0
                return !(RemainingDuration > 0f);
            }
        }

        #endregion


        public EffectBase(float duration, EffectDuration durationType, EffectType effectType, Sprite effectIcon)
        {
            RemainingDuration = initialDuration = duration;
            this.durationType = durationType;
            this.effectType = effectType;
            _effectIcon = effectIcon;
        }

        public void DoTick(float deltaTime)
        {
            RemainingDuration -= deltaTime;
            OnTickEffect();
        }

        public void ExpireNow()
        {
            OnExpireEffect();
        }

        public virtual void OnApplyEffect(IEffectApplicable effectTarget)
        {
            target = effectTarget;
        }

        protected virtual void OnTickEffect()
        {
        }

        protected virtual void OnExpireEffect()
        {
        }

        public static implicit operator EffectData(EffectBase effect)
        {
            var statType = StatType.Undefined;
            var affectRegen = false;
            var amount = 0f;
            var duration = 0f;
            if (effect is StatChangingEffect statEffect)
            {
                statType = statEffect.AffectStatType;
                amount = statEffect.Amount;
                duration = statEffect.RemainingDuration;
                if (effect is StatRegenerationChangingEffect regenEffect)
                    affectRegen = true;
            }
            return new EffectData(effect.EffectType, effect.DurationType, statType, affectRegen, amount, duration,
                effect._effectIcon.name);
        }
    }
}