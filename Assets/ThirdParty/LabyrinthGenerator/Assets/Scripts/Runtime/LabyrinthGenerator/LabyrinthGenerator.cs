using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace LabyrinthGenerator
{
    public class LabyrinthGenerator : MonoBehaviour
    {
        #region Fields

        private const string PARENT_ROAD = "Road";
        private const string PARENT_WALLS = "Walls";

        private Cell[,] _cells;
        private GameObject _cellPrefub;
        private GameObject _wallPrefub;
        private Transform _parent;
        private float _cellGridSizeX;
        private float _cellGridSizeZ;
        private int _labyrinthSizeX;
        private int _labyrinthSizeZ;

        #endregion


        #region PrivateData

        public LabyrinthGenerator(Transform parent, int labyrinthSizeX, int labyrinthSizeZ, GameObject cellPrefub, GameObject wallPrefub = null)
        {
            _parent = parent;
            _labyrinthSizeX = labyrinthSizeX;
            _labyrinthSizeZ = labyrinthSizeZ;
            _cellPrefub = cellPrefub;
            _wallPrefub = wallPrefub;
        }

        #endregion


        #region Methods


        /// <summary>
        /// Main logic method
        /// </summary>
        public void GenerateLabyrinth()
        {
            var roadParrent = new GameObject { name = PARENT_ROAD };
            roadParrent.transform.SetParent(_parent);

            _cells = new Cell[_labyrinthSizeX, _labyrinthSizeZ];
            var cellsStack = new Stack<Cell>();

            var currentSell = new Cell(_labyrinthSizeX / 2, _labyrinthSizeZ / 2, _parent.position, true);
            var isGridSet = false;

            do
            {
                var rowX = currentSell.RowX;
                var columnZ = currentSell.ColumnZ;
                var position = currentSell.Position;

                List<Cell> cellsForNextStep = new List<Cell>();

                // Check the options in four directions around current cell for the next step
                // right
                if (CheckCell(rowX + 1, columnZ) && CheckPlaceAroundCell(rowX + 1, columnZ, Direction.Right))
                    cellsForNextStep.Add(new Cell(rowX + 1, columnZ, PositionSetter.Set(position, _cellGridSizeX, _cellGridSizeZ, Direction.Right)));

                // left
                if (CheckCell(rowX - 1, columnZ) && CheckPlaceAroundCell(rowX - 1, columnZ, Direction.Left))
                    cellsForNextStep.Add(new Cell(rowX - 1, columnZ, PositionSetter.Set(position, _cellGridSizeX, _cellGridSizeZ, Direction.Left)));

                // up
                if (CheckCell(rowX, columnZ + 1) && CheckPlaceAroundCell(rowX, columnZ + 1, Direction.Up))
                    cellsForNextStep.Add(new Cell(rowX, columnZ + 1, PositionSetter.Set(position, _cellGridSizeX, _cellGridSizeZ, Direction.Up)));

                // down
                if (CheckCell(rowX, columnZ - 1) && CheckPlaceAroundCell(rowX, columnZ - 1, Direction.Down))
                    cellsForNextStep.Add(new Cell(rowX, columnZ - 1, PositionSetter.Set(position, _cellGridSizeX, _cellGridSizeZ, Direction.Down)));

                // Get random next step
                if (cellsForNextStep.Count > 0)
                {
                    Cell newCell = cellsForNextStep[Random.Range(0, cellsForNextStep.Count)];

                    _cells[newCell.RowX, newCell.ColumnZ] = newCell;
                    _cells[newCell.RowX, newCell.ColumnZ].IsBusy = true;
                    cellsStack.Push(_cells[newCell.RowX, newCell.ColumnZ]);
                    currentSell = _cells[newCell.RowX, newCell.ColumnZ];

                    var cellObj = Instantiate(_cellPrefub, newCell.Position, Quaternion.identity) as GameObject;
                    // var cellObj = PrefabUtility.InstantiatePrefab(_cellPrefub, SceneManager.GetActiveScene()) as GameObject;
                    cellObj.transform.SetPositionAndRotation(newCell.Position, Quaternion.identity);
                    cellObj.transform.Rotate(Vector3.up, Random.Range(0, 4)*90f);                    
                    cellObj.transform.SetParent(roadParrent.transform);

                    if (!isGridSet)
                        isGridSet = SetGridStepsSize(cellObj); 
                }
                else
                {
                    currentSell = cellsStack.Pop();
                }
            }
            while (cellsStack.Count > 0);

            if (_wallPrefub != null)
                FillWalls();
        }

        /// <summary>
        /// Generates walls in the complete labyrinth
        /// </summary>
        private void FillWalls()
        {
            var wallsParrent = new GameObject { name = PARENT_WALLS };
            wallsParrent.transform.SetParent(_parent);

            for (var x = 0; x < _cells.GetUpperBound(0); x++)
            {
                for (var z = 0; z < _cells.GetUpperBound(1); z++)
                {
                    if (CheckCellInArray(x, z) && !_cells[x, z].IsBusy)
                    {
                        var position = SetNewPositionForWall(x, z);
                        var wall = Instantiate(_wallPrefub, position, Quaternion.identity);
                        // var wall = PrefabUtility.InstantiatePrefab (_wallPrefub, SceneManager.GetActiveScene()) as GameObject;
                        wall.transform.SetPositionAndRotation(position, Quaternion.identity);
                        wall.transform.Rotate(Vector3.up, Random.Range(0, 4)*90f);
                        wall.transform.SetParent(wallsParrent.transform);

                        _cells[x, z].Position = position;
                        _cells[x, z].IsBusy = true;
                    }
                }
            }
        }

        /// <summary>
        /// Finding busy cell in 8 cells around current cell and return new position
        /// </summary>
        private Vector3 SetNewPositionForWall(int rowX, int columnZ)
        {
            for (var x = -1; x <= 1; x++)
            {
                for (var z = -1; z <= 1; z++)
                {
                    if (CheckCellInArray(rowX + x, columnZ + z) && _cells[rowX + x, columnZ + z].IsBusy)
                    {
                        var position = _cells[rowX + x, columnZ + z].Position;
                        Direction directionX = Direction.None;
                        Direction directionZ = Direction.None;

                        if (z == 1) directionZ = Direction.Down;
                        if (z == -1) directionZ = Direction.Up;
                        if (x == 1) directionX = Direction.Left;
                        if (x == -1) directionX = Direction.Right;

                        position = PositionSetter.Set(position, _cellGridSizeX, _cellGridSizeZ, directionX, directionZ);
                        return position;
                    }
                }
            }
            Debug.Log("defult x= " + rowX + " z= " + columnZ);
            return default;
        }

        /// <summary>
        /// Calculate labyrinth grid size for two dimensions
        /// </summary>
        private bool SetGridStepsSize(GameObject gameObj)
        {
            var boxCollider = gameObj.GetComponent<BoxCollider>();
            if (boxCollider == null)
                boxCollider = gameObj.AddComponent<BoxCollider>();
            _cellGridSizeX = boxCollider.bounds.size.x;
            _cellGridSizeZ = boxCollider.bounds.size.z;
            return true;
        }
        

        /// <summary>
        /// Checks ability for next step labyrinth road path
        /// </summary>
        private bool CheckPlaceAroundCell(int rowX, int columnZ, Direction value)
        {
            switch (value)
            {
                case Direction.Up:
                    for (var x = -1; x <= 1; x++)
                    {
                        for (var z = 0; z <= 1; z++)
                        {
                            if (!CheckCell(rowX + x, columnZ + z))
                                return false;
                        }
                    }
                    return true;

                case Direction.Down:
                    for (var x = -1; x <= 1; x++)
                    {
                        for (var z = -1; z <= 0; z++)
                        {
                            if (!CheckCell(rowX + x, columnZ + z))
                                return false;
                        }
                    }
                    return true;

                case Direction.Left:
                    for (var x = -1; x <= 0; x++)
                    {
                        for (var z = -1; z <= 1; z++)
                        {
                            if (!CheckCell(rowX + x, columnZ + z))
                                return false;
                        }
                    }
                    return true;

                case Direction.Right:
                    for (var x = 0; x <= 1; x++)
                    {
                        for (var z = -1; z <= 1; z++)
                        {
                            if (!CheckCell(rowX + x, columnZ + z))
                                return false;
                        }
                    }
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks busy status of the cell
        /// </summary>
        private bool CheckCell(int rowX, int columnZ)
        {
            if (CheckCellInArray(rowX, columnZ))
                return !_cells[rowX, columnZ].IsBusy;
            else
                return false;
        }

        /// <summary>
        /// Check includes or not incoming coordinates in the array
        /// </summary>
        private bool CheckCellInArray(int rowX, int columnZ)
        {
            return rowX >= 0 && rowX < _cells.GetUpperBound(0) && columnZ >= 0 && columnZ < _cells.GetUpperBound(1);
        }

        #endregion


    }
}
