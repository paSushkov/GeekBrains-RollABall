using System.IO;
using UnityEngine;


namespace LabyrinthGame.SerializebleData
{
    public class SaveDataRepository
    {
        private readonly IData<SaveData> _data;
        private const string _folderName = "dataSave";
        private const string _fileName = "effectsData.bat";
        private readonly string _path;

        public SaveDataRepository()
        {
            if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
            {
                _data = new CollectiblePrefsData();
            }
            else
            {
                _data = new JsonData<SaveData>();
            }

            _path = Path.Combine(UnityEngine.Application.dataPath, _folderName);
        }

        public void Save(SaveData data)
        {
            if (!Directory.Exists(Path.Combine(_path)))
            {
                Directory.CreateDirectory(_path);
            }

            
            _data.Save(data, Path.Combine(_path, _fileName));
        }

        public bool TryLoad(out SaveData data)
        {
            var file = Path.Combine(_path, _fileName);
            if (File.Exists(file))
            {
                data = _data.Load(file);
                return true;
            }
                data = null;
                return false;
        }
    }
}