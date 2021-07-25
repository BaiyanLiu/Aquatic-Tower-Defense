using UnityEngine;

namespace Assets.Scripts.Effect.Innate
{
    public sealed class ExperienceTrickleEffect : InnateEffect
    {
        public override string Name => "Experience Trickle";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Award"];
    }
}
