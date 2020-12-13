using UnityEngine;
using System;
using System.Collections.Generic;
using LabirinthGame.Common;
using LabirinthGame.Common.Handlers;
using LabirinthGame.Effects;


namespace LabirinthGame.Stats
{
    [Serializable]
    public class Stat<T> where T : Enum
    {
        #region PrivateData

        [SerializeField] private T _type;
        [SerializeField] private float minValue;
        [SerializeField] private float maxValue;
        [SerializeField] private float normalValue;
        private float _currentValue;
        private List<ExtraValue> _extraValues;

        #endregion


        #region Properties

        public float MinValue => minValue;
        public float MaxValue => maxValue;
        public T Type => _type;

        public float CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = Mathf.Clamp(value, minValue, maxValue);
                CurrentChanged?.Invoke(_currentValue);
            }
        }

        public float DefaultValue
        {
            get => normalValue;
            set => normalValue = Mathf.Clamp(value, minValue, maxValue);
        }

        #endregion


        #region ClassLifeCycles

        public Stat(T type, MinMaxCurrent minMaxCurrent)
        {
            _type = type;
            minValue = minMaxCurrent.minValue;
            SetMaxValue(minMaxCurrent.maxValue);
            DefaultValue = CurrentValue = minMaxCurrent.currentValue;
        }

        #endregion


        #region Methods

        public virtual void Initialize()
        {
            _extraValues = new List<ExtraValue>();
            CurrentValue = normalValue;
        }

        public void SetMaxValue(float value)
        {
            if (value < minValue || Mathf.Approximately(value, minValue))
                maxValue = minValue;
            else
                maxValue = value;
            MinMaxChanged?.Invoke(minValue, maxValue);

            if (_currentValue > maxValue)
                CurrentValue = maxValue;
        }

        public void SetMinValue(float value)
        {
            if (value > maxValue || Mathf.Approximately(value, maxValue))
                minValue = maxValue;
            else
                minValue = value;
            MinMaxChanged?.Invoke(minValue, maxValue);

            if (_currentValue < minValue)
                CurrentValue = minValue;
        }

        public void AddExtraValue(ref ExtraValue extraValue)
        {
            _extraValues.Add(extraValue);
            CurrentValue += extraValue.value;
        }

        public void RemoveExtraValue(ref ExtraValue extraValue)
        {
            if (_extraValues.Contains(extraValue))
            {
                _extraValues.Remove(extraValue);
            }

            _currentValue = normalValue;
            for (var i = 0; i < _extraValues.Count; i++)
            {
                _currentValue += _extraValues[i].value;
            }
            // To trigger clamp-process and shoot event
            CurrentValue += 0;
        }

        #endregion


        #region Events

        public event MinMaxChanged MinMaxChanged;
        public event ValueChanged CurrentChanged;
        
        #endregion
    }
}