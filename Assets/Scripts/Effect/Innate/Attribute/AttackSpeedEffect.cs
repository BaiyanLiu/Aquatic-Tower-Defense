using Assets.Scripts.Tower;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Effect.Innate.Attribute
{
    [UsedImplicitly]
    public sealed class AttackSpeedEffect : AttributeEffect
    {
        public override string Name => "A. Speed Increase";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Stopwatch"];

        protected override AttributeValue GetAttribute(TowerBase tower)
        {
            return tower.AttackSpeed;
        }

        protected override void OnApplyMultiply(AttributeValue attribute)
        {
            attribute.Value /= Amount.Value / 100f;
        }
    }
}
