using System.Collections.Generic;
using LabyrinthGame.Common.Interfaces;
using LabyrinthGame.Managers;
using LabyrinthGame.Player;
using LabyrinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabyrinthGame.GameRadar
{
    public sealed  class Radar : IHaveTransform, IPlayerLoop
    {
      private IPlayer player; // Позиция главного героя
      private readonly float mapScale = 8;
      private readonly Dictionary<IRadarTrackable, RadarObject> radarObjects = new Dictionary<IRadarTrackable, RadarObject>();
      private GameObject iconPrefab;

      public void Initialize(IPlayer player, Transform view, IPlayerLoopProcessor loopProcessor, GameObject defaultIcon)
      {
         this.player = player;
         iconPrefab = defaultIcon;
         GameTransform = view;
         RegisterAsTransformOwner();
         PlayerLoopSubscriptionController.Initialize(this, loopProcessor);
         PlayerLoopSubscriptionController.SubscribeToLoop();
         iconPrefab = Resources.Load("Prefabs/UI/DefaultRadarIcon", typeof(GameObject)) as GameObject;
      }

      public void Shutdown()
      {
         PlayerLoopSubscriptionController?.Shutdown();
         DisposeTransform();
         player = null;
         foreach (var radarObject in radarObjects.Values)
         {
            radarObject?.Shutdown();
         }
         radarObjects.Clear();
      }

      public void RegisterRadarObject(IRadarTrackable radarTrackable, Sprite icon)
      {
         var image = MasterManager.Instance.InstantiateObject(iconPrefab, GameTransform);
         image.name = "radarObject";
         var radarObject = new RadarObject();
         radarObject.Initialize(image.transform, icon);
         radarObjects.Add(radarTrackable, radarObject);
      }
      
      public void RemoveRadarObject(IRadarTrackable radarTrackable)
      {
         if (radarObjects.ContainsKey(radarTrackable))
         {
            radarObjects[radarTrackable].Shutdown();
            radarObjects.Remove(radarTrackable);
         }
      }
      
      private void DrawRadarDots() // Синхронизирует значки на миникарте с реальными объектами
      {
         if (!player?.GameTransform)
            return;

         var playerPosition = player.GameTransform.position;
         var selfPosition = GameTransform.position;
         var cameraRotation = MasterManager.Instance.LinksHolder.CameraController.GameTransform.eulerAngles.y;
         
         foreach (var radarObjects in radarObjects)
         {
            var ownerPosition = radarObjects.Key.RadarPosition;

            var radarPosition = ownerPosition - playerPosition;
            var distToObject = Vector3.Distance(playerPosition, ownerPosition) * mapScale;
            
            var deltaY = Mathf.Atan2(radarPosition.x, radarPosition.z) * Mathf.Rad2Deg -
                         270 - cameraRotation;

            radarPosition.x = distToObject * Mathf.Cos(deltaY * Mathf.Deg2Rad) * -1;
            radarPosition.z = distToObject * Mathf.Sin(deltaY * Mathf.Deg2Rad);

            radarObjects.Value.GameTransform.position =
               new Vector3(radarPosition.x, radarPosition.z, 0) + GameTransform.position;
         }
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

      
      #region IPlayerLoop

      public IPlayerLoopSubscriptionController PlayerLoopSubscriptionController { get; } = new PlayerLoopSubscriptionController();

      public void ProcessUpdate(float deltaTime)
      {
      }

      public void ProcessFixedUpdate(float fixedDeltaTime)
      {
      }

      public void ProcessLateUpdate(float fixedDeltaTime)
      {
         if (Time.frameCount % 2 == 0)
         {
            DrawRadarDots();
         }
      }
      
      #endregion
    }
}
