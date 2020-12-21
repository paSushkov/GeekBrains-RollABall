using LabyrinthGame.Common.Interfaces;
using LabyrinthGame.Effects;
using LabyrinthGame.Stats;
using LabyrinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabyrinthGame.Player
{
    public interface IPlayer : IMovable, ITrackable, IHaveStats, IEffectApplicable, IPlayerLoop, IHaveTransform, IJump
    {
        Vector3 MoveDirection { get; set; }
        void Initialize(Transform gameTransform, IPlayerLoopProcessor playerLoopProcessor, StatsDictionary stats, float jumpPower);
        void Shutdown();

    }
}