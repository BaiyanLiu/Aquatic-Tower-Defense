using System;
using System.Collections.Generic;
using Assets.Scripts.Tower;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public class TowerDetails : MonoBehaviour
    {
        public RectTransform Screen;
        public Color SelectedColor;

        public Text NameText;
        public Text LevelText;
        public Text DamageText;
        public Text RangeText;
        public Text AttackSpeedText;
        public Text ProjectileSpeedText;
        public Text DamageDoneText;
        public Text KillsText;
        public Text DamageTypeText;
        public Transform RangeIndicator;
        public GameObject EffectsParent;

        private GameObject _tower;
        private TowerBase _base;
        private SpriteRenderer[] _spriteRenderers;
        private float _initialHeight;

        private readonly List<GameObject> _effects = new List<GameObject>();
        private readonly List<EffectDisplay> _effectDisplays = new List<EffectDisplay>();
        private readonly List<RectTransform> _effectTransforms = new List<RectTransform>();

        private void Start()
        {
            _initialHeight = Screen.rect.height;
            for (var i = 0; i < EffectsParent.transform.childCount; i++)
            {
                var effect = EffectsParent.transform.GetChild(i).gameObject;
                _effects.Add(effect);
                _effectDisplays.Add(effect.GetComponent<EffectDisplay>());
                _effectTransforms.Add(effect.GetComponent<RectTransform>());
            }
        }

        private void Update()
        {
            if (_tower == null)
            {
                return;
            }

            NameText.text = _base.Name;
            LevelText.text = $"Level: {_base.Level} ({_base.Experience}/{_base.ExperienceRequired})";
            DamageText.text = $"Damage: {_base.Damage} (+{_base.DamageGain})";
            RangeText.text = $"Range: {_base.Range} (+{_base.RangeGain})";
            AttackSpeedText.text = $"A. Speed: {_base.AttackSpeed} ({_base.AttackSpeedGain})";
            ProjectileSpeedText.text = $"P. Speed: {_base.ProjectileSpeed} (+{_base.ProjectileSpeedGain})";
            DamageDoneText.text = "Dmg Done: " + Math.Round(_base.DamageDone);
            KillsText.text = "Kills: " + _base.Kills;
            DamageTypeText.text = "Damage Type: " + _base.DamageType;

            var height = 0f;
            for (var i = 0; i < _effects.Count; i++)
            {
                if (i < _base.Effects.Count)
                {
                    _effectTransforms[i].anchoredPosition = new Vector2(0f, -height);
                    height += _effectDisplays[i].UpdateEffect(_base.Effects[i]) + 10f;
                    _effects[i].SetActive(true);
                }
                else
                {
                    _effects[i].SetActive(false);
                }
            }

            Screen.pivot = new Vector2(-0.1f, _tower.transform.position.y >= 0f ? 1f : 0f);
            Screen.position = new Vector2(_tower.transform.position.x, _tower.transform.position.y);
            Screen.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _initialHeight + height);

            RangeIndicator.position = _tower.transform.position;
            RangeIndicator.localScale = new Vector2(_base.Range * 2f, _base.Range * 2f);
        }

        public void UpdateTower(GameObject tower)
        {
            if (_tower != null)
            {
                foreach (var spriteRenderer in _spriteRenderers)
                {
                    spriteRenderer.color = Color.white;
                }
                Screen.gameObject.SetActive(false);
                RangeIndicator.gameObject.SetActive(false);
            }

            if (tower != null && tower != _tower)
            {
                _tower = tower;
                _base = tower.GetComponentInChildren<TowerBase>();
                _spriteRenderers = tower.GetComponentsInChildren<SpriteRenderer>();
                foreach (var spriteRenderer in _spriteRenderers)
                {
                    spriteRenderer.color = SelectedColor;
                }
                Screen.gameObject.SetActive(true);
                RangeIndicator.gameObject.SetActive(true);
            }
            else
            {
                _tower = null;
            }
        }
    }
}
