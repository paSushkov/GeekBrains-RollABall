using System;
using System.Collections.Generic;
using LabirinthGame.Tech.PlayerLoop;

namespace LabirinthGame.Effects
{
    public class EffectController : IPlayerLoop
    {
        private readonly List<EffectBase> effects;
        private readonly IEffectApplicable effectsHolder;

        public EffectController(IEffectApplicable effectsHolder)
        {
            effects = new List<EffectBase>();
            this.effectsHolder = effectsHolder;
        }

        public void TickEffects(float deltaTime)
        {
            for (var i = 0; i < effects.Count; i++)
            {
                effects[i].DoTick(deltaTime);
                if (effects[i].DurationExpired)
                    effects.RemoveAt(i);
            }
        }

        public void ApplyEffect(EffectBase effect)
        {
            effects.Add(effect);
            effect.OnApplyEffect();
        }

        public void RemoveAllByType(Type type) 
        {
            for (var i = 0; i < effects.Count; i++)
            {
                if (effects[i].GetType() == type)
                {
                    effects.RemoveAt(i);
                }
            }
        }

        public void RemoveAllNonPermanent()
        {
            for (var i = 0; i < effects.Count; i++)
            {
                if (effects[i].DurationType!=EffectDuration.Permanent)
                    effects.RemoveAt(i);
            }
        }
        
        public void RemoveAllByType(EffectType type = EffectType.Undefined, EffectDuration durationType = EffectDuration.Undefined)
        {
            bool needToRemove;
            for (var i = 0; i < effects.Count; i++)
            {
                // check by type (positive / negative)
                needToRemove = type == EffectType.Undefined || effects[i].EffectType == type;
                // check by type of duration
                needToRemove = durationType == EffectDuration.Undefined || effects[i].DurationType == durationType;

                if (needToRemove)
                   effects.RemoveAt(i);
            }
        }

        public void RemoveAllInstantly()
        {
            effects.Clear();            
        }


        #region IUpdateProcessor implementation

        public IPlayerLoopSubscriptionController PlayerLoopSubscriptionController { get; }

        public void ProcessUpdate(float deltaTime)
        {
        }

        public void ProcessFixedUpdate(float fixedDeltaTime)
        {
            TickEffects(fixedDeltaTime);
        }

        public void ProcessLateUpdate(float fixedDeltaTime)
        {
        }

        #endregion

    }
}