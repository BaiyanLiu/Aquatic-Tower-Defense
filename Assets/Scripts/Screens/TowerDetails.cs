using System;
using System.Globalization;
using Assets.Scripts.Effect;
using Assets.Scripts.Item;
using Assets.Scripts.Tower;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public sealed class TowerDetails : DetailsScreen<TowerBase>
    {
        public IconText Level;
        public IconText Damage;
        public IconText Range;
        public IconText AttackSpeed;
        public IconText ProjectileSpeed;
        public IconText DamageDone;
        public IconText Kills;
        public IconText DamageType;

        public Upgrades Upgrades;
        public Inventory Inventory;
        public Transform RangeIndicator;
        public RectTransform SellButton;
        public Text SellButtonText;

        protected override EffectBase[] TargetEffects => Base.Effects.ToArray();

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
            Level.Text.text = $"{Base.Level} ({Base.Experience}/{Base.ExperienceRequired})";
            Damage.Text.text = $"{Math.Round(Base.Damage.Value, 2)} (+{Base.Damage.Gain})";
            Range.Text.text = $"{Math.Round(Base.Range.Value, 2)} (+{Base.Range.Gain})";
            AttackSpeed.Text.text = $"{Math.Round(Base.AttackSpeed.Value, 2)} ({Base.AttackSpeed.Gain})";
            ProjectileSpeed.Text.text = $"{Math.Round(Base.ProjectileSpeed.Value, 2)} (+{Base.ProjectileSpeed.Gain})";
            DamageDone.Text.text = Math.Round(Base.DamageDone).ToString(CultureInfo.InvariantCulture);
            Kills.Text.text = Base.Kills.ToString();
            DamageType.Text.text = Base.DamageType.ToString();

            if (!GameState.Instance.IsBuilding)
            {
                _upgradesTransform.anchoredPosition = new Vector2(5f, -(InitialHeight + height));
                height += _inventoryTransform.rect.height + 5f;
                Upgrades.gameObject.SetActive(true);

                _inventoryTransform.anchoredPosition = new Vector2(5f, -(InitialHeight + height));
                height += _inventoryTransform.rect.height + 5f;
                Inventory.gameObject.SetActive(true);

                if (!GameState.Instance.IsWaveActive)
                {
                    SellButtonText.text = "Sell: " + Base.SellCost.Value;
                    SellButton.anchoredPosition = new Vector2(5f, -(InitialHeight + height));
                    height += SellButton.rect.height + 5f;
                    SellButton.gameObject.SetActive(true);
                }
                else
                {
                    SellButton.gameObject.SetActive(false);
                }
            }
            else
            {
                Upgrades.gameObject.SetActive(false);
                Inventory.gameObject.SetActive(false);
                SellButton.gameObject.SetActive(false);
            }

            RangeIndicator.position = Target.transform.position;
            RangeIndicator.localScale = new Vector2(Base.Range.Value * 2f, Base.Range.Value * 2f);

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
            if (!GameState.Instance.IsBuilding && !GameState.Instance.IsWaveActive && Target != null)
            {
                GameState.Instance.UpdateGold((int) Base.SellCost.Value);
                Destroy(Target);
                UpdateTarget(null, false);
            }
        }
    }
}
