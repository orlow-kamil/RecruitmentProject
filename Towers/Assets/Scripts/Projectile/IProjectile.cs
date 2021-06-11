using System;
using UnityEngine;

namespace Projectile
{
    public interface IProjectile
    {
        bool NoCreatingBullet { get; set; }

        float Speed { get; set; }

        Vector2 Direction { get; set; }

        void Move();

        void Destroy(object sender, EventArgs args);

        void CreateTower(object sender, EventArgs args);
    }
}