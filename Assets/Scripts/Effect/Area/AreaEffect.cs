using System.Collections.Generic;
using Assets.Scripts.Enemy;
using Assets.Scripts.Tower;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public abstract class AreaEffect : EffectBase
    {
        public AttributeValue Range;
        public CircleCollider2D Collider;

        public override bool IsConstant => true;

        private readonly ISet<IHasEffect> _targets = new HashSet<IHasEffect>();

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

        private void HandleSourceDestroyed(object sender, GameObject source)
        {
            foreach (var target in _targets)
            {
                target.RemoveEffect(this);
            }
        }

        protected override void OnStart()
        {
            Range ??= new AttributeValue();
            UpdateRange(Range.Base);
        }

        public override void LevelUp()
        {
            base.LevelUp();
            UpdateRange(Range.Value + Range.Gain);
        }

        public override void UpdateLevel(int level)
        {
            base.UpdateLevel(level);
            UpdateRange(Range.Base + Range.Gain * (level - 1));
        }

        public void UpdateAmount(float amount)
        {
            Amount.Value = amount;
            foreach (var target in _targets)
            {
                RefreshEffect(target);
            }
        }

        protected virtual void RefreshEffect(IHasEffect target) {}

        public void UpdateRange(float range)
        {
            Collider.radius = Range.Value = range;
        }

        public override List<string> GetAmountDisplayText()
        {
            var amountDisplayText = base.GetAmountDisplayText();
            amountDisplayText.Add(FormatDisplayText(Range, false));
            return amountDisplayText;
        }

        public override List<Sprite> GetAmountIcon()
        {
            var amountIcon = base.GetAmountIcon();
            amountIcon.Add(GameState.Instance.IconsByName["Icon_Signal_Stream"]);
            return amountIcon;
        }

        public override object Clone()
        {
            var clone = (AreaEffect) base.Clone();
            clone.Range = (AttributeValue) Range.Clone();
            return clone;
        }

        [UsedImplicitly]
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var target = GetValidTarget(collision, true);
            if (target != null)
            {
                _targets.Add(target);
                target.AddEffect(this);
            }
        }

        [UsedImplicitly]
        private void OnTriggerExit2D(Collider2D collision)
        {
            var target = GetValidTarget(collision, false);
            if (target != null)
            {
                _targets.Remove(target);
                target.RemoveEffect(this);
            }
        }

        private IHasEffect GetValidTarget(Collider2D collision, bool isEnter)
        {
            if (collision.GetComponent<Interaction>() == null)
            {
                return null;
            }

            var hitColliders = new List<Collider2D>();
            collision.OverlapCollider(new ContactFilter2D().NoFilter(), hitColliders);
            if (hitColliders.Contains(Collider) != isEnter)
            {
                return null;
            }

            return GetTargetFromCollision(collision);
        }

        protected abstract IHasEffect GetTargetFromCollision(Collider2D collision);
    }
}
