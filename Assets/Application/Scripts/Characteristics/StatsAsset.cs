using Sirenix.OdinInspector;
using UnityEngine;

namespace LabyrinthGame.Stats
{
    [CreateAssetMenu(menuName = "Sushkov/Assets/StatsSetLibrary")]
    public class StatsAsset : SerializedScriptableObject
    {
        [SerializeField] private StatIconDictionary statsIcons = new StatIconDictionary();
        [SerializeField] private StatsDictionary playerStatDictionary = new StatsDictionary();
        [SerializeField] private float defaultJumpPower = 1f;
        public StatsDictionary DefaultPlayerStats => playerStatDictionary;
        public float DefaultJumpPower => defaultJumpPower;

        public Sprite GetStatIcon(StatType type)
        {
            return statsIcons.ContainsKey(type) ? statsIcons[type] : statsIcons[StatType.Undefined];
        }
    }
}