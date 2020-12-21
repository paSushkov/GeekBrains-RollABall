using UnityEngine;

namespace LabyrinthGame.LevelGenerator
{
    public interface IlabirinthElementsHolder
    {
        GameObject[] Walls { get; }
        GameObject[] Cells { get; }
    }
}