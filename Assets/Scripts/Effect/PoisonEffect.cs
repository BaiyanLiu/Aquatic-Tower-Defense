using UnityEngine;

namespace Assets.Scripts.Effect
{
    public class PoisonEffect : EffectBase
    {
        public override string Name => "Poison";
        public override Color StatusColor => new Color32(34, 177, 76, 255);
    }
}
