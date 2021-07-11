using JetBrains.Annotations;

namespace Assets.Scripts.Upgrade
{
    [UsedImplicitly]
    public sealed class LevelIndicator : LevelIndicatorBase
    {
        public UpgradeBase Upgrade { private get; set; }

        protected override int Level => Upgrade.Level + 1;
        protected override int MaxLevel => Upgrade.Cost.Length;
    }
}
