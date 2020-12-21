using Sirenix.OdinInspector;
using UnityEngine;

namespace LabyrinthGame.Stats
{
    [CreateAssetMenu(menuName = "Sushkov/Assets/StatsSetLibrary")]
    public class StatsAsset : SerializedScriptableObject
    {
        [SerializeField] private StatsDictionary playerStatDictionary = new StatsDictionary();

        public StatsDictionary DefaultPlayerStats => playerStatDictionary;
    }
}