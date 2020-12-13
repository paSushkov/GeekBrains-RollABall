using UnityEngine;


namespace LabyrinthGenerator
{
    /// <summary>
    /// Add this script on Game Object
    /// </summary>
    public class Labyrinth : MonoBehaviour
    {


        [SerializeField] private GameObject _cellPrefub = null;
        [SerializeField] private GameObject _wallPrefub = null;
        [SerializeField] private int _labyrinthSizeX = 0;
        [SerializeField] private int _labyrinthSizeZ = 0;


        private void Start()
        {

            LabyrinthGenerator labyrinth = new LabyrinthGenerator(
                transform,
                _labyrinthSizeX,
                _labyrinthSizeZ,
                _cellPrefub,
                _wallPrefub
                );

            labyrinth.GenerateLabyrinth();
        }


    }
}
