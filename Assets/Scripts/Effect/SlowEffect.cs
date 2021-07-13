using UnityEngine;

namespace Assets.Scripts.Effect
{
    public sealed class SlowEffect : EffectBase
    {
        public override string Name => "Slow";
        public override Color StatusColor => Colors.Instance.Blue;
    }
}
