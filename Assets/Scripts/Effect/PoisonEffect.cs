using UnityEngine;

namespace Assets.Scripts.Effect
{
    public sealed class PoisonEffect : EffectBase
    {
        public override string Name => "Poison";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Skull_Crossbones"];
        public override Color StatusColor => Colors.Instance.Green;
    }
}
