using UnityEngine;

namespace Assets.Scripts.Effect
{
    public class ChainEffect : InnateEffect
    {
        public float Range;
        public float RangeGain;

        public override string Name => "Chain";
        public override Color StatusColor => new Color32(255, 242, 0, 255);

        public override void LevelUp()
        {
            base.LevelUp();
            Range += RangeGain;
        }
    }
}
