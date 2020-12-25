// using System.IO;
// using UnityEngine;
//
// namespace LabyrinthGame.SerializebleData
// {
//     public class JsonArrayWrapper<T> : IData<T>  
//     {
//         [System.Serializable]
//         private class Wrapper<T>
//         {
//             public T[] array;
//         }
//
//         public void Save(T[] data, string path)
//         {
//             var wrapper = new Wrapper<T> {array = data};
//             var str = JsonUtility.ToJson(wrapper);
//             File.WriteAllText(path, str);
//         }
//
//         public T[] Load(string path)
//         {
//             var json = File.ReadAllText(path);
//             //string newJson = "{ \"array\": " + json + "}";
//             Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
//             return wrapper.array;
//         }
//     }
// }