using LabyrinthGame.Common.Interfaces;
using LabyrinthGame.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace LabyrinthGame.GameRadar
{
    public sealed class RadarObject : IHaveTransform
    {
        public void Initialize(Transform transform, Sprite icon)
        {
            GameTransform = transform;
            RegisterAsTransformOwner();
            if (!transform.TryGetComponent(out Image image))
                image = transform.gameObject.AddComponent<Image>();
            image.sprite = icon;
        }

        public void Shutdown()
        {
            DisposeTransform();
        }


        #region IHaveTransform
        public Transform GameTransform { get; private set; }
        
        public void RegisterAsTransformOwner()
        {
            MasterManager.Instance.LinksHolder.RegisterTransform(this, GameTransform);
        }

        public void DisposeTransform()
        {
            MasterManager.Instance.LinksHolder.DismissTransform(this);
        }
        
        #endregion

    }
}