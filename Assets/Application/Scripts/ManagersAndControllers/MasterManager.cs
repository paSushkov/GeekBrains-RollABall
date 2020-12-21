using System.Collections.Generic;
using System.Linq;
using LabyrinthGame.Camera;
using LabyrinthGame.Collectibles;
using LabyrinthGame.Common;
using LabyrinthGame.LevelGenerator;
using LabyrinthGame.Player;
using LabyrinthGame.Tech;
using LabyrinthGame.Tech.Input;
using LabyrinthGame.Tech.PlayerLoop;
using LabyrinthGame.UI;
using Sushkov.SingletonScriptableObject;
using UnityEngine;
using UnityEngine.UI;

namespace LabyrinthGame.Managers
{
    [CreateAssetMenu(menuName = "Sushkov/SingletonScriptableObject/MasterManager")]
    public class MasterManager : SingletonScriptableObject<MasterManager>
    {
        public int mandatoryScore = 0;

        [SerializeField] private LinksManager linksHolder = null;
        private LabyrinthGenerator _levelGenerator = null;
        private List<GameObject> instantiatedObjects = new List<GameObject>();
        
        // TODO: move all loaded resources to separate container. It gets messy
        private GameObject[] _playerModels;
        private GameObject _cameraPrefab;
        private GameObject _coinPrefab;
        private Material _silverMat;
        private Material _goldMat;
        private GameObject uiCanvas;
        private GameObject statsHud;
        private GameObject statBar;
        private GameObject winScreen;
        private Transform activeCanvas;
        private GameObject activeWinScreen;


        #region Properties

        public LinksManager LinksHolder => linksHolder;

        #endregion


        #region Static methods

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void FirstInitialize()
        {
            Debug.Log("Master manger initialized");
        }

        #endregion


        #region Public methods

        public void Initialize(IPlayerLoopProcessor playerLoopProcessor, Transform canvas, Transform levelRoot = null)
        {
            activeCanvas = canvas;
            mandatoryScore = 0;

            var labyrinth = GenerateLabyrinth(levelRoot);

            LoadResources();
            var camera = InstantiateCamera();
            var player = InstantiateRandomPlayer(labyrinth.CellsPositions[0] + Vector3.up * 2);

            if (LinksHolder)
                LinksHolder.Initialize(playerLoopProcessor, labyrinth);


            LinksHolder.CameraController = new CameraController();
            LinksHolder.UserInputManager.Initialize(new GameProcessInputController(), null);

            // Temp
            LinksHolder.ActivePlayer = new RigidbodyPlayer();
            LinksHolder.ActivePlayer.Initialize(player.transform, playerLoopProcessor, LinksHolder.StatsLibrary.DefaultPlayerStats,LinksHolder.StatsLibrary.DefaultJumpPower);
            LinksHolder.CameraController.Initialize(playerLoopProcessor, camera.transform);
            LinksHolder.CameraController.StartTracking(LinksHolder.ActivePlayer);
            LinksHolder.UserInputManager.GetGameActiveController().Start();

            GenerateCollectibleItems(50, 50);
            InstantiateUI();
        }

        public void Shutdown()
        {
            if (linksHolder)
                linksHolder.Shutdown();
            CleanUp();
        }

        #endregion


        #region Private methods

        private void LoadResources()
        {
            // TODO: in a separate container make editor-exposed fields to determine path for each essencial resource
            _cameraPrefab = Resources.Load("Prefabs/MainCamera", typeof(GameObject)) as GameObject;
            _playerModels = Resources.LoadAll("Prefabs/PlayerModels").Cast<GameObject>().ToArray();
            _coinPrefab = Resources.Load("Prefabs/Collectibles/Coins/Coin", typeof(GameObject)) as GameObject;
            _silverMat = Resources.Load("Materials/CoinsMaterials/silver_coin_color", typeof(Material)) as Material;
            _goldMat = Resources.Load("Materials/CoinsMaterials/gold_coin_color", typeof(Material)) as Material;

            uiCanvas = Resources.Load("Prefabs/UI/HUDCanvas", typeof(GameObject)) as GameObject;
            statsHud = Resources.Load("Prefabs/UI/StatsHUD", typeof(GameObject)) as GameObject;
            statBar = Resources.Load("Prefabs/UI/PlayerStatBar", typeof(GameObject)) as GameObject;
            winScreen = Resources.Load("Prefabs/UI/WinMessage", typeof(GameObject)) as GameObject;
        }

        private GameObject InstantiateCamera()
        {
            var camera = Instantiate(_cameraPrefab);
            instantiatedObjects.Add(camera);
            return camera;
        }

        private GameObject InstantiateRandomPlayer(Vector3 position)
        {
            var index = Random.Range(0, _playerModels.Length);
            var player = Instantiate(_playerModels[index]);
            player.transform.position = position;
            instantiatedObjects.Add(player);
            return player;
        }

        private GameObject InstantiateObject(GameObject obj = null, Transform parent = null)
        {
            var newObject = obj != null ? Instantiate(obj, parent) : Instantiate(new GameObject(), parent);
            instantiatedObjects.Add(newObject);
            return newObject;
        }

        private void CleanUp()
        {
            for (var i = 0; i < instantiatedObjects.Count; i++)
            {
                if (instantiatedObjects[i])
                    Destroy(instantiatedObjects[i]);
            }
        }

        private Labyrinth GenerateLabyrinth(Transform levelRoot, int xSize = -1, int zSize = -1)
        {
            if (!levelRoot)
            {
                var levelRootGO = new GameObject {name = "Labyrinth root"};
                instantiatedObjects.Add(levelRootGO);
                levelRoot = levelRootGO.transform;
            }

            if (_levelGenerator == null)
                _levelGenerator = new LabyrinthGenerator(levelRoot, LinksHolder.LevelElementsHolder.Cells,
                    LinksHolder.LevelElementsHolder.Walls);

            var labyrinthGenerator = new LabyrinthGenerator(levelRoot,
                this.LinksHolder.LevelElementsHolder.Cells, LinksHolder.LevelElementsHolder.Walls);
            var labXsize = xSize < 3 ? Random.Range(10, 30) : xSize;
            var labZsize = zSize < 3 ? Random.Range(10, 30) : zSize;

            return labyrinthGenerator.GenerateLabyrinth(labXsize, labZsize);
        }

        private void GenerateCollectibleItems(int commonProbability, int effectProbability)
        {
            commonProbability = Mathf.Clamp(commonProbability, 0, 100);
            effectProbability = Mathf.Clamp(effectProbability, 0, 100);

            var cells = LinksHolder.Labyrinth.CellsPositions;
            var collectibleFactory = LinksHolder.GameCollectibleFactory;
            var effectFactory = LinksHolder.GameEffectFactory;
            var layerMask = new LayerMask();
            layerMask |= (1 << LinksHolder.ActivePlayer.GameTransform.gameObject.layer);
            //layerMask |= (1 << LayerMask.NameToLayer("Default"));
            
            var root = InstantiateObject();
            root.name = "Collectibles";
            root.transform.SetParent(LinksHolder.Labyrinth.Root);
            
            for (var i = 0; i < cells.Length; i++)
            {
                var chance = Random.Range(0, 101);
                if (chance <= commonProbability)
                {
                    
                    var view = InstantiateObject(_coinPrefab);
                    Collider collider;
                    TriggerListener listener;

                    if (!view.TryGetComponent<Collider>(out collider))
                        collider = view.AddComponent<SphereCollider>();
                    if (!view.TryGetComponent<TriggerListener>(out listener))
                        listener = view.AddComponent<TriggerListener>();
                    
                    
                    view.transform.position = cells[i] + Vector3.up;
                    view.transform.SetParent(root.transform);

                    var meshRenderer = Helper.FindComponentInChildWithTag<MeshRenderer>(view, "Body");
                    var banners = Helper.GetComponentsInChildrenWithTag<Image>(view, "Banner");
                    
                    
                    var effectChance = Random.Range(0, 101);
                    CollectibleBase collectible;
                    if (effectChance <= effectProbability)
                    {
                        var effect =
                            effectFactory.MakeRandomEffect();
                        collectible = collectibleFactory.GetEffectCollectible(effect);
                        if (meshRenderer)
                        {
                            meshRenderer.material = _silverMat;
                        }
                        collectible.Initialize(false, view.transform, listener, layerMask);
                        foreach (var banner in banners)
                        {
                            banner.sprite = effect.EffectIcon;
                        }
                    }
                    else
                    {
                        collectible = collectibleFactory.GetSimpleCollectible();
                        if (meshRenderer)
                        {
                            meshRenderer.material = _goldMat;
                        }
                        foreach (var banner in banners)
                        {
                            banner.enabled = false;
                        }
                        collectible.Initialize(true, view.transform, listener, layerMask);
                    }
                    collectible.StartRotating(LinksHolder.RotationManager);
                    
                }
            }
        }

        private void InstantiateUI()
        {
            //var canvas = InstantiateObject(uiCanvas);
            var canvas = activeCanvas;
            var statsHud = InstantiateObject(this.statsHud, canvas);
            var player = LinksHolder.ActivePlayer;
            foreach (var statType in LinksHolder.ActivePlayer.StatHolder.ActiveStats)
            {
                var handlerObj = InstantiateObject(statBar, statsHud.transform);
                
                if (handlerObj.TryGetComponent(out BarHandler handler))
                {
                    handler.Initialize(statType, player);                    
                }
            }
            activeWinScreen = InstantiateObject(winScreen, canvas);
        }

        #endregion
    }
}