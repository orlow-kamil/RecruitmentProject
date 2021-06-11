using UnityEngine;

namespace Ball
{
    public interface IBall : IClickable
    {
        Transform Transform { get; }

        Rigidbody2D Rigidbody { get; }

        DistanceJoint2D Joint { get; }

        bool IsHolding { get; set; }

        void Setup(float mass);
    }
}