    using LabirinthGame.Managers;
using UnityEngine;

namespace LabirinthGame.Common.Interfaces
{
    public interface IFlickerableMaterialSender
    {
        Material FlickeringMaterial { get; }

        void Register(MasterFlicker master);
    }
}