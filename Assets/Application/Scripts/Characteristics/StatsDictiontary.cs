using System;
using LabyrinthGame.Tech.UnitySerializedDictionary;

namespace LabyrinthGame.Stats
{
    [Serializable]
    public class StatsDictionary : UnitySerializedDictionary<StatType,StatSet> { }
}