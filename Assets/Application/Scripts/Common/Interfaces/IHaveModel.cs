using UnityEngine;

namespace LabyrinthGame.Common.Interfaces
{
    public interface IHaveTransform
    {
        Transform GameTransform { get; }

        void RegisterAsTransformOwner();
        void DisposeTransform();

    }
}