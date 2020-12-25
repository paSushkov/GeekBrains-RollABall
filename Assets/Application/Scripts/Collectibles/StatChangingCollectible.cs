using System;
using LabyrinthGame.Effects;
using LabyrinthGame.Managers;
using UnityEngine;

namespace LabyrinthGame.Collectibles
{
    [Serializable]
    public class StatChangingCollectible : CollectibleBase
    {
        [SerializeField] protected EffectBase _effect;
        public  EffectBase Effect => _effect;
        
        public StatChangingCollectible(EffectBase effect)
        {
            _effect = effect;
        }


        public override void Collect(Collider other)
        {
            if (MasterManager.Instance.LinksHolder.TryGetTransformOwner(other.transform, out var owner))
            {
                if (!(owner is IEffectApplicable target)) return;
                target.ApplyEffect(Effect);
                base.Collect(other);
            }
        }
    }
}