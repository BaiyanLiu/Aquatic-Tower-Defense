using System.Collections.Generic;
using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public abstract class EnemyAreaEffect : AreaEffect<EnemyBase>
    {
        private readonly List<EnemyBase> _targets = new List<EnemyBase>();

        protected override void OnStart()
        {
            base.OnStart();
            var source = (EnemyBase) Source;
            source.OnDie += HandleSourceDestroyed;
            source.OnDestroyed += HandleSourceDestroyed;
        }

        private void HandleSourceDestroyed(object sender, GameObject e)
        {
            _targets.ForEach(target => target.RemoveEffect(this));
        }

        protected override void OnTargetEnter(EnemyBase target)
        {
            _targets.Add(target);
        }

        protected override void OnTargetExit(EnemyBase target)
        {
            _targets.Remove(target);
        }
    }
}
