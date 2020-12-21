using LabyrinthGame.Effects;
using LabyrinthGame.Managers;
using UnityEngine;

namespace LabyrinthGame.Collectibles
{
    public class CollectibleFactory : ICollectibleFactory
    {

        public CollectibleBase GetRandomCollectible()
        {
            return Random.Range(0, 2) > 0 ? GetEffectCollectible() : GetSimpleCollectible();
        }
        
        public CollectibleBase GetEffectCollectible(EffectBase effect = null, EffectDuration duration = EffectDuration.Undefined)
        {
            if (effect == null)
            {
                effect = MasterManager.Instance.LinksHolder.GameEffectFactory.MakeRandomEffect(duration);
            }
            return new StatChangingCollectible(effect);
        }

        public CollectibleBase GetSimpleCollectible()
        {
            return new CollectibleBase();
        }
    }
}