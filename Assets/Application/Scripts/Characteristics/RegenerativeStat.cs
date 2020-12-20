using LabirinthGame.Common;
using LabirinthGame.Common.Handlers;
using LabirinthGame.Tech.PlayerLoop;

namespace LabirinthGame.Stats
{
    public sealed class RegenerativeStat : Stat, IPlayerLoop
    {
        #region Events

        public event ValueChanged RegenChanged;

        #endregion

        #region PrivateData

        private float _defaultRegenerationAmount;
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

        public RegenerativeStat(
            MinMaxCurrent minMaxCurrent,
            float defaultRegenerationAmount
        ) : base(minMaxCurrent)
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

        private void Regenerate(float deltaTime)
        {
            if (_currentRegenerationAmount > 0 && CurrentValue < MaxValue ||
                _currentRegenerationAmount < 0 && CurrentValue > MinValue)
                CurrentValue += _currentRegenerationAmount * deltaTime;
        }

        #endregion


        #region IPlayerLoop implementation

        public IPlayerLoopSubscriptionController PlayerLoopSubscriptionController { get; }

        public void ProcessUpdate(float deltaTime)
        {
        }

        public void ProcessFixedUpdate(float fixedDeltaTime)
        {
            Regenerate(fixedDeltaTime);
        }

        public void ProcessLateUpdate(float fixedDeltaTime)
        {
        }

        #endregion
    }
}