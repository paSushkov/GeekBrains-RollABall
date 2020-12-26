using System.Collections.Generic;
using LabyrinthGame.Common.Interfaces;
using LabyrinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabyrinthGame.Managers
{
    [CreateAssetMenu(menuName = "Sushkov/Managers/RotationManager")]
    public class RotationManager : ScriptableObject, IRotator
    {
        #region Private data

        private List<IRotatable> _rotateblesAtUpdate;
        private List<IRotatable> _rotatablesAtFixedUpdate;
        private bool _isSubscribedForUpdates;

        #endregion


        #region Public methods

        
        #region IPlayerLoop implementation

        public IPlayerLoopSubscriptionController PlayerLoopSubscriptionController { get;} = new PlayerLoopSubscriptionController();

        public void ProcessUpdate(float deltaTime)
        {
            Rotate(_rotateblesAtUpdate, deltaTime);
        }
        public void ProcessFixedUpdate(float fixedDeltaTime)
        {
            Rotate(_rotatablesAtFixedUpdate, fixedDeltaTime);
        }

        public void ProcessLateUpdate(float fixedDeltaTime)
        {
        }

        #endregion


        #region IRotator implementation
        
        public void Initialize(IPlayerLoopProcessor playerLoopProcessor)
        {
            _rotateblesAtUpdate = new List<IRotatable>();
            _rotatablesAtFixedUpdate = new List<IRotatable>();
            PlayerLoopSubscriptionController.Initialize(this, playerLoopProcessor);
            PlayerLoopSubscriptionController.SubscribeToLoop();
        }
        
        #endregion

        public void RotateInUpdate(IRotatable rotatable)
        {
            Register(rotatable, _rotateblesAtUpdate);
        }

        public void StopRotateInUpdate(IRotatable rotatable)
        {
            Unregister(rotatable, _rotatablesAtFixedUpdate);
        }

        public void RotateInFixedUpdate(IRotatable rotatable)
        {
            Register(rotatable, _rotatablesAtFixedUpdate);
        }

        public void StopRotateInFixedUpdate(IRotatable rotatable)
        {
            Unregister(rotatable, _rotatablesAtFixedUpdate);
        }
        
        public void Shutdown()
        {
            PlayerLoopSubscriptionController.Shutdown();
            AskAllToUnsubscribe(_rotateblesAtUpdate);
        }

        #endregion


        #region Private methods

        private void Register(IRotatable obj, ICollection<IRotatable> list)
        {
            if (obj != null && !list.Contains(obj))
                list.Add(obj);
        }

        private void Unregister(IRotatable obj, IList<IRotatable> list)
        {
            if (list == null)
                return;

            var index = list.IndexOf(obj);
            if (index > -1)
                list.RemoveAt(index);
        }

        private void Rotate(IList<IRotatable> list, float deltaTime)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var rotatable = list[i];
                if (rotatable == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                if (!rotatable.RotationalTransform)
                {
                    rotatable.StopRotating();
                    if (list.Contains(rotatable))
                        list.RemoveAt(i);
                    continue;
                }

                var rotation = rotatable.RotationalTransform.rotation;
                var speed = rotatable.RotationSpeed;
                rotation *= Quaternion.AngleAxis(speed.x * deltaTime, Vector3.right);
                rotation *= Quaternion.AngleAxis(speed.y * deltaTime, Vector3.up);
                rotation *= Quaternion.AngleAxis(speed.z * deltaTime, Vector3.forward);

                rotatable.RotationalTransform.rotation = rotation;
            }
        }

        private void AskAllToUnsubscribe(IList<IRotatable> list)
        {
            for (var i = 0; i < list.Count; i++)
            {
                var rotatable = list[i];
                if (rotatable == null)
                {
                    list.RemoveAt(i);
                    continue;
                }

                if (!rotatable.RotationalTransform)
                {
                    rotatable.StopRotating();
                    if (list.Contains(rotatable))
                        list.RemoveAt(i);
                }
            }
        }

        #endregion
    }
}