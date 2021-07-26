using System;
using System.Globalization;
using Assets.Scripts.Effect;
using Assets.Scripts.Enemy;
using UnityEngine;

namespace Assets.Scripts.Screens
{
    public sealed class EnemyDetails : DetailsScreen<EnemyBase>
    {
        public IconText Health;
        public IconText Armor;
        public IconText Speed;
        public IconText Experience;
        public IconText Gold;
        public IconText Lives;
        public IconText ItemChance;
        public IconText ArmorType;

        protected override EffectBase[] TargetEffects => Base.Effects.ToArray();

        protected override float OnUpdate(float height)
        {
            NameText.text = Base.Name;
            Health.Text.text = $"{Math.Max(0, Math.Ceiling(Base.Health))}/{Base.MaxHealth.Value}";
            Armor.Text.text = Base.Armor.Value.ToString(CultureInfo.InvariantCulture);
            Speed.Text.text = Base.Speed.ToString(CultureInfo.InvariantCulture);
            Experience.Text.text = Base.Experience.ToString();
            Gold.Text.text = Base.Gold.ToString();
            Lives.Text.text =Base.Lives.ToString();
            ItemChance.Text.text = Base.ItemChance * 100 + "%";
            ArmorType.Text.text = Base.ArmorType.ToString();
            return height;
        }

        protected override void OnDeselected()
        {
            Target.GetComponent<EnemyBase>().OnDestroyed -= HandleEnemyDestroyed;
        }

        protected override void OnSelected()
        {
            Target.GetComponent<EnemyBase>().OnDestroyed += HandleEnemyDestroyed;
        }

        private void HandleEnemyDestroyed(object sender, GameObject e)
        {
            UpdateTarget(null, false);
        }
    }
}
