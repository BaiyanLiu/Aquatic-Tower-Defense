using UnityEngine;

namespace Assets.Scripts.Effect
{
    public sealed class SlowEffect : EffectBase
    {
        public override string Name => "Slow Effect";
        public override Color StatusColor => new Color32(0, 162, 232, 255);
    }
}
