using System;
using UnityEngine;

namespace LabyrinthGame.Common
{
    [Serializable]
    public struct MinMaxCurrent
    {
        #region Fields

        [SerializeField] private float minValue;
        [SerializeField] private float maxValue;
        [SerializeField] private float currentValue;

        #endregion

        public float MinValue => minValue;
        public float MaxValue => maxValue;
        public float CurrentValue => currentValue;
        

        #region ClassLifeCycles

        public MinMaxCurrent(float minValue, float maxValue, float currentValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.currentValue = currentValue;

        }

        #endregion


    }
}