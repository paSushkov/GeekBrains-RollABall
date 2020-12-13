using System;
using LabirinthGame.Player;
using LabirinthGame.Stats;
using TMPro;
using UnityEngine;

namespace LabirinthGame.Temp
{
    public class RegenDebug : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI valueText = null;
        [SerializeField] private PlayerBase player = null;
        [SerializeField] private CharacterResourceType type = CharacterResourceType.Undefined;
        private RegenerativeCharacterResource _regenerative;
        private bool _isSubscibed;
        
        
        private void Awake()
        {
            player.StatHolder.TryGetResource(type, out var resource);
            if (resource is RegenerativeCharacterResource regenerative)
            {
                _regenerative = regenerative;
                RegenValueChanged(_regenerative.CurrentRegenerationAmount);
            }
        }

        private void OnEnable()
        {
            SubscribeForSource();
        }

        private void OnDisable()
        {
            UnsubscribeFromSource();
        }

        private void SubscribeForSource()
        {
            if (_isSubscibed || (_regenerative is null))
                return;
            _regenerative.RegenChanged += RegenValueChanged;
            _isSubscibed = true;
        }

        private void UnsubscribeFromSource()
        {
            _isSubscibed = false;
            if (!_isSubscibed || (_regenerative is null))
                return;
            _regenerative.RegenChanged -= RegenValueChanged;
        }
        
        private void RegenValueChanged(float newValue)
        {
            valueText.text = $"{Enum.GetName(typeof(CharacterResourceType), type)} regen: {newValue.ToString("# ###")}";
        }
        
    }
}