using LabyrinthGame.Common;
using LabyrinthGame.Common.Interfaces;
using LabyrinthGame.GameRadar;
using UnityEngine;

namespace LabyrinthGame.Collectibles
{
    public interface ICollectible : IHaveTransform, IListenTrigger, IRadarTrackable
    {
        bool IsMandatory { get; }
        void Initialize(bool isMandatory,Transform gameTransform, TriggerListener listener, LayerMask reactLayers, Sprite radarIcon);
        void Shutdown();
        void Collect(Collider collider);
    }
}