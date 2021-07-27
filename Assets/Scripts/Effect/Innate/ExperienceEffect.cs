using UnityEngine;

namespace Assets.Scripts.Effect.Innate
{
    public sealed class ExperienceEffect : InnateEffect
    {
        public override string Name => "Experience Bonus";
        protected override string AmountUnit => "%";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Trophy_Star"];
    }
}
