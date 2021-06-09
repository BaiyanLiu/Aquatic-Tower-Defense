using UnityEngine;

namespace Assets.Scripts.Effect
{
    public class PoisonEffect : EffectBase
    {
        public override string Name => "Poison Effect";
        public override string AmountName => "Damage";
        public override Color StatusColor => new Color32(34, 177, 76, 255);
    }
}
