using UnityEngine;

namespace Assets.Scripts.Effect.Innate
{
    public sealed class GoldEffect : InnateEffect
    {
        public override string Name => "Gold Bonus";
        protected override string AmountUnit => "%";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Magnifying_Glass_Dollar"];
    }
}
