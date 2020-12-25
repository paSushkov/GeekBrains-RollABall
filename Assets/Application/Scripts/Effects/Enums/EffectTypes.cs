using System;

namespace LabyrinthGame.Effects
{
    [Serializable]
    public enum EffectType
    {
        Undefined,
        Positive,
        Negative,
        Neutral
    }

    [Serializable]
    public enum EffectDuration
    {
        Undefined,
        Timed,
        Permanent
    }
}