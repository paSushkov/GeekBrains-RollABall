using UnityEngine;

namespace Application.Scripts.Common.Interfaces
{
    public interface IHaveTransform
    {
        Transform GameTransform { get; }
    }
}