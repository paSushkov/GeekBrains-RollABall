﻿using UnityEngine;

namespace LabirinthGame.Common.Interfaces
{
    public delegate void PositionChangeProcessor(Vector3 newPosition);
    public interface ITrackable
    {
        Vector3 Position { get; }
        event PositionChangeProcessor OnPositionChange;
    }
}