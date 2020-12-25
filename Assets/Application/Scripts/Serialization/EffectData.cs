using System;
using LabyrinthGame.Effects;
using LabyrinthGame.Stats;
using UnityEngine;

namespace LabyrinthGame.SerializebleData
{
    [Serializable]
    public struct EffectData
    {
        [SerializeField] public bool isFake;
        [SerializeField] public EffectType gameEffectType;
        [SerializeField] public EffectDuration durationType;
        [SerializeField] public StatType gameStatType;
        [SerializeField] public bool affectRegen;
        [SerializeField] public float amount;
        [SerializeField] public float duration;
        [SerializeField] public string iconName;

        public EffectData(EffectType gameEffectType, EffectDuration durationType, StatType gameStatType,
            bool affectRegen, float amount, float duration, string iconName)
        {
            this.gameEffectType = gameEffectType;
            this.durationType = durationType;
            this.gameStatType = gameStatType;
            this.affectRegen = affectRegen;
            this.amount = amount;
            this.duration = duration;
            this.iconName = iconName;
            isFake = false;
        }

        public static EffectData MakeFakeData()
        {
            var result = new EffectData();
            result.isFake = true;
            return result;
        }

    }
}