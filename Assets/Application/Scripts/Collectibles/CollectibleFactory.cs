using System.Collections.Generic;
using LabyrinthGame.Effects;
using LabyrinthGame.Managers;
using LabyrinthGame.SerializebleData;
using UnityEngine;

namespace LabyrinthGame.Collectibles
{
    public class CollectibleFactory : ICollectibleFactory
    {
        public CollectibleBase[] UnpackCollectibles (CollectibleData[] data)
        {
            var lenght = data.Length;
            var effectFactory = MasterManager.Instance.LinksHolder.GameEffectFactory;
            var result = new CollectibleBase[lenght];
            
            for (var i = 0 ; i < lenght; i++)
            {
                if (data[i].effectData.isFake)
                {
                    result[i] = GetSimpleCollectible();
                }
                else
                {
                    var effect = effectFactory.UnpackEffectLoosely(data[i].effectData);
                    result[i] = GetEffectCollectible(effect, data[i].effectData.durationType);
                }

            }
            return result;
        }
        
        
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

    public class WrappedCollectibles
    {
        public  List<CollectibleBase> collectibles;
        public List<Vector3> positions;
        public List<bool> isMandatory;
        
    }
}