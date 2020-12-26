using UnityEngine;

namespace LabyrinthGame.GameRadar
{
    public interface IRadarTrackable
    {
        Vector3 RadarPosition { get; }
        Sprite RadarIcon { get; }

        void RegisterToRadar();
        void UnregisterFromRadar();
    }
}