using System.IO;
using UnityEngine;

namespace LabyrinthGame.SerializebleData
{
    public class JsonData<T> : IData<T>
    {
        public void Save(T data, string path = null)
        {

            Debug.Log(data.GetType().Name);
            var str = JsonUtility.ToJson(data);
            File.WriteAllText(path, str);
            //File.WriteAllText(path, Crypto.CryptoXOR(str)); 
        }

        public T Load(string path = null)
        {
            var str = File.ReadAllText(path);
            return JsonUtility.FromJson<T>(str); 
            //return JsonUtility.FromJson<T>(Crypto.CryptoXOR(str)); 
        }
    }
}