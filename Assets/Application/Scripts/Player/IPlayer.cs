using Application.Scripts.Common.Interfaces;
using LabirinthGame.Common.Interfaces;
using LabirinthGame.Effects;
using LabirinthGame.Stats;
using LabirinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabirinthGame.Player
{
    public interface IPlayer : IMovable, ITrackable, IHaveStats, IEffectApplicable, IPlayerLoop, IHaveTransform
    {
        Vector3 MoveDirection { get; set; }
        void Initialize(Transform gameTransform, IPlayerLoopProcessor playerLoopProcessor, Stat speedStat);
        void Shutdown();

    }
}