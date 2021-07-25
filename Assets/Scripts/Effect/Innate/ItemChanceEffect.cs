using UnityEngine;

namespace Assets.Scripts.Effect.Innate
{
    public sealed class ItemChanceEffect : InnateEffect
    {
        public override string Name => "Item Drop Chance";
        protected override string AmountUnit => "%";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Gift"];
    }
}
