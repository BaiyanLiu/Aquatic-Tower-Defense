using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public abstract class EnemyAreaEffect : AreaEffect
    {
        protected sealed override IHasEffect GetTargetFromCollision(Collider2D collision)
        {
            return collision.GetComponent<EnemyBase>();
        }
    }
}
