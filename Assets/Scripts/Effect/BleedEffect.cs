using UnityEngine;

namespace Assets.Scripts.Effect
{
    public sealed class BleedEffect : EffectBase
    {
        public override string Name => "Bleeding";
        protected override string AmountUnit => "%";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Scalpel"];
        public override Color StatusColor => Colors.Instance.Red;
    }
}
