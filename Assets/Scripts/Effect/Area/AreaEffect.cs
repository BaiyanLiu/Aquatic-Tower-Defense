using System.Collections.Generic;
using Assets.Scripts.Enemy;
using Assets.Scripts.Tower;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public abstract class AreaEffect<T> : EffectBase where T : IHasEffect
    {
        public Attribute<float> Range;
        public CircleCollider2D Collider;

        public override bool IsConstant => true;

        private readonly ISet<T> _targets = new HashSet<T>();

        protected override void BeforeStart()
        {
            switch (Source)
            {
                case EnemyBase source:
                    source.OnDie += HandleSourceDestroyed;
                    source.OnDestroyed += HandleSourceDestroyed;
                    break;
                case TowerBase source:
                    source.OnDestroyed += HandleSourceDestroyed;
                    break;
            }
        }

        protected void HandleSourceDestroyed(object sender, GameObject source)
        {
            foreach (var target in _targets)
            {
                target.RemoveEffect(this);
            }
        }

        protected override void OnStart()
        {
            Range ??= new Attribute<float>();
            Collider.radius = Range.Value = Range.Base;
        }

        public override void LevelUp()
        {
            base.LevelUp();
            Range.Value += Range.Gain;
            Collider.radius = Range.Value;
        }

        public override void UpdateLevel(int level)
        {
            base.UpdateLevel(level);
            Range.Value = Range.Base + Range.Gain * (level - 1);
            Collider.radius = Range.Value;
        }

        public override List<string> GetAmountDisplayText()
        {
            var amountDisplayText = base.GetAmountDisplayText();
            amountDisplayText.Add(FormatDisplayText("Range", Range, false));
            return amountDisplayText;
        }

        public override object Clone()
        {
            var clone = (AreaEffect<T>) base.Clone();
            clone.Range = (Attribute<float>) Range.Clone();
            return clone;
        }

        [UsedImplicitly]
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsInCollider(collision))
            {
                return;
            }

            var target = collision.gameObject.GetComponent<T>();
            if (target != null)
            {
                _targets.Add(target);
                target.AddEffect(this);
            }
        }

        [UsedImplicitly]
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (IsInCollider(collision))
            {
                return;
            }

            var target = collision.gameObject.GetComponent<T>();
            if (target != null)
            {
                _targets.Remove(target);
                target.RemoveEffect(this);
            }
        }

        private bool IsInCollider(Collider2D collision)
        {
            var hitColliders = new List<Collider2D>();
            collision.OverlapCollider(new ContactFilter2D().NoFilter(), hitColliders);
            return hitColliders.Contains(Collider);
        }
    }
}
