using System;
using System.Globalization;
using Assets.Scripts.Effect;
using Assets.Scripts.Enemy;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public sealed class EnemyDetails : DetailsScreen<EnemyBase>
    {
        public Text HealthText;
        public Text ArmorText;
        public Text SpeedText;
        public Text ExperienceText;
        public Text GoldText;
        public Text LivesText;
        public Text ItemChanceText;
        public Text ArmorTypeText;

        protected override EffectBase[] TargetEffects => Base.Effects.ToArray();

        protected override float OnUpdate(float height)
        {
            NameText.text = Base.Name;
            HealthText.text = $"{Math.Max(0, Math.Ceiling(Base.Health))}/{Base.MaxHealth.Value}";
            ArmorText.text = Base.Armor.Value.ToString(CultureInfo.InvariantCulture);
            SpeedText.text = Base.Speed.ToString(CultureInfo.InvariantCulture);
            ExperienceText.text = Base.Experience.ToString();
            GoldText.text = Base.Gold.ToString();
            LivesText.text =Base.Lives.ToString();
            ItemChanceText.text = Base.ItemChance * 100 + "%";
            ArmorTypeText.text = Base.ArmorType.ToString();
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
