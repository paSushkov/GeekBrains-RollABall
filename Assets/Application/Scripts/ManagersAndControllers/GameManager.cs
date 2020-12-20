// using System;
// using System.Collections.Generic;
// using LabirinthGame.Camera;
// using LabirinthGame.Collectibles;
// using LabirinthGame.Common;
// using LabirinthGame.Player;
// using UnityEngine;
//
// namespace LabirinthGame.Managers
// {
//     public class GameManager : Singleton<GameManager>
//     {
//         #region Private data
//
//         [SerializeField] private PlayerBase _player = null;
//         [SerializeField] private CameraController _mainCameraController = null;
//         private List<CollectibleBase> _mandatoryCollectibles = new List<CollectibleBase>();
//         [SerializeField] private GameObject winMsg;
//
//         private ObjectPool<CollectibleBase> collectiblesPool = new ObjectPool<CollectibleBase>();
//         private ObjectPool<Transform> collectibleModelsPool = new ObjectPool<Transform>();
//         
//         
//         #endregion
//
//
//         #region Unity events
//
//         private void Start()
//         {
//             winMsg.SetActive(false);
//             try
//             {
//             }
//             catch (NullReferenceException)
//             {
//                 if (_mainCameraController is null)
//                 {
//                     if (UnityEngine.Camera.main == null)
//                     {
//                         var cameraObj = new GameObject();
//                         cameraObj.AddComponent<UnityEngine.Camera>();
//                         cameraObj.AddComponent<AudioListener>();
//                         //cameraObj.AddComponent<CameraController>();
//                         cameraObj.tag = "MainCamera";
//                     }
//
//                     UnityEngine.Camera.main.TryGetComponent(out _mainCameraController);
//                 }
//
//                 if (_player == null)
//                 {
//                 }
//                 if (_mainCameraController != null && _player !=null)
//                     Start();
//                 else
//                 {
//                     Debug.LogError("Cant find player or camera!", gameObject);
//                 }
//             }
//         }
//
//         #endregion
//
//
//         #region Public methods
//
//         public void RegisterMandatoryCollectible(CollectibleBase collectible)
//         {
//             if (!_mandatoryCollectibles.Contains(collectible))
//                 _mandatoryCollectibles.Add(collectible);
//         }
//
//         public void UnregisterMandatoryCollectible(CollectibleBase collectible)
//         {
//             if (_mandatoryCollectibles.Contains(collectible))
//                 _mandatoryCollectibles.Remove(collectible);
//
//             if (_mandatoryCollectibles.Count == 0)
//                 winMsg.SetActive(true);
//         }
//
//         #endregion
//     }
// }