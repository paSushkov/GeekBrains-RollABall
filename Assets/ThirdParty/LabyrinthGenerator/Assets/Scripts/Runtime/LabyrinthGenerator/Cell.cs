using UnityEngine;


namespace LabyrinthGenerator
{
    public struct Cell
    {


        #region Fields

        public Vector3 Position;
        public bool IsBusy;
        public int RowX;
        public int ColumnZ;

        #endregion


        #region PrivateData

        public Cell(int _rowX, int _columnZ, Vector3 _position = default, bool _isBusy = false)
        {
            RowX = _rowX;
            ColumnZ = _columnZ;
            Position = _position;
            IsBusy = _isBusy;
        }

        #endregion


    }
}