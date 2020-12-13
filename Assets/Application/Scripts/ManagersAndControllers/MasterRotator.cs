using System.Collections.Generic;
using LabirinthGame.Common;
using LabirinthGame.Common.Interfaces;
using UnityEngine;


namespace LabirinthGame.Managers
{
    public class MasterRotator : Singleton<MasterRotator> 
    {
        private List<IRotatable> rotatables = new List<IRotatable>();


        #region Unity events

        private void FixedUpdate()
        {
            Rotate();
        }

        #endregion
        
        #region Public methods

        public void  Register(IRotatable rotatable)
        {
            if (rotatables.Contains(rotatable) || rotatable == null)
                return;
            rotatables.Add(rotatable);
        }

        public void Unregister(IRotatable rotatable)
        {
            if (!rotatables.Contains(rotatable) || rotatable == null)
                return;
            rotatables.Remove(rotatable);
        }

        #endregion
        

        #region Private methods

        private void Rotate()
        {
            foreach (var rotatable in rotatables)
            {
                var rotation = rotatable.RotationalTransform.rotation;
                rotation *= Quaternion.AngleAxis(rotatable.RotationSpeed.x*Time.deltaTime, Vector3.right);
                rotation *= Quaternion.AngleAxis(rotatable.RotationSpeed.y*Time.deltaTime, Vector3.up);
                rotation *= Quaternion.AngleAxis(rotatable.RotationSpeed.z*Time.deltaTime, Vector3.forward);

                rotatable.RotationalTransform.rotation = rotation;
            }
        }
        
        #endregion

    }
}