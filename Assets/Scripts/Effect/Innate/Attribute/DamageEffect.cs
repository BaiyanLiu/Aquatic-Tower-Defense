using Assets.Scripts.Tower;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Effect.Innate.Attribute
{
    [UsedImplicitly]
    public sealed class DamageEffect : AttributeEffect
    {
        public override string Name => "Damage Increase";
        protected override Sprite AmountIcon => GameState.Instance.IconsByName["Icon_Swords"];

        protected override AttributeValue GetAttribute(TowerBase tower)
        {
            return tower.Damage;
        }
    }
}
