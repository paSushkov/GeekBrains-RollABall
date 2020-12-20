using LabirinthGame.Common.Interfaces;
using LabirinthGame.Effects;
using LabirinthGame.Stats;
using LabirinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabirinthGame.Player
{
    public interface IPlayer : IMovable, ITrackable, IHaveStats, IEffectApplicable, IPlayerLoop
    {
        Transform ModelTransform { get;}
        
    }
}