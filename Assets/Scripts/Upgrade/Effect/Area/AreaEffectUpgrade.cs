using System.Collections.Generic;
using Assets.Scripts.Effect.Area;
using UnityEngine;

namespace Assets.Scripts.Upgrade.Effect.Area
{
    public abstract class AreaEffectUpgrade<T> : EffectUpgrade<T> where T : AreaEffect
    {
        public float[] Range;
        public CircleCollider2D Collider;

        protected override void OnStart()
        {
            base.OnStart();
            Effect.Collider = Collider;
        }

        protected override void OnLevelUp()
        {
            base.OnLevelUp();
            Effect.UpdateRange(Range[Level]);
        }

        protected override void InitForLoading()
        {
            base.InitForLoading();
            Effect.Range = new AttributeValue();
        }

        public override List<string> GetAmountDisplayText()
        {
            var amountDisplayText = base.GetAmountDisplayText();
            amountDisplayText.Add(FormatDisplayText("Range", Range, false));
            return amountDisplayText;
        }
    }
}
