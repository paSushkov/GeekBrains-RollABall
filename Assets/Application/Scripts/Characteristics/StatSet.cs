using System;
using LabyrinthGame.Common;
using UnityEngine;

namespace LabyrinthGame.Stats
{
    [Serializable]
    public class StatSet
    {
        [SerializeField] private MinMaxCurrent amount;
        [SerializeField] private bool regenerative = false;
        [SerializeField] private float regenerativeForce;

        public MinMaxCurrent Amount => amount;
        public bool Regenerative => regenerative;
        public float RegenerativeForce => regenerativeForce;
    }
}