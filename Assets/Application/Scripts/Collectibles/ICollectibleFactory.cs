using LabyrinthGame.Effects;
using LabyrinthGame.SerializebleData;

namespace LabyrinthGame.Collectibles
{
    public interface ICollectibleFactory
    {
        CollectibleBase[] UnpackCollectibles(CollectibleData[] data);
        CollectibleBase GetRandomCollectible();
        CollectibleBase GetSimpleCollectible();
        CollectibleBase GetEffectCollectible(EffectBase effect = null, EffectDuration duration = EffectDuration.Undefined);

    }
}