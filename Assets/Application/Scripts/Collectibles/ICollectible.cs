using LabyrinthGame.Common;
using LabyrinthGame.Common.Interfaces;
using UnityEngine;

namespace LabyrinthGame.Collectibles
{
    public interface ICollectible : IHaveTransform, IListenTrigger
    {
        bool IsMandatory { get; }
        void Initialize(bool isMandatory,Transform gameTransform, TriggerListener listener, LayerMask reactLayers);
        void Shutdown();
        void Collect(Collider collider);
    }
}