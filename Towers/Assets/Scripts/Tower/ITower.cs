using System;
using System.Collections;
using UnityEngine;

namespace Tower
{
    public interface ITower
    {
        bool CanShoot { get; set; }

        SpriteRenderer SpriteRenderer { get; set; }

        IEnumerator BattleCombat();
        void Rotate(object sender, EventArgs args);

        void Shoot(object sender, EventArgs args);

        void Destroy(object sender, EventArgs args);

        void Destroy();

        void Restart();
    }
}