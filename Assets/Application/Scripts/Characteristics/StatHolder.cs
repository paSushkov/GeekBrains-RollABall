using System;
using System.Collections.Generic;
using LabirinthGame.Common;
using UnityEngine;

namespace LabirinthGame.Stats
{
    [Serializable]
    public class StatHolder : IUpdateProcessor
    {
        #region PrivateData

        // Once upon a time i will write for myself a serializeble Dictionary
        [SerializeField] private List<RegenerativeCharacterResource> resources = new List<RegenerativeCharacterResource>();
        [SerializeField] private List<Characteristic> characteristics = new List<Characteristic>();

        #endregion


        #region Properties

        public List<RegenerativeCharacterResource> Resources => resources;
        public List<Characteristic> Characteristics => characteristics;

        #endregion
        

        #region Methods

        public void Initialize()
        {
            foreach (var resource in resources)
                resource.Initialize();

            foreach (var characteristic in characteristics)
                characteristic.Initialize();
        }

        public void ProcessUpdate(float deltaTile)
        {
            foreach (var resource in resources)
            {
                resource.Regenerate(deltaTile);
            }
        }
        
        public bool TryGetResource(CharacterResourceType type, out CharacterResource resource)
        {
            if (resources != null)
            {
                foreach (var _resource in resources)
                {
                    if (_resource.Type != type) continue;
                    resource = _resource;
                    return true;
                }
            }
            resource = null;
            return false;
        }
        
        public bool TryGetCharacteristic(CharacteristicType type, out Characteristic characteristic)
        {
            if (characteristics != null)
            {
                foreach (var _characteristic in characteristics)
                {
                    if (_characteristic.Type != type) continue;
                    characteristic = _characteristic;
                    return true;
                }
            }
            characteristic = null;
            return false;
        }

        #endregion
    }
}