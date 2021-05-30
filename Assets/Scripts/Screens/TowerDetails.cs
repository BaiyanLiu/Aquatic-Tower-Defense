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
        public Text SplashText;
        public Text ChainDamageText;
        public Text ChainRangeText;
        public Text DamageTypeText;

        private GameObject _tower;
        private TowerBase _base;
        private SpriteRenderer[] _spriteRenderers;

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
            AttackSpeedText.text = $"Attack Speed: {_base.AttackSpeed} ({_base.AttackSpeedGain})";
            ProjectileSpeedText.text = $"P. Speed: {_base.ProjectileSpeed} (+{_base.ProjectileSpeedGain})";
            SplashText.text = $"Splash: {_base.Splash} (+{_base.SplashGain})";
            ChainDamageText.text = $"Chain Damage: {_base.ChainDamage} (+{_base.ChainDamageGain})";
            ChainRangeText.text = $"Chain Range: {_base.ChainRangeGain} (+{_base.ChainRangeGain})";
            DamageTypeText.text = "Damage Type: " + _base.DamageType;
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
            }

            if (tower != _tower)
            {
                _tower = tower;
                _base = tower.GetComponentInChildren<TowerBase>();
                _spriteRenderers = tower.GetComponentsInChildren<SpriteRenderer>();
                foreach (var spriteRenderer in _spriteRenderers)
                {
                    spriteRenderer.color = SelectedColor;
                }

                Screen.pivot = tower.transform.position.y >= 0f ? Vector2.up : Vector2.zero;
                Screen.position = new Vector2(tower.transform.position.x, tower.transform.position.y);
                Screen.gameObject.SetActive(true);
            }
            else
            {
                _tower = null;
            }
        }
    }
}
