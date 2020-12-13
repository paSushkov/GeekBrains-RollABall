using LabirinthGame.Player;
using LabirinthGame.Stats;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LabirinthGame.UI
{
    public class BarHandler : MonoBehaviour
    {
        #region PrivateData

        [SerializeField] private bool rotateToCamera = true;
        [SerializeField] private Image content = null;
        [SerializeField] private TextMeshProUGUI valueText = null;
        [SerializeField] private float lerpSpeed = 2f;
        [SerializeField] private Color fullColor = Color.green;
        [SerializeField] private Color lowColor = Color.red;
        [SerializeField] bool changeColor;
        [SerializeField] private PlayerBase player = null;
        [SerializeField] private CharacterResourceType type = CharacterResourceType.Undefined;
        private CharacterResource resource;
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

        private void Awake()
        {
            if (player.StatHolder.TryGetResource(type, out resource))
            {
                MaxValue = resource.MaxValue;
                Value = resource.CurrentValue;

                if (changeColor)
                    content.color = Color.Lerp(lowColor, fullColor, content.fillAmount);
            }

            _cameraTransform = UnityEngine.Camera.main?.transform;
            _selfTransform = transform;
        }

        private void OnEnable()
        {
            SubscribeForSource();
        }

        private void OnDisable()
        {
            UnsubscribeFromSource();
        }

        private void LateUpdate()
        {
            HandleBar();
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
            if (_isSubscibed || (resource is null))
                return;
            resource.CurrentChanged += CurrentValueChanged;
            _isSubscibed = true;
        }

        private void UnsubscribeFromSource()
        {
            _isSubscibed = false;
            if (!_isSubscibed || (resource is null))
                return;
            resource.CurrentChanged -= CurrentValueChanged;
        }

        #endregion
    }
}