using System;
using Assets.Scripts.Effect;
using Assets.Scripts.Item;
using Assets.Scripts.Tower;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public sealed class TowerDetails : DetailsScreen<TowerBase>
    {
        public Text LevelText;
        public Text DamageText;
        public Text RangeText;
        public Text AttackSpeedText;
        public Text ProjectileSpeedText;
        public Text DamageDoneText;
        public Text KillsText;
        public Text DamageTypeText;

        public Upgrades Upgrades;
        public Inventory Inventory;
        public Transform RangeIndicator;
        public RectTransform SellButton;
        public Text SellButtonText;

        protected override EffectBase[] TargetEffects => Base.Effects ?? new EffectBase[0];

        private RectTransform _upgradesTransform;
        private RectTransform _inventoryTransform;

        protected override void OnStart()
        {
            _upgradesTransform = Upgrades.GetComponent<RectTransform>();
            _inventoryTransform = Inventory.GetComponent<RectTransform>();
        }

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
                _upgradesTransform.anchoredPosition = new Vector2(5f, -(InitialHeight + height));
                height += _inventoryTransform.rect.height + 5f;
                Upgrades.gameObject.SetActive(true);

                _inventoryTransform.anchoredPosition = new Vector2(5f, -(InitialHeight + height));
                height += _inventoryTransform.rect.height + 5f;
                Inventory.gameObject.SetActive(true);

                SellButtonText.text = "Sell: " + Base.SellCost;
                SellButton.anchoredPosition = new Vector2(5f, -(InitialHeight + height));
                height += SellButton.rect.height + 5f;
                SellButton.gameObject.SetActive(true);
            }
            else
            {
                Upgrades.gameObject.SetActive(false);
                Inventory.gameObject.SetActive(false);
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
            Upgrades.SetUpgrades(null);
            Inventory.ResetItems();
            Base.OnItemAdded -= HandleTowerAdded;
            Base.OnItemRemoved -= HandleItemRemoved;
        }

        protected override void OnSelected()
        {
            RangeIndicator.gameObject.SetActive(true);
            Upgrades.SetUpgrades(Base.Upgrades);
            Base.Items.ForEach(Inventory.AddItem);
            Base.OnItemAdded += HandleTowerAdded;
            Base.OnItemRemoved += HandleItemRemoved;
        }

        private void HandleTowerAdded(object sender, ItemBase item)
        {
            Inventory.AddItem(item);
        }

        private void HandleItemRemoved(object sender, int index)
        {
            Inventory.RemoveItem(index);
        }

        public void Sell()
        {
            if (!GameState.IsBuilding && Target != null)
            {
                GameState.UpdateGold(Base.SellCost);
                Destroy(Target);
                UpdateTarget(null, false);
            }
        }
    }
}
