using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public abstract class AreaTowerEffect : AreaEffect
    {
        protected sealed override IHasEffect GetTargetFromCollision(Collider2D collision)
        {
            var parent = collision.transform.parent;
            if (collision is BoxCollider2D && parent.name.StartsWith("Tower"))
            {
                return parent.GetComponentInChildren<TowerBase>();
            }
            return null;
        }

        protected sealed override void RefreshEffect(IHasEffect target)
        {
            if (target is TowerBase tower)
            {
                tower.UpdateStats();
            }
        }
    }
}
