using System.Collections.Generic;
using LabyrinthGame.SerializebleData;
using LabyrinthGame.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LabyrinthGame.Effects
{
    [CreateAssetMenu(menuName = "Sushkov/Assets/EffectsSetLibrary")]
    public class EffectsSetLibrary : SerializedScriptableObject
    {

        [SerializeField] private List<EffectSet> effects = new List<EffectSet>();

        public EffectSet GetRandomEffectSet()
        {
            var index = Random.Range(0, effects.Count);
            return effects[index];
        }

        public EffectSet GetEffectSet(EffectType effectType, StatType statType, bool regenEffect)
        {
            foreach (var effect in effects)
            {
                if (effect.GameEffectType == effectType && effect.GameStatType == statType &&
                    effect.AffectRegen == regenEffect)
                {
                    return effect;
                }
            }
            return null;
        }

        public EffectSet GetEffectSet(EffectData data)
        {
            return GetEffectSet(data.gameEffectType, data.gameStatType, data.affectRegen);
        }
    }
}