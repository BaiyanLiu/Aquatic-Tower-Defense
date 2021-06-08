using System;
using System.Collections.Generic;
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
        public Transform EffectsParent;
        public RectTransform SellButton;
        public Text SellButtonText;

        private readonly List<GameObject> _effects = new List<GameObject>();
        private readonly List<EffectDisplay> _effectDisplays = new List<EffectDisplay>();
        private readonly List<RectTransform> _effectTransforms = new List<RectTransform>();

        protected override void OnStart()
        {
            for (var i = 0; i < EffectsParent.childCount; i++)
            {
                var effect = EffectsParent.GetChild(i).gameObject;
                _effects.Add(effect);
                _effectDisplays.Add(effect.GetComponent<EffectDisplay>());
                _effectTransforms.Add(effect.GetComponent<RectTransform>());
            }
        }

        protected override void OnUpdate()
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

            var height = 0f;
            for (var i = 0; i < _effects.Count; i++)
            {
                if (i < Base.Effects.Length)
                {
                    _effectTransforms[i].anchoredPosition = new Vector2(0f, -height);
                    height += _effectDisplays[i].UpdateEffect(Base.Effects[i]) + 10f;
                    _effects[i].SetActive(true);
                }
                else
                {
                    _effects[i].SetActive(false);
                }
            }

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

            Screen.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, InitialHeight + height);

            RangeIndicator.position = Target.transform.position;
            RangeIndicator.localScale = new Vector2(Base.Range * 2f, Base.Range * 2f);

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Sell();
            }
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
