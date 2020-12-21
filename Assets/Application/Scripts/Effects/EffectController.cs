using System;
using System.Collections.Generic;
using LabyrinthGame.Managers;
using LabyrinthGame.Stats;
using LabyrinthGame.Tech.PlayerLoop;
using UnityEngine;
using UnityEngine.UI;

namespace LabyrinthGame.Effects
{
    public class EffectController : IPlayerLoop
    {
        private readonly List<EffectBase> _effects;
        private readonly IEffectApplicable _effectsHolder;
        private readonly GameObject _iconPrefab;
        private readonly Transform _effectsHud;
        private readonly Dictionary<EffectBase, GameObject> _icons = new Dictionary<EffectBase, GameObject>();

        public EffectController(IEffectApplicable effectsHolder)
        {
            _effects = new List<EffectBase>();
            _effectsHolder = effectsHolder;
            _effectsHud = MasterManager.Instance.LinksHolder.EffectIconHolder.transform;
            _iconPrefab = MasterManager.Instance.LinksHolder.EffectIconPrefab;
        }

        public void TickEffects(float deltaTime)
        {
            for (var i = 0; i < _effects.Count; i++)
            {
                _effects[i].DoTick(deltaTime);
                if (_effects[i].DurationExpired)
                {
                    _effects[i].ExpireNow();
                    _effects.RemoveAt(i);
                }
            }
        }

        public void ApplyEffect(EffectBase effect)
        {
            _effects.Add(effect);
            effect.OnApplyEffect(_effectsHolder);
            var newIcon = MasterManager.Instance.InstantiateObject(_iconPrefab, _effectsHud);
            _icons.Add(effect, newIcon);
            if (!newIcon.TryGetComponent(out Image iconImage))
            {
                iconImage = newIcon.AddComponent<Image>();
            }

            iconImage.sprite = effect.EffectIcon;

        }

        public void RemoveAllByType(Type type) 
        {
            for (var i = 0; i < _effects.Count; i++)
            {
                if (_effects[i].GetType() == type)
                {
                    _effects[i].ExpireNow();
                    if (_icons[_effects[i]])
                        UnityEngine.Object.Destroy(_icons[_effects[i]]);
                    _effects.RemoveAt(i);
                }
            }
        }

        public void RemoveAllNonPermanent()
        {
            for (var i = 0; i < _effects.Count; i++)
            {
                if (_effects[i].DurationType != EffectDuration.Permanent)
                {
                    _effects[i].ExpireNow();
                    if (_icons[_effects[i]])
                        UnityEngine.Object.Destroy(_icons[_effects[i]]);
                    _effects.RemoveAt(i);

                }

            }
        }
        
        public void RemoveAllByType(EffectType type = EffectType.Undefined, EffectDuration durationType = EffectDuration.Undefined, bool? regenerative = null)
        {
            bool needToRemove = true;
            for (var i = 0; i < _effects.Count; i++)
            {
                // check by type (positive / negative)
                needToRemove = needToRemove && (type == EffectType.Undefined || _effects[i].EffectType == type);
                // check by type of duration
                needToRemove = needToRemove && (durationType == EffectDuration.Undefined || _effects[i].DurationType == durationType);

                if (regenerative.HasValue)
                {
                    needToRemove = needToRemove && (regenerative.Value == _effects[i] is RegenerativeStat);
                }

                if (!needToRemove) continue;
                _effects[i].ExpireNow();
                if (_icons[_effects[i]])
                    UnityEngine.Object.Destroy(_icons[_effects[i]]);
                _effects.RemoveAt(i);

            }
        }

        public void RemoveAllInstantly()
        {
            foreach (var effect in _effects)
            {
                if (_icons[effect])
                    UnityEngine.Object.Destroy(_icons[effect]);

                effect.ExpireNow();
            }
            _effects.Clear();            
        }


        #region IUpdateProcessor implementation

        public IPlayerLoopSubscriptionController PlayerLoopSubscriptionController { get; }

        public void ProcessUpdate(float deltaTime)
        {
        }

        public void ProcessFixedUpdate(float fixedDeltaTime)
        {
            TickEffects(fixedDeltaTime);
        }

        public void ProcessLateUpdate(float fixedDeltaTime)
        {
        }

        #endregion

    }
}