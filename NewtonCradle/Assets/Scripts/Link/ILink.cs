using UnityEngine;

namespace Link
{
    public interface ILink
    {
        Transform Transform { get; }

        DistanceJoint2D Joint { get; }

        void Setup(Rigidbody2D connectRgb, float distance);
    }
}