using System;
using UnityEngine;

namespace LabyrinthGame.SerializebleData
{
    [Serializable]
    public struct CollectibleData
    {
        [SerializeField] public Vector3 position;
        [SerializeField] public bool isMandatory;
        [SerializeField] public EffectData effectData;

        public CollectibleData(Vector3 position, bool isMandatory, EffectData effectData)
        {
            this.position = position;
            this.isMandatory = isMandatory;
            this.effectData = effectData;
        }

        public override string ToString()
        {
            var result = $"Position: {position}, Mandatory: {isMandatory}";
            if (effectData.isFake)
                return result;

            result += $" EffectData {effectData.isFake}, {effectData.gameEffectType}, {effectData.durationType} " +
                      $"{effectData.gameStatType}, {effectData.affectRegen}, {effectData.amount}, {effectData.duration}" +
                      $" {effectData.iconName}";
            return result;
        }
    }
}