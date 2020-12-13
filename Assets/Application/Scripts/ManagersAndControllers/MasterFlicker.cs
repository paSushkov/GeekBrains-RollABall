using System.Collections.Generic;
using LabirinthGame.Common;
using UnityEngine;


namespace LabirinthGame.Managers
{
    public class MasterFlicker : Singleton<MasterFlicker> 
    {
        private List<Material> materials = new List<Material>();


        #region Unity events

        private void FixedUpdate()
        {
            Flicker();
        }

        #endregion
        
        #region Public methods

        public void  Register(Material material)
        {
            if (materials.Contains(material) || material == null)
                return;
            materials.Add(material);
        }

        public void Unregister(Material material)
        {
            if (materials.Contains(material) || material == null)
                return;
            materials.Remove(material);
        }

        #endregion
        

        #region Private methods

        private void Flicker()
        {
            foreach (var material in materials)
            {
                material.color = new Color(material.color.r, material.color.g, material.color.b, Mathf.PingPong(Time.time, 1.0f));

            }
        }
        
        #endregion

    }
}