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
        [SerializeField] private float lerpSpeed = 2f;
        [SerializeField] private Color fullColor = Color.green;
        [SerializeField] private Color lowColor = Color.red;
        [SerializeField] bool changeColor;
        private IPlayer player = null;
        [SerializeField] private StatType type;
        private Stat stat;
        private float _fillAmount;
        private bool _isSubscibed;
        private Transform _cameraTransform;
        private Transform _selfTransform;
        
        #endregion
        
        #region Properties
        
        public float MaxValue { get; set; }
        
        
        public float Value
        {
            set
            {
                valueText.text = value.ToString("# ###") + " / " + MaxValue.ToString("# ###");
                _fillAmount = Map(value, 0, MaxValue, 0, 1);
            }
        }

        
        #endregion
        
        
        #region UnityMethods
        
        private void Start()
        {
            player = MasterManager.Instance.LinksHolder.ActivePlayer;
            if (player.StatHolder.TryGetStat(type, out stat))
            {
                SubscribeForSource();
                MaxValue = stat.MaxValue;
                Value = stat.CurrentValue;
        
                if (changeColor)
                    content.color = Color.Lerp(lowColor, fullColor, content.fillAmount);
            }
        
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
        
        private void HandleBar()
        {
            if (content.fillAmount != _fillAmount)
            {
                content.fillAmount = Mathf.Lerp(content.fillAmount, _fillAmount, Time.deltaTime * lerpSpeed);
                if (changeColor)
                    content.color = Color.Lerp(lowColor, fullColor, content.fillAmount);
            }
        
            if (rotateToCamera)
                _selfTransform.LookAt(_cameraTransform);
        }
        
        private void CurrentValueChanged(float newValue)
        {
            Value = newValue;
        }
        
        private float Map(float value, float inMin, float inMax, float outMin, float outMax)
        {
            return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
        }
        
        private void SubscribeForSource()
        {
            if (_isSubscibed || (stat is null))
                return;
            stat.CurrentChanged += CurrentValueChanged;

            _isSubscibed = true;
        }
        
        private void UnsubscribeFromSource()
        {
            _isSubscibed = false;
            if (!_isSubscibed || (stat is null))
                return;
            stat.CurrentChanged -= CurrentValueChanged;
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