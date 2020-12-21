using System.Collections.Generic;

namespace LabyrinthGame.Collectibles
{
    public interface ICollectibleManager
    {
        List<CollectibleBase> MandatoryCollectibles { get; }
        
    }
}