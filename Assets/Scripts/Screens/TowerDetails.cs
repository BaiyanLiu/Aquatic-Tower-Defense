using System;
using Assets.Scripts.Effect;
using Assets.Scripts.Tower;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public class TowerDetails : DetailsScreen<TowerBase>
    {
        public Text LevelText;
        public Text DamageText;
        public Text RangeText;
        public Text AttackSpeedText;
        public Text ProjectileSpeedText;
        public Text DamageDoneText;
        public Text KillsText;
        public Text DamageTypeText;

        public Transform RangeIndicator;
        public RectTransform SellButton;
        public Text SellButtonText;

        protected override EffectBase[] TargetEffects => Base.Effects;

        protected override float OnUpdate(float height)
        {
            NameText.text = Base.Name;
            LevelText.text = $"Level: {Base.Level} ({Base.Experience}/{Base.ExperienceRequired})";
            DamageText.text = $"Damage: {Base.Damage} (+{Base.DamageGain})";
            RangeText.text = $"Range: {Base.Range} (+{Base.RangeGain})";
            AttackSpeedText.text = $"A. Speed: {Base.AttackSpeed} ({Base.AttackSpeedGain})";
            ProjectileSpeedText.text = $"P. Speed: {Base.ProjectileSpeed} (+{Base.ProjectileSpeedGain})";
            DamageDoneText.text = "Dmg Done: " + Math.Round(Base.DamageDone);
            KillsText.text = "Kills: " + Base.Kills;
            DamageTypeText.text = "Damage Type: " + Base.DamageType;

            if (!GameState.IsBuilding)
            {
                SellButtonText.text = "Sell: " + Base.SellCost;
                SellButton.anchoredPosition = new Vector2(5f, -(InitialHeight + height));
                height += SellButton.rect.height + 5f;
                SellButton.gameObject.SetActive(true);
            }
            else
            {
                SellButton.gameObject.SetActive(false);
            }

            RangeIndicator.position = Target.transform.position;
            RangeIndicator.localScale = new Vector2(Base.Range * 2f, Base.Range * 2f);

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Sell();
            }

            return height;
        }

        protected override void OnDeselected()
        {
            RangeIndicator.gameObject.SetActive(false);
        }

        protected override void OnSelected()
        {
            RangeIndicator.gameObject.SetActive(true);
        }

        public void Sell()
        {
            if (!GameState.IsBuilding && Target != null)
            {
                GameState.UpdateGold(Base.SellCost);
                Destroy(Target);
                UpdateTarget(null);
            }
        }
    }
}
