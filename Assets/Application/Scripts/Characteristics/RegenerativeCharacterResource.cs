using System;
using LabirinthGame.Common;
using LabirinthGame.Common.Handlers;
using UnityEngine;

namespace LabirinthGame.Stats
{
    [Serializable]
    public sealed class RegenerativeCharacterResource : CharacterResource
    {
        #region Events

        public event ValueChanged RegenChanged;

        #endregion

        #region PrivateData

        [SerializeField] private float _defaultRegenerationAmount;
        private float _currentRegenerationAmount;

        #endregion


        #region Properties

        public float DefaultRegenerationAmount => _defaultRegenerationAmount;

        public float CurrentRegenerationAmount
        {
            get => _currentRegenerationAmount;
            set
            {
                _currentRegenerationAmount = value;
                RegenChanged?.Invoke(_currentRegenerationAmount);
            }
        }

        #endregion


        #region ClassLifeCycles

        public RegenerativeCharacterResource(
            CharacterResourceType type,
            MinMaxCurrent minMaxCurrent,
            float defaultRegenerationAmount
        ) : base(type, minMaxCurrent)
        {
            _defaultRegenerationAmount = _currentRegenerationAmount = defaultRegenerationAmount;
        }

        #endregion


        #region Metods

        public override void Initialize()
        {
            base.Initialize();
            CurrentRegenerationAmount = _defaultRegenerationAmount;
        }

        public void Regenerate(float deltaTime)
        {
            if (_currentRegenerationAmount > 0 && CurrentValue < MaxValue ||
                _currentRegenerationAmount < 0 && CurrentValue > MinValue)
                CurrentValue += _currentRegenerationAmount * deltaTime;
        }

        #endregion
    }
}