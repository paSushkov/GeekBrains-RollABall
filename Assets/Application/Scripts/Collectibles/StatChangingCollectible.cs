using LabyrinthGame.Effects;
using LabyrinthGame.Managers;
using UnityEngine;

namespace LabyrinthGame.Collectibles
{
    public class StatChangingCollectible : CollectibleBase
    {
        private readonly EffectBase _effect;

        public StatChangingCollectible(EffectBase effect)
        {
            _effect = effect;
        }

        public override void Collect(Collider other)
        {
            if (MasterManager.Instance.LinksHolder.TryGetTransformOwner(other.transform, out var owner))
            {
                if (!(owner is IEffectApplicable target)) return;
                target.ApplyEffect(_effect);
                base.Collect(other);
            }
        }
    }
}