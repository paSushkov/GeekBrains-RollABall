using UnityEngine;

namespace LabyrinthGame.LevelGenerator
{
    [CreateAssetMenu(menuName = "Sushkov/Level/MainLevelElements")]
    public class LabyrinthElementsHolder : ScriptableObject, IlabirinthElementsHolder
    {
        [SerializeField] private GameObject[] walls = null;
        [SerializeField] private GameObject[] cells= null;
        
        public GameObject[] Walls => walls;
        public GameObject[] Cells => cells;
    }
}