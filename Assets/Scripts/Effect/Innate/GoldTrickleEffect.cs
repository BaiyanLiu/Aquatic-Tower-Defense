using UnityEngine;

namespace Assets.Scripts.Effect.Innate
{
    public sealed class GoldTrickleEffect : InnateEffect
    {
        public override string Name => "Gold Trickle";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Circle_Dollar"];
    }
}
