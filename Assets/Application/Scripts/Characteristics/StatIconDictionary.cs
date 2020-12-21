using System;
using LabyrinthGame.Tech.UnitySerializedDictionary;
using UnityEngine;

namespace LabyrinthGame.Stats
{
        [Serializable]
        public class StatIconDictionary : UnitySerializedDictionary<StatType,Sprite> { }
}