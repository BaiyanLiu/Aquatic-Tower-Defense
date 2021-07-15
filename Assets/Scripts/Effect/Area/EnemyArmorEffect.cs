using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Effect.Area
{
    public sealed class EnemyArmorEffect : EnemyAreaEffect<EnemyBase>
    {
        public override string Name => "Armor Chant";
        protected override string AmountName => "Armor";
        public override Color StatusColor => Colors.Instance.Yellow;
    }
}
