using System;
using JetBrains.Annotations;

namespace Assets.Scripts.Tower
{
    [UsedImplicitly]
    public sealed class LevelIndicator : LevelIndicatorBase 
    {
        public TowerBase Tower;

        protected override int Level => Tower.Level - 1;
        protected override int MaxLevel => 10;

        protected override void OnStart()
        {
            Tower.OnLevelUp += HandleTowerLevelUp;
        }

        private void HandleTowerLevelUp(object sender, EventArgs e)
        {
            UpdateColor();
        }
    }
}
