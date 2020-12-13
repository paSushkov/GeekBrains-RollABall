using System;
using UnityEditor;
using UnityEngine;


namespace LabyrinthGenerator
{
    public class LabyrinthGeneratorWindow : EditorWindow
    {


        #region Fields

        private const string NOTICE = "First needs to fill required fields";
        private const string PARENT = "Labyrinth";

        private GameObject _parent;
        private GameObject _cellPrefub;
        private GameObject _wallPrefub;
        private GUIStyle style = new GUIStyle();
        private int _labyrinthSizeX;
        private int _labyrinthSizeZ;

        #endregion


        #region Window

        [MenuItem("Tools/Labyrinth Generator")]
        public static void ShowWindow()
        {
            GetWindow(typeof(LabyrinthGeneratorWindow), false, "Labyrinth Generator");
        }

        private void OnEnable()
        {
            style.richText = true;
            style.alignment = TextAnchor.MiddleCenter;
        }

        private void OnGUI()
        {
            GUILayout.Space(20);
            GUILayout.Label("Labyrinth size in prefubs (required)", EditorStyles.boldLabel);
            _labyrinthSizeX = EditorGUILayout.IntField("X", _labyrinthSizeX);
            _labyrinthSizeZ = EditorGUILayout.IntField("Z", _labyrinthSizeZ);

            GUILayout.Space(10);
            GUILayout.Label("Road prefub (required)", EditorStyles.boldLabel);
            _cellPrefub = EditorGUILayout.ObjectField(_cellPrefub, typeof(GameObject), true) as GameObject;

            GUILayout.Space(10);
            GUILayout.Label("Wall prefub (optional)", EditorStyles.boldLabel);
            _wallPrefub = EditorGUILayout.ObjectField(_wallPrefub, typeof(GameObject), true) as GameObject;

            GUILayout.Space(10);
            GUILayout.Label("Parent GameObject on Scene (optional)", EditorStyles.boldLabel);
            _parent = EditorGUILayout.ObjectField(_parent, typeof(GameObject), true) as GameObject;

            GUILayout.Space(10);
            var notice = CheckFills() == false ? $"<color=red>{NOTICE}</color>" : String.Empty;
            GUILayout.Label(notice, style);

            GUILayout.Space(10);
            if (GUILayout.Button("Generate") && CheckFills())
                Generate();
        }

        #endregion


        #region Methods

        private void Generate()
		{
            if (_parent == null)
                _parent = new GameObject { name = PARENT };

            LabyrinthGenerator labyrinth = new LabyrinthGenerator(
                _parent.transform,
                _labyrinthSizeX,
                _labyrinthSizeZ,
                _cellPrefub,
                _wallPrefub
                );

            labyrinth.GenerateLabyrinth();
        }

        private bool CheckFills()
        {
            return _cellPrefub != null
                && _labyrinthSizeX > 0
                && _labyrinthSizeZ > 0;
        }

        #endregion


    }
}
