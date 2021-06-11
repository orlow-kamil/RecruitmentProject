using UnityEngine;

namespace Link
{
    [RequireComponent(typeof(DistanceJoint2D))]
    public class Link : MonoBehaviour, ILink
    {
        public Transform Transform => this.transform;
        public DistanceJoint2D Joint => this.joint;

        private DistanceJoint2D joint;

        private void Awake() => this.joint = this.GetComponent<DistanceJoint2D>();

        public void Setup(Rigidbody2D connectRgb, float distance)
        {
            this.joint.connectedBody = connectRgb;
            this.joint.distance = distance;
            this.joint.maxDistanceOnly = true;
        }
    }
}