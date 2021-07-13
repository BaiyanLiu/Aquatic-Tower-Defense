using UnityEngine;

namespace Assets.Scripts.Effect
{
    public sealed class PoisonEffect : EffectBase
    {
        public override string Name => "Poison";
        protected override string AmountName => "Damage";
        public override Color StatusColor => Colors.Instance.Green;
    }
}
