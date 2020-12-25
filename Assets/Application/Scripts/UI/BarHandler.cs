using LabyrinthGame.Managers;
using LabyrinthGame.Player;
using LabyrinthGame.Stats;
using LabyrinthGame.Tech.PlayerLoop;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LabyrinthGame.UI
{
    public class BarHandler : MonoBehaviour, IPlayerLoop
    {
        #region PrivateData
        
        [SerializeField] private bool rotateToCamera = true;
        [SerializeField] private Image content = null;
        [SerializeField] private TextMeshProUGUI valueText = null;
        [SerializeField] private TextMeshProUGUI regenText = null;
        [SerializeField] private float lerpSpeed = 2f;
        [SerializeField] private Color fullColor = Color.green;
        [SerializeField] private Color lowColor = Color.red;
        [SerializeField] private Color positiveRegen = Color.green;
        [SerializeField] private Color negativeRegen = Color.red;
        [SerializeField] private Image icon;
        [SerializeField] bool changeColor;
        private Stat _stat;
        private RegenerativeStat _regenStat; 
        private float _fillAmount;
        private bool _isSubscribed;
        private Transform _cameraTransform;
        private Transform _selfTransform;
        private float _minValue;
        private float _maxValue;
        private float _currentValue;
        private float _regenValue;
        
        #endregion
        
        
        #region UnityMethods
        
        public void Initialize(Stat stat, StatType type)
        {
            if (_isSubscribed)
                UnsubscribeFromSource();
            _stat = stat;
            icon.sprite = MasterManager.Instance.LinksHolder.StatsLibrary.GetStatIcon(type);
            _maxValue = _stat.MaxValue;
            _minValue = _stat.MinValue;
            _currentValue = _stat.CurrentValue;
            if (_stat is RegenerativeStat statRegen)
            {
                _regenStat = statRegen;
                _regenValue = _regenStat.CurrentRegenerationAmount;
                regenText.gameObject.SetActive(true);
            }

            SubscribeForSource();

            UpdateValueText();
            UpdateRegenText();

            if (changeColor)
                content.color = Color.Lerp(lowColor, fullColor, content.fillAmount);

            _cameraTransform = MasterManager.Instance.LinksHolder.CameraController.GameTransform;
            _selfTransform = transform;
            PlayerLoopSubscriptionController.Initialize(this, MasterManager.Instance.LinksHolder.PlayerLoopProcessor);
            PlayerLoopSubscriptionController.SubscribeToLoop();
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromSource();
            PlayerLoopSubscriptionController.Shutdown();
        }
        
        #endregion
        
        
        #region Methods

        private void UpdateValueText()
        {
            valueText.text = _currentValue.ToString("# ###") + " / " + _maxValue.ToString("# ###");
            _fillAmount = Map(_currentValue, _minValue, _maxValue, 0, 1);
        }
        
        private void UpdateRegenText()
        {
            regenText.text = _regenValue.ToString("+0.#;-0.#;0");
            regenText.color = _regenValue < 0f ? negativeRegen : positiveRegen; 
        }
        
        

        private void HandleBar()
        {
            if (!content)
                return;
            
            if (content.fillAmount != _fillAmount)
            {
                content.fillAmount = Mathf.Lerp(content.fillAmount, _fillAmount, Time.deltaTime * lerpSpeed);
                if (changeColor)
                    content.color = Color.Lerp(lowColor, fullColor, content.fillAmount);
            }
        
            if (rotateToCamera && _selfTransform)
                _selfTransform.LookAt(_cameraTransform);
        }
        
        private void CurrentValueChanged(float newValue)
        {
            _currentValue = newValue;
            UpdateValueText();
        }

        private void MinMaxChanged(float minValue, float maxValue)
        {
            this._minValue = minValue;
            this._maxValue = maxValue;
            UpdateValueText();
        }
        private void RegenChanged(float regenPower)
        {
            _regenValue = regenPower;
            UpdateRegenText();
        }
        
        private float Map(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }

        private void SubscribeForSource()
        {
            UnsubscribeFromSource();
            _stat.CurrentChanged += CurrentValueChanged;
            _stat.MinMaxChanged += MinMaxChanged;
            if (_regenStat != null)
                _regenStat.RegenChanged += RegenChanged;
            _isSubscribed = true;
        }

        private void UnsubscribeFromSource()
        {
            if (_isSubscribed && _stat !=null)
            {
                _stat.CurrentChanged -= CurrentValueChanged;
                _stat.MinMaxChanged -= MinMaxChanged;
            }
            if (_regenStat != null)
                _regenStat.RegenChanged -= RegenChanged;
            _isSubscribed = false;
        }

        #endregion

        #region IPlayerLoop

        

        public IPlayerLoopSubscriptionController PlayerLoopSubscriptionController { get; } = new PlayerLoopSubscriptionController();
        public void ProcessUpdate(float deltaTime)
        {
        }

        public void ProcessFixedUpdate(float fixedDeltaTime)
        {
        }

        public void ProcessLateUpdate(float fixedDeltaTime)
        {
            HandleBar();
        }
        
        #endregion

    }
}