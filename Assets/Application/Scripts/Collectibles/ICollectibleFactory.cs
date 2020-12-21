using LabyrinthGame.Effects;

namespace LabyrinthGame.Collectibles
{
    public interface ICollectibleFactory
    {
        CollectibleBase GetRandomCollectible();
        CollectibleBase GetSimpleCollectible();
        CollectibleBase GetEffectCollectible(EffectBase effect = null, EffectDuration duration = EffectDuration.Undefined);

    }
}