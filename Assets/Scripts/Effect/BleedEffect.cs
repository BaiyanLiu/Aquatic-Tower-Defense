using UnityEngine;

namespace Assets.Scripts.Effect
{
    public sealed class BleedEffect : EffectBase
    {
        public override string Name => "Bleeding";
        protected override string AmountName => "Damage";
        protected override string AmountUnit => "%";
        public override Color StatusColor => Colors.Instance.Red;
    }
}
