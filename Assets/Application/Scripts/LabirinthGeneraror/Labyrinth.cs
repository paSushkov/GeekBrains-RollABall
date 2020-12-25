using UnityEngine;

namespace LabyrinthGame.LevelGenerator
{
    public class Labyrinth
    {
        public Transform collectiblesRoot;

        private readonly Transform root;
        private readonly Vector3[] wallsPositions;
        private readonly Vector3[] cellsPositions;

        public Transform Root => root;
        public Vector3[] WallsPositions => wallsPositions;
        public Vector3[] CellsPositions => cellsPositions;

        public Labyrinth(Vector3[] walls, Vector3[] cells, Transform root)
        {
            this.root = root;
            wallsPositions = walls;
            cellsPositions = cells;
        }
    }
}