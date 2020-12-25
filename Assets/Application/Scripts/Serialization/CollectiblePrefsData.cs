using System.Collections.Generic;
using LabyrinthGame.Effects;
using LabyrinthGame.Stats;
using UnityEngine;

namespace LabyrinthGame.SerializebleData
{
    public class CollectiblePrefsData : IData<SaveData>
    {
        private const string playerPosition = "PlayerPosition";
        private const string collectiblePositions = "CollectiblesPositions";
        private const string collectibleIsMandatory = "CollectiblesMandatory";
        private const string effectFakeFlag = "EffectFake";
        private const string effectTypes = "EffectTypes";
        private const string effectDurationsTypes = "EffectDurationTypes";
        private const string effectAffectStats = "EffectAffectStats";
        private const string effectRegens = "EffectRegenAffects";
        private const string effectAmounts = "EffectAmounts";
        private const string effectDurations = "EffectDirations";
        private const string effectIconNames = "EffectIconNames";
        
        
        public void Save(SaveData data, string path = null)
        {
            
            var lenght = data.collectibles.Length;
            
            var position = new Vector3[lenght];
            var mandatory = new bool[lenght];
            var isFake = new bool[lenght];
            var gameEffectType = new int[lenght];
            var durationEffectType = new int[lenght];
            var statType = new int[lenght];
            var affectRegen = new bool[lenght];
            var amount = new float[lenght];
            var duration = new float[lenght];
            var icon = new string[lenght];

            for (var i = 0; i < lenght; i++)
            {
                var collectible = data.collectibles[i];
                
                position[i] = collectible.position;
                mandatory[i] = collectible.isMandatory;
                var effectData = collectible.effectData;

                isFake[i] = effectData.isFake;
                gameEffectType[i] = (int) effectData.gameEffectType;
                durationEffectType[i] = (int) effectData.durationType;
                statType[i] = (int) effectData.gameStatType;
                affectRegen[i] = effectData.affectRegen;
                amount[i] = effectData.amount;
                duration[i] = effectData.duration;
                icon[i] = effectData.iconName;
            }

            PlayerPrefsX.SetVector3(playerPosition, data.playerPosition);
            PlayerPrefsX.SetVector3Array(collectiblePositions, position);
            PlayerPrefsX.SetBoolArray(collectibleIsMandatory, mandatory);
            PlayerPrefsX.SetBoolArray(effectFakeFlag, isFake);
            PlayerPrefsX.SetIntArray(effectTypes, gameEffectType);
            PlayerPrefsX.SetIntArray(effectDurationsTypes, durationEffectType);
            PlayerPrefsX.SetIntArray(effectAffectStats, statType);
            PlayerPrefsX.SetBoolArray(effectRegens, affectRegen);
            PlayerPrefsX.SetFloatArray(effectAmounts, amount);
            PlayerPrefsX.SetFloatArray(effectDurations, duration);
            PlayerPrefsX.SetStringArray(effectIconNames, icon);
        }

        public SaveData Load(string path = null)
        {
            var result = new SaveData();
            var collectibles = new List<CollectibleData>();
            result.collectibles = new CollectibleData[0]; 
            var playerPos = PlayerPrefsX.GetVector3(playerPosition);
            result.playerPosition = playerPos;
                
            var position = PlayerPrefsX.GetVector3Array(collectiblePositions);
            var mandatory = PlayerPrefsX.GetBoolArray(collectibleIsMandatory);
            var isFake = PlayerPrefsX.GetBoolArray(effectFakeFlag);
            var effectType = PlayerPrefsX.GetIntArray(effectTypes);
            var effectDurationsType = PlayerPrefsX.GetIntArray(effectDurationsTypes);
            var effectAffectStat = PlayerPrefsX.GetIntArray(effectAffectStats);
            var effectRegen = PlayerPrefsX.GetBoolArray(effectRegens);
            var effectAmount = PlayerPrefsX.GetFloatArray(effectAmounts);
            var effectDuration = PlayerPrefsX.GetFloatArray(effectDurations);
            var effectIconName = PlayerPrefsX.GetStringArray(effectIconNames);

            var lenght = position.Length; 
            
            if (mandatory.Length == lenght &&
                effectType.Length == lenght &&
                effectDurationsType.Length == lenght &&
                effectAffectStat.Length == lenght &&
                effectRegen.Length == lenght &&
                effectAmount.Length == lenght &&
                effectDuration.Length == lenght &&
                effectIconName.Length == lenght
            )
            {
                for (var i = 0; i < lenght; i++)
                {
                    EffectData effectData;
                        if (isFake[i])
                            effectData = EffectData.MakeFakeData();
                        else
                        {
                            effectData = new EffectData()
                            {
                                gameEffectType = (EffectType)effectType[i],
                                durationType = (EffectDuration)effectDurationsType[i],
                                gameStatType = (StatType)effectAffectStat[i],
                                affectRegen = effectRegen[i],
                                amount = effectAmount[i],
                                duration = effectDuration[i],
                                iconName = effectIconName[i]
                            };
                        }
                    var collectible = new CollectibleData(position[i], mandatory[i], effectData);
                    collectibles.Add(collectible);
                }

                result.collectibles = collectibles.ToArray();
                return result;
            }
            else
            {
                Debug.LogError("Effect prefs data corrupted");
                return result;
            }
        }
    }

}