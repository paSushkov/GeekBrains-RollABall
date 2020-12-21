using System.Collections.Generic;
using LabyrinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabyrinthGame.Stats
{
    // TODO: Re-implement with interface
    public class StatHolder : IPlayerLoop
    {
        #region PrivateData

        private readonly Dictionary<StatType, Stat> _stats = new Dictionary<StatType, Stat>();
        private readonly List<IPlayerLoop> _selfDynamicStats = new List<IPlayerLoop>();

        #endregion
        
        
        #region Stat management

        public bool TryGetStat(StatType type, out Stat stat)
        {
            if (_stats.ContainsKey(type))
            {
                stat = _stats [type];
                return true;
            }

            stat = null;
            return false;
        }
        
        public void AddStat(StatType type, Stat stat)
        {
            if (_stats.ContainsKey(type))
            {
                Debug.LogWarning($"Attempt to add resource of type, which is already presented in {GetType().Name}");
                return;
            }
            
            _stats?.Add(type, stat);
            stat.Initialize();
            
            if (stat is IPlayerLoop processor)
            {
                _selfDynamicStats.Add(processor);
            }
        }
        
        public void RemoveStat(StatType type)
        {
            if (_stats.TryGetValue(type, out var stat))
            {
                if (stat is IPlayerLoop dynamic && _selfDynamicStats.Contains(dynamic))
                {
                    _selfDynamicStats.Remove(dynamic);
                }

                _stats.Remove(type);
            }
            else
            {
                Debug.LogWarning($"Attempt to remove resource of type, which is not presented in {GetType().Name}");
            }
        }
    
        public void Clear()
        {
            _stats.Clear();
            _selfDynamicStats.Clear();
        }

        #endregion
        
        
        #region IPlayerLoop implementation

        public IPlayerLoopSubscriptionController PlayerLoopSubscriptionController { get; }

        public void ProcessUpdate(float deltaTime)
        {
        }

        public void ProcessFixedUpdate(float fixedDeltaTime)
        {
            foreach (var processor in _selfDynamicStats)
                processor.ProcessFixedUpdate(fixedDeltaTime);
        }

        public void ProcessLateUpdate(float fixedDeltaTime)
        {
        }

        #endregion
    }
}