using System.Collections.Generic;
using System.Linq;
using LabyrinthGame.Managers;
using LabyrinthGame.Tech.PlayerLoop;
using LabyrinthGame.UI;
using UnityEngine;

namespace LabyrinthGame.Stats
{
    // TODO: Re-implement with interface
    public class StatHolder : IPlayerLoop
    {
        #region PrivateData

        private readonly Dictionary<StatType, Stat> _stats = new Dictionary<StatType, Stat>();
        private readonly List<IPlayerLoop> _selfDynamicStats = new List<IPlayerLoop>();
        private Dictionary<Stat, BarHandler> _statBars = new Dictionary<Stat, BarHandler>();

        #endregion

        #region Properties

        public StatType[] ActiveStats => _stats.Keys.ToArray();

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

            var statBarObj = MasterManager.Instance.InstantiateObject(
                MasterManager.Instance.LinksHolder.StatBarPrefab,
                MasterManager.Instance.LinksHolder.StatBarHud.transform);


            BarHandler handler;
            if (!statBarObj.TryGetComponent(out handler))
                handler = statBarObj.AddComponent<BarHandler>();
            handler.Initialize(stat, type);                    
            _statBars.Add(stat, handler);
            
        }
        
        public void RemoveStat(StatType type)
        {
            if (_stats.TryGetValue(type, out var stat))
            {
                if (stat is IPlayerLoop dynamic && _selfDynamicStats.Contains(dynamic))
                {
                    _selfDynamicStats.Remove(dynamic);
                }

                if (_statBars[stat])
                {
                    Object.Destroy(_statBars[stat]);
                }
                _stats.Remove(type);
                _statBars.Remove(stat);
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