using UnityEngine;
using Tower;
using System;

namespace Projectile
{
    public class Projectile : MonoBehaviour, IProjectile
    {
        public event EventHandler<EventArgs> OnTriggerOrDestroy;

        public bool NoCreatingBullet { get => this.noCreatingBullet; set => this.noCreatingBullet = value; }
        public float Speed { get => this.speed; set => this.speed = value; }
        public Vector2 Direction { get => this.direction; set => this.direction = value; }

        [SerializeField] private GameObject tower;
        [SerializeField] private float speed = 4f;
        [SerializeField] private Vector2 durationMinMax = new Vector2(1f, 4f);

        private float duration;
        private Vector3 direction;
        private float currentTime;
        private bool noCreatingBullet = false;

        private void Awake()
        {
            if (this.tower == null)
                throw new NullReferenceException("Projectile not found");
        }

        private void Start()
        {
            this.duration = this.RandomDuration();
            this.OnTriggerOrDestroy += this.Destroy;
            if (!this.noCreatingBullet)
            {
                this.OnTriggerOrDestroy += this.CreateTower;
            }
        }

        private float RandomDuration() => UnityEngine.Random.Range(this.durationMinMax.x, this.durationMinMax.y);

        private void Update()
        {
            if (this.currentTime <= this.duration)
                this.Move();
            else
            {
                this.OnTriggerOrDestroy?.Invoke(this, EventArgs.Empty);
            }
            
            this.currentTime += Time.deltaTime;
        }

        public void Move() => this.transform.position += this.speed * Time.deltaTime * this.direction;

        public void Destroy(object sender, EventArgs args)
        {
            //TODO: Pooling
            MonoBehaviour.Destroy(this.gameObject);
        }

        public void CreateTower(object sender, EventArgs args)
        {
            var towerObj = Instantiate(this.tower, this.transform.position, Quaternion.identity);
            towerObj.transform.SetParent(SceneManager.Instance.TowersParent.transform);
            towerObj.GetComponent<ITower>().CanShoot = false;
            SceneManager.Instance.IncreaseTowerCounter();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.TryGetComponent(out ITower tower))
            {
                this.OnTriggerOrDestroy -= this.CreateTower;
                tower.Destroy();
                this.OnTriggerOrDestroy?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}