using Assets.Scripts.Tower;

namespace Assets.Scripts.Upgrade
{
    public class DamageUpgrade : UpgradeBase
    {
        public override string Name => "Damage Upgrade";
        public override string AmountName => "Damage";

        public override void Apply(TowerBase tower)
        {
            if (Level == -1)
            {
                return;
            }
            tower.Damage += Amount[Level];
        }
    }
}
