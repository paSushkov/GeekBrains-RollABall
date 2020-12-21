﻿using System.Collections.Generic;
using LabyrinthGame.Camera;
using LabyrinthGame.Collectibles;
using LabyrinthGame.Common.Interfaces;
using LabyrinthGame.Effects;
using LabyrinthGame.LevelGenerator;
using LabyrinthGame.Player;
using LabyrinthGame.Stats;
using LabyrinthGame.Tech.Input;
using LabyrinthGame.Tech.PlayerLoop;
using UnityEngine;

namespace LabyrinthGame.Managers
{
    [CreateAssetMenu(menuName = "Sushkov/Managers/LinksManager")]
    public class LinksManager : ScriptableObject
    {
        #region Private data

        [SerializeField] private RotationManager rotationManager = null;
        [SerializeField] private InputManagerTranslator inputManagerTranslator = null;
        [SerializeField] private InputManagerListener inputManagerListener = null;
        [SerializeField] private UserInputManager userInputManager = null;
        [SerializeField] private LabyrinthElementsHolder elementsHolder = null;
        [SerializeField] private EffectsSetLibrary effectsSetLibrary;
        [SerializeField] private StatsAsset statsAsset;
        private Dictionary<IHaveTransform, Transform> _transformRegistry = new Dictionary<IHaveTransform, Transform>();
        private Labyrinth _labyrinth;
        private ICollectibleFactory _collectibleFactory;
        private EffectFactory _effectFactory;
        
        #endregion
        
        
        #region Tech links

        public IPlayerLoopProcessor PlayerLoopProcessor { get; private set; }
        public IInputTranslator InputTranslator => inputManagerTranslator;
        public IInputListener InputListener => inputManagerListener;
        public IUserInputManager UserInputManager => userInputManager;
        public IlabirinthElementsHolder LevelElementsHolder => elementsHolder;
        public EffectsSetLibrary EffectsSetLibrary => effectsSetLibrary;
        public StatsAsset CharactersStatsAsset => statsAsset;

        
        #endregion


        #region Game-logic essencial links

        public IRotator RotationManager => rotationManager;
        public IPlayer ActivePlayer { get; set; }
        public ICameraController CameraController { get; set; }
        public Labyrinth Labyrinth => _labyrinth;
        public EffectFactory GameEffectFactory => _effectFactory;
        public ICollectibleFactory GameCollectibleFactory=> _collectibleFactory;


        #endregion


        #region Public methods

        public void Initialize(IPlayerLoopProcessor playerLoopProcessor, Labyrinth labyrinth)
        {
            PlayerLoopProcessor = playerLoopProcessor;
            _labyrinth = labyrinth; 
            _effectFactory = new EffectFactory();
            _collectibleFactory = new CollectibleFactory();
            RotationManager?.Initialize(playerLoopProcessor);
            InputTranslator?.Initialize(playerLoopProcessor);
            InputListener?.Initialize(InputTranslator);
        }

        public void Shutdown()
        {
            ActivePlayer?.Shutdown();
            ActivePlayer = null;
            CameraController?.Shutdown();
            CameraController = null;
            _collectibleFactory = null;

            RotationManager?.Shutdown();
            InputTranslator?.Shutdown();
            InputListener?.Shutdown(inputManagerTranslator);
            UserInputManager?.Shutdown();
        }

        public bool TryGetTransformOwner(Transform transform, out IHaveTransform owner)
        {
            foreach (var value in _transformRegistry)
            {
                if (value.Value != transform) continue;
                owner = value.Key;
                return true;
            }

            owner = null;
            return false;
        }

        public void RegisterTransform(IHaveTransform owner, Transform property)
        {
            _transformRegistry.Add(owner, property);
        }

        public void DismissTransform(IHaveTransform owner)
        {
            if (_transformRegistry.ContainsKey(owner))
            {
                var transform = _transformRegistry[owner];
                if (transform)
                {
                    Destroy(transform.gameObject);
                }

                    
                _transformRegistry.Remove(owner);
            }

        }

        #endregion
    }
}