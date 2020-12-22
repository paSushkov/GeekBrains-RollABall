using System;
using LabyrinthGame.Stats;
using UnityEngine;

namespace LabyrinthGame.Effects
{
    [Serializable]
    public class EffectSet
    {
        [SerializeField] private EffectType gameEffectType = EffectType.Undefined;
        [SerializeField] private StatType gameStatType = StatType.Undefined;
        [SerializeField] private bool affectRegen;
        [SerializeField] private float minAmount = 0f;
        [SerializeField] private float maxAmount = 0f;
        [SerializeField] private float minDuration;
        [SerializeField] private float maxDuration;
        [SerializeField] private Sprite icon;
        public EffectType GameEffectType => gameEffectType;
        public StatType GameStatType => gameStatType;
        public bool AffectRegen => affectRegen;
        public float MinAmount => minAmount;
        public float MaxAmount => maxAmount;
        public float MinDuration => minDuration;
        public float MaxDuration => maxDuration;
        public Sprite Icon => icon;
    }
}