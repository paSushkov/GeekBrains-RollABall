using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using LabyrinthGame.Camera;
using LabyrinthGame.Collectibles;
using LabyrinthGame.Common;
using LabyrinthGame.LevelGenerator;
using LabyrinthGame.Player;
using LabyrinthGame.SerializebleData;
using LabyrinthGame.Tech;
using LabyrinthGame.Tech.Input;
using LabyrinthGame.Tech.PlayerLoop;
using Sushkov.SingletonScriptableObject;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LabyrinthGame.Managers
{
    [CreateAssetMenu(menuName = "Sushkov/SingletonScriptableObject/MasterManager")]
    public class MasterManager : SingletonScriptableObject<MasterManager>
    {
        [SerializeField] private LinksManager linksHolder = null;
        [SerializeField] private int mandatoryScore = 0;
        private LabyrinthGenerator _levelGenerator = null;
        private List<GameObject> instantiatedObjects = new List<GameObject>();

        // TODO: move all loaded resources to separate container. It gets messy
        private GameObject[] _playerModels;
        private GameObject _cameraPrefab;
        private GameObject _coinPrefab;
        private Material _silverMat;
        private Material _goldMat;
        private SaveDataRepository _repository;
        private ObservableCollection<CollectibleBase> collectibles;
        private bool isShuttingDown;


        #region Properties

        public LinksManager LinksHolder => linksHolder;

        public int MandatoryScore
        {
            get => mandatoryScore;
            private set
            {
                mandatoryScore = value;
            }
        }

        #endregion


        #region Static methods

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private void FirstInitialize()
        {
            Debug.Log("Master manger initialized");
        }

        #endregion


        #region Public methods

        public void Initialize(IPlayerLoopProcessor playerLoopProcessor, Transform levelRoot = null)
        {
            isShuttingDown = false;
            MandatoryScore = 0;
            
            _repository = new SaveDataRepository();

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
            LinksHolder.ActivePlayer.Initialize(player.transform, playerLoopProcessor,
                LinksHolder.StatsLibrary.DefaultPlayerStats, LinksHolder.StatsLibrary.DefaultJumpPower);
            LinksHolder.CameraController.Initialize(playerLoopProcessor, camera.transform);
            LinksHolder.CameraController.StartTracking(LinksHolder.ActivePlayer);
            LinksHolder.UserInputManager.GetGameActiveController().Start();

            collectibles = GenerateCollectibleItems(50, 50);
        }

        public void Shutdown()
        {
            isShuttingDown = true;
            ShutdownCollectibles();
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

            var uiCanvasPrefab = Resources.Load("Prefabs/UI/HUDCanvas", typeof(GameObject)) as GameObject;
            var statsHudPrefab = Resources.Load("Prefabs/UI/StatsHUD", typeof(GameObject)) as GameObject;
            var statBarPrefab = Resources.Load("Prefabs/UI/PlayerStatBar", typeof(GameObject)) as GameObject;
            var winScreenPrefab = Resources.Load("Prefabs/UI/WinMessage", typeof(GameObject)) as GameObject;
            var effectHolderPrefab = Resources.Load("Prefabs/UI/EffectIconsHolder", typeof(GameObject)) as GameObject;
            var effectIconPrefab = Resources.Load("Prefabs/UI/EffectIconView", typeof(GameObject)) as GameObject;
            var restartButton = Resources.Load("Prefabs/UI/RestartButton", typeof(GameObject)) as GameObject;


            InstantiateUI();

            void InstantiateUI()
            {
                var canvas = InstantiateObject(uiCanvasPrefab);
                var statBarHud = InstantiateObject(statsHudPrefab, canvas.transform);
                var winScreen = InstantiateObject(winScreenPrefab, canvas.transform);
                var effectHolder = InstantiateObject(effectHolderPrefab, canvas.transform);
                LinksHolder.InitializeUILinks(winScreen, effectIconPrefab, effectHolder, statBarPrefab, statBarHud);
                var restBtn = InstantiateObject(restartButton, canvas.transform);
                if (restBtn.TryGetComponent<Button>(out var restButton))
                {
                    restButton.onClick.AddListener(ReloadScene);
                }
            }
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

        public GameObject InstantiateObject(GameObject obj = null, Transform parent = null)
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

        private void ShutdownCollectibles()
        {
            if (collectibles is null)
                return;
            collectibles.CollectionChanged -= Collectibles_CollectionChanged;
            for (var i = 0; i < collectibles.Count; )
            {
                collectibles[i].Shutdown();
                if (collectibles[i].IsMandatory)
                    MandatoryScore--;
                collectibles.RemoveAt(i);
            }
            

            if (MandatoryScore != 0)
            {
                Debug.LogError($"Collectibles were shut down, but mandatoryScore is: {MandatoryScore}");
                MandatoryScore = 0;
            }
            
        }

        private void RebootCollectibles(CollectibleData[] data)
        {
            ShutdownCollectibles();
            if (collectibles == null)
                collectibles = new ObservableCollection<CollectibleBase>();
            
            var layerMask = new LayerMask();
            layerMask |= (1 << LinksHolder.ActivePlayer.GameTransform.gameObject.layer);

            var root = LinksHolder.Labyrinth.collectiblesRoot;
            if (!root)
            {
                root = InstantiateObject().transform;
                root.transform.SetParent(LinksHolder.Labyrinth.Root);
                LinksHolder.Labyrinth.collectiblesRoot = root.transform;
                root.name = "Collectibles";
            }

            var collectiblesModels = LinksHolder.GameCollectibleFactory.UnpackCollectibles(data);
            
            for (var i = 0; i < data.Length; i++)
            {
                var view = InstantiateObject(_coinPrefab);
                Collider collider;
                TriggerListener listener;

                if (!view.TryGetComponent<Collider>(out collider))
                    view.AddComponent<SphereCollider>();
                if (!view.TryGetComponent<TriggerListener>(out listener))
                    listener = view.AddComponent<TriggerListener>();
                
                view.transform.position = data[i].position;
                view.transform.SetParent(root.transform);
                
                var meshRenderer = Helper.FindComponentInChildWithTag<MeshRenderer>(view, "Body");
                var banners = Helper.GetComponentsInChildrenWithTag<Image>(view, "Banner");

                if (data[i].effectData.isFake)
                {
                    if (meshRenderer)
                        meshRenderer.material = _goldMat;
                    MandatoryScore++;

                    foreach (var banner in banners)
                        banner.enabled = false;

                }
                else
                {
                    if (meshRenderer)
                        meshRenderer.material = _silverMat;

                    if (collectiblesModels[i] is StatChangingCollectible effected)
                    {
                        foreach (var banner in banners)
                            banner.sprite = effected.Effect.EffectIcon;
                    }
                }
                collectibles.Add(collectiblesModels[i]);
                collectiblesModels[i].Initialize(data[i].effectData.isFake, view.transform, listener, layerMask);
                collectiblesModels[i].StartRotating(LinksHolder.RotationManager);
            }
            collectibles.CollectionChanged += Collectibles_CollectionChanged;
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

        private ObservableCollection<CollectibleBase> GenerateCollectibleItems(int commonProbability, int effectProbability)
        {
            var result = new ObservableCollection<CollectibleBase>();

            commonProbability = Mathf.Clamp(commonProbability, 0, 100);
            effectProbability = Mathf.Clamp(effectProbability, 0, 100);

            var cells = LinksHolder.Labyrinth.CellsPositions;
            var collectibleFactory = LinksHolder.GameCollectibleFactory;
            var effectFactory = LinksHolder.GameEffectFactory;
            var layerMask = new LayerMask();
            layerMask |= (1 << LinksHolder.ActivePlayer.GameTransform.gameObject.layer);

            var root = InstantiateObject();
            root.name = "Collectibles";
            root.transform.SetParent(LinksHolder.Labyrinth.Root);
            LinksHolder.Labyrinth.collectiblesRoot = root.transform;

            for (var i = 1; i < cells.Length; i++)
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
                            meshRenderer.material = _goldMat;

                        foreach (var banner in banners)
                            banner.enabled = false;

                        collectible.Initialize(true, view.transform, listener, layerMask);
                        MandatoryScore++;
                    }

                    collectible.StartRotating(LinksHolder.RotationManager);
                    result.Add(collectible);
                }
            }

            result.CollectionChanged += Collectibles_CollectionChanged;
            return result;
        }

        public void Collected(CollectibleBase collectible)
        {
            if (collectibles.Contains(collectible))
                collectibles.Remove(collectible);
        }

        private void ReloadScene()
        {
            Time.timeScale = 0.0f;
            Shutdown();
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void SaveLevel()
        {
            Debug.Log("Saving");
            
            var data = new SaveData();

            if (LinksHolder.ActivePlayer?.GameTransform)
                data.playerPosition = LinksHolder.ActivePlayer.GameTransform.position;

            if (collectibles != null)
            {
                data.collectibles = new CollectibleData[collectibles.Count];
                for (var i = 0; i < data.collectibles.Length; i++)
                {
                    data.collectibles[i] = collectibles[i];
                }
            }
            else
            {
                data.collectibles = new CollectibleData[0];
            }

            _repository.Save(data);
        }

        public void LoadSave()
        {
            Time.timeScale = 0f;
            if (TryLoadData(out var data))
            {
                RebootCollectibles(data.collectibles);
                if (LinksHolder.ActivePlayer?.GameTransform)
                    LinksHolder.ActivePlayer.GameTransform.position = data.playerPosition;
            }
            Time.timeScale = 1f;
        }

        private bool TryLoadData(out SaveData data)
        {
            Debug.Log("Loading");
            if (_repository.TryLoad(out var loadedData))
            {
                data = loadedData;
                return true;
            }

            data = null;
            return false;
        }

        private void Collectibles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Remove:
                        foreach (var item in e.OldItems)
                        {
                            if (item is CollectibleBase collectible && collectible.IsMandatory)
                                MandatoryScore--;
                        }
                        CheckWin();
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        foreach (var item in e.OldItems)
                        {
                            if (item is CollectibleBase collectible && collectible.IsMandatory)
                                MandatoryScore--;
                        }
                        MandatoryScore = 0;
                        break;
                }
            }
        }

        private void CheckWin()
        {
            if (MandatoryScore == 0)
            {
                Time.timeScale = 0f;
                LinksHolder.WinWindow.SetActive(true);
            }
        }

        #endregion
    }
}