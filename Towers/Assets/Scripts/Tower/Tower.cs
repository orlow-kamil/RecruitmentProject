using Projectile;
using System;
using System.Collections;
using UnityEngine;

namespace Tower
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Tower : MonoBehaviour, ITower
    {
        const int MaxAttackCount = 12;

        public event EventHandler<EventArgs> OnShooting;
        public event EventHandler<EventArgs> OnRotating;
        public event EventHandler<EventArgs> OnDestroy;

        public bool CanShoot { get => this.canShoot; set => this.canShoot = value; }
        public SpriteRenderer SpriteRenderer { get => this.spriteRenderer; set => this.spriteRenderer = value; }

        [Header("Projectile")]
        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform gunTransform;

        [Header("Tower")]
        [SerializeField] private bool canShoot = true;
        [SerializeField] private float startTime = 6f;
        [SerializeField] private float rotateTime = 0.5f;
        [SerializeField] private Vector2 angleMinMax = new Vector2(15f, 45f);
        [SerializeField] private Color32 activeTowerColor = new Color32(255, 255, 255, 255);
        [SerializeField] private Color32 exhaustedTowerColor = new Color32(255, 0, 0, 255);

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();

            if (this.projectile == null)
                throw new NullReferenceException("Projectile not found");


            if (this.gunTransform == null || this.gunTransform == this.transform)
                throw new NullReferenceException("GunTransform is missing or wrong setup");

        }

        private IEnumerator Start()
        {
            this.OnShooting = this.Shoot;
            this.OnRotating = this.Rotate;
            this.OnDestroy = this.Destroy;
            yield return this.BattleCombat();
        }

        public void Rotate(object sender, EventArgs args) => transform.Rotate(transform.forward, this.RandomAngle());
        private float RandomAngle() => UnityEngine.Random.Range(angleMinMax.x, angleMinMax.y);

        public void Shoot(object sender, EventArgs args)
        {
            //TODO: Pooling
            var bulletObj = Instantiate(this.projectile, this.gunTransform.position, gunTransform.rotation);
            bulletObj.transform.SetParent(SceneManager.Instance.BulletsParent.transform);
            var bullet = bulletObj.GetComponent<IProjectile>();
            bullet.Direction = this.gunTransform.up;
            bullet.NoCreatingBullet = false;
        }

        public void LastSalvo(object sender, EventArgs args)
        {
            //TODO: Pooling
            var bulletObj = Instantiate(this.projectile, this.gunTransform.position, gunTransform.rotation);
            var bullet = bulletObj.GetComponent<IProjectile>();
            bullet.Direction = this.gunTransform.up;
            bullet.NoCreatingBullet = true;
        }


        public IEnumerator BattleCombat()
        {
            yield return (this.canShoot) ? null : new WaitForSeconds(this.startTime);
            this.spriteRenderer.color = this.activeTowerColor;
            for (int i = 0; i < MaxAttackCount; i++)
            {
                OnRotating.Invoke(this, EventArgs.Empty);
                OnShooting.Invoke(this, EventArgs.Empty);
                yield return new WaitForSeconds(this.rotateTime);
            }
            this.spriteRenderer.color = this.exhaustedTowerColor;
        }

        public void Destroy(object sender, EventArgs args)
        {
            //TODO: Pooling
            MonoBehaviour.Destroy(this.gameObject);
            SceneManager.Instance.DecreaseTowerCounter();
        }

        public void Destroy() => this.OnDestroy?.Invoke(this, EventArgs.Empty);

        public void Restart()
        {
            this.OnShooting = this.LastSalvo;
            StopAllCoroutines();
            this.canShoot = true;
            Debug.Log("Restart");
            StartCoroutine(this.BattleCombat());
        }
    }
}