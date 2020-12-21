using System;
using LabyrinthGame.Player;
using LabyrinthGame.Stats;
using TMPro;
using UnityEngine;

namespace LabyrinthGame.Temp
{
    public class StatDebug : MonoBehaviour
    {
        // [SerializeField] private TextMeshProUGUI valueText = null;
        // [SerializeField] private PlayerBase player = null;
        // [SerializeField] private StatType type = StatType.Undefined;
        // private Characteristic _characteristic;
        // private bool _isSubscibed;
        //
        //
        // private void Awake()
        // {
        //     player.StatHolder.TryGetCharacteristic(type, out _characteristic);
        // }
        //
        // private void OnEnable()
        // {
        //     SubscribeForSource();
        // }
        //
        // private void OnDisable()
        // {
        //     UnsubscribeFromSource();
        // }
        //
        // private void SubscribeForSource()
        // {
        //     if (_isSubscibed || (_characteristic is null))
        //         return;
        //     _characteristic.CurrentChanged += CurrentValueChanged;
        //     _isSubscibed = true;
        // }
        //
        // private void UnsubscribeFromSource()
        // {
        //     _isSubscibed = false;
        //     if (!_isSubscibed || (_characteristic is null))
        //         return;
        //     _characteristic.CurrentChanged -= CurrentValueChanged;
        // }
        //
        // private void CurrentValueChanged(float newValue)
        // {
        //     valueText.text = $"{Enum.GetName(typeof(StatType), type)}: {newValue.ToString("# ###")}";
        // }
        
    }
}