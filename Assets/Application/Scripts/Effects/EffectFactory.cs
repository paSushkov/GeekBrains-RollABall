using LabyrinthGame.Managers;
using LabyrinthGame.Stats;
using Random = UnityEngine.Random;

namespace LabyrinthGame.Effects
{
    public class EffectFactory
    {
        private EffectsSetLibrary _library;

        public EffectFactory()
        {
            _library = MasterManager.Instance.LinksHolder.EffectsSetLibrary;
        }


        #region Health amount effects

        public EffectBase MakeHealEffect()
        {
            var effectSet = _library.GetEffectSet(EffectType.Positive, StatType.Health, false);
            return MakeEffect(effectSet, EffectDuration.Permanent);
        }

        public EffectBase MakeHealEffectTemp()
        {
            var effectSet = _library.GetEffectSet(EffectType.Positive, StatType.Health, false);
            return MakeEffect(effectSet, EffectDuration.Timed);
        }

        public EffectBase MakeDamageEffect()
        {
            var effectSet = _library.GetEffectSet(EffectType.Negative, StatType.Health, false);
            return MakeEffect(effectSet, EffectDuration.Permanent);
        }

        public EffectBase MakeDamageEffectTemp()
        {
            var effectSet = _library.GetEffectSet(EffectType.Positive, StatType.Health, false);
            return MakeEffect(effectSet, EffectDuration.Timed);
        }

        #endregion

        
        #region Health regen effects

        public EffectBase MakeHpRegenUpEffect()
        {
            var effectSet = _library.GetEffectSet(EffectType.Positive, StatType.Health, true);
            return MakeEffect(effectSet, EffectDuration.Permanent);
        }

        public EffectBase MakeHpRegenUpEffectTemp()
        {
            var effectSet = _library.GetEffectSet(EffectType.Positive, StatType.Health, true);
            return MakeEffect(effectSet, EffectDuration.Timed);
        }

        public EffectBase MakeHpRegenDownEffect()
        {
            var effectSet = _library.GetEffectSet(EffectType.Negative, StatType.Health, true);
            return MakeEffect(effectSet, EffectDuration.Permanent);
        }

        public EffectBase MakeHpRegenDownEffectTemp()
        {
            var effectSet = _library.GetEffectSet(EffectType.Positive, StatType.Health, true);
            return MakeEffect(effectSet, EffectDuration.Timed);
        }

        #endregion
        
        
        #region Speed effects

        public EffectBase MakeSpeedUpEffect()
        {
            var effectSet = _library.GetEffectSet(EffectType.Positive, StatType.Speed, false);
            return MakeEffect(effectSet, EffectDuration.Permanent);
        }

        public EffectBase MakeSpeedUpEffectTemp()
        {
            var effectSet = _library.GetEffectSet(EffectType.Positive, StatType.Speed, false);
            return MakeEffect(effectSet, EffectDuration.Timed);
        }

        public EffectBase MakeSpeedDownEffect()
        {
            var effectSet = _library.GetEffectSet(EffectType.Negative, StatType.Speed, false);
            return MakeEffect(effectSet, EffectDuration.Permanent);
        }

        public EffectBase MakeSpeedDownEffectTemp()
        {
            var effectSet = _library.GetEffectSet(EffectType.Positive, StatType.Speed, false);
            return MakeEffect(effectSet, EffectDuration.Timed);
        }

        #endregion
        

        public EffectBase MakeEffect(EffectSet effectSet, EffectDuration durationType = EffectDuration.Undefined)
        {
            if (durationType == EffectDuration.Undefined)
                durationType = Random.Range(0, 2) > 0 ? EffectDuration.Timed : EffectDuration.Permanent;
            
            EffectBase effect;
            var statType = effectSet.GameStatType;
            var amount = Random.Range(effectSet.MinAmount, effectSet.MaxAmount);
            if (effectSet.GameEffectType == EffectType.Negative)
                amount *= -1;
            var duration = 0f;
            if (durationType == EffectDuration.Timed)
                duration = Random.Range(effectSet.MinDuration, effectSet.MaxDuration);
                
            if (effectSet.AffectRegen)
            {
                effect = new StatRegenerationChangingEffect(statType, amount, duration, durationType, effectSet.GameEffectType, effectSet.Icon);
            }
            else
            {
                effect = new StatValueChangingEffect(statType, amount, duration, durationType, effectSet.GameEffectType, effectSet.Icon);
            }

            return effect;
        }

        public EffectBase MakeRandomEffect(EffectDuration durationType = EffectDuration.Undefined)
        {
            var effectSet = MasterManager.Instance.LinksHolder.EffectsSetLibrary.GetRandomEffectSet();
            return MakeEffect(effectSet, durationType);
        }
    }
}