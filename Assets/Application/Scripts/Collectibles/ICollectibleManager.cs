using System.Collections.Generic;

namespace LabirinthGame.Collectibles
{
    public interface ICollectibleManager
    {
        List<CollectibleBase> MandatoryCollectibles { get; }
        
    }
}