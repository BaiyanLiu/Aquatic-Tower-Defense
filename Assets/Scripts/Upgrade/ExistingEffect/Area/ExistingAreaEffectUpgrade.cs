using System.Collections.Generic;
using Assets.Scripts.Effect.Area;

namespace Assets.Scripts.Upgrade.ExistingEffect.Area
{
    public abstract class ExistingAreaEffectUpgrade<T> : ExistingEffectUpgrade<T> where T : AreaEffect
    {
        public float[] Range;

        protected override void OnLevelUp()
        {
            var delta = GetDelta(Amount);
            Effect.Amount.Base += delta;
            Effect.UpdateAmount(Effect.Amount.Value + delta);

            delta = GetDelta(Range);
            Effect.Range.Base += delta;
            Effect.UpdateRange(Effect.Range.Value + delta);
        }

        public override List<string> GetAmountDisplayText()
        {
            var amountDisplayText = base.GetAmountDisplayText();
            amountDisplayText.Add(FormatDisplayText(Range, false));
            return amountDisplayText;
        }
    }
}
