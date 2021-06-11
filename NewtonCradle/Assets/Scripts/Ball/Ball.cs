using UnityEngine;

namespace Ball
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour, IBall
    {
        public Transform Transform => transform;
        public Rigidbody2D Rigidbody => this.rgb;
        public DistanceJoint2D Joint => GetComponent<DistanceJoint2D>();
        public bool IsHolding { get => isHolding; set => isHolding = value; }

        private Rigidbody2D rgb;
        private bool isHolding;
        private float offsetX, offsetY;

        private void Awake() => this.rgb = GetComponent<Rigidbody2D>();

        public void Setup(float mass)
        {
            this.rgb.gravityScale = 1f;
            this.rgb.mass = mass;
        }

        private void OnMouseDown()
        {
            if (!isHolding)
            {
                offsetX = GetOffset(Input.mousePosition).x;
                offsetY = GetOffset(Input.mousePosition).y;
                isHolding = true;
            }
        }

        public Vector2 GetOffset(Vector2 mousePos) => GetMousePos(mousePos) - transform.position;

        private Vector3 GetMousePos(Vector2 mousePos) => Camera.main.ScreenToWorldPoint(mousePos);


        private void OnMouseDrag()
        {
            if (isHolding)
            {
                var mousePos = GetMousePos(Input.mousePosition);
                transform.position = new Vector2(mousePos.x - offsetX, mousePos.y - offsetY);
            }
        }

        private void OnMouseUp()
        {
            isHolding = false;
        }
    }
}