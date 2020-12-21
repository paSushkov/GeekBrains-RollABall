using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LabyrinthGame.Tech
{
    public static class Helper
    {
        public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag)where T:Component{
            Transform t = parent.transform;
            foreach(Transform tr in t)
            {
                if(tr.tag == tag)
                {
                    return tr.GetComponent<T>();
                }
            }
            return null;
        }
        
        public static T[] GetComponentsInChildrenWithTag<T>(this GameObject gameObject, string tag)
            where T: Component
        {
            List<T> results = new List<T>();
 
            if(gameObject.CompareTag(tag))
                results.Add(gameObject.GetComponent<T>());
 
            foreach(Transform t in gameObject.transform)
                results.AddRange(t.gameObject.GetComponentsInChildrenWithTag<T>(tag));
 
            return results.ToArray();
        }
        
        public static  List<T> GetAllEnumValues<T>() where T : Enum
        {
            var enumList = Enum.GetValues(typeof(T)).Cast<T>().ToList();
            return enumList;
        }
    }
}