using UnityEngine;

namespace Assets.Scripts.Effect.enemy
{
    public sealed class EnemyArmorEffect : EnemyEffect
    {
        public override string Name => "Armor Chant";
        protected override string AmountName => "Armor";
        public override Color StatusColor => Colors.Instance.Yellow;
    }
}
