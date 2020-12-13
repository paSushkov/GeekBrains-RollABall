using System;
using LabirinthGame.Common;

namespace LabirinthGame.Stats
{
    [Serializable]
    public class CharacterResource : Stat <CharacterResourceType>
    {
        public CharacterResource(CharacterResourceType type, MinMaxCurrent minMaxCurrent) : base(type,minMaxCurrent)
        {
        }
    }
}