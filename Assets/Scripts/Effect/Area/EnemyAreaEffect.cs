using System.Collections.Generic;
using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public abstract class EnemyAreaEffect<T> : AreaEffect<T> where T : IHasEffect
    {
        private readonly List<T> _targets = new List<T>();

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

        protected override void OnTargetEnter(T target)
        {
            _targets.Add(target);
        }

        protected override void OnTargetExit(T target)
        {
            _targets.Remove(target);
        }
    }
}
