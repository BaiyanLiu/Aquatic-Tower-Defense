using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public sealed class EnemyTowerDamageEffect : EnemyAreaEffect<TowerBase>
    {
        public override string Name => "Electric Shock";
        protected override string AmountName => "Damage";
        protected override string AmountUnit => "%";
        public override Color StatusColor => Colors.Instance.Purple;
    }
}
