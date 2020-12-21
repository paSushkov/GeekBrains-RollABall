using System.Collections.Generic;
using Application.Scripts.Common.Interfaces;

namespace LabyrinthGame.Managers
{
    public class ObjectPool<T> : IObjectPool<T>
    {
        private List<T> poolList = new List<T>();


        public bool GetObject(out T obj)
        {
            if (poolList.Count > 0)
            {
                obj = poolList[poolList.Count - 1];
                poolList.RemoveAt(poolList.Count - 1);
                return true;
            }

            obj = default;
            return false;
        }

        public void ReturnObject(T obj)
        {
            poolList.Add(obj);
        }
    }
}