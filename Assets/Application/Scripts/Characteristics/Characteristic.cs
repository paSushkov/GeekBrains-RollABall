using System;
using LabirinthGame.Common;

namespace LabirinthGame.Stats
{
    [Serializable]
    public class Characteristic : Stat <CharacteristicType>
    {
        public Characteristic(CharacteristicType type, MinMaxCurrent minMaxCurrent) : base(type,minMaxCurrent)
        {
        }
    }
}