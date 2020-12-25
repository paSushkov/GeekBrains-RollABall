using System;
using UnityEngine;

namespace LabyrinthGame.SerializebleData
{
    [Serializable]
    public class SaveData
    {
        public Vector3 playerPosition;
        public CollectibleData[] collectibles;
    }
}