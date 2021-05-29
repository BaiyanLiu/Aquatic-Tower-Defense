using Assets.Scripts.Tower;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public class TowerDetails : MonoBehaviour
    {
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

        private TowerBase _tower;

        private void Update()
        {
            if (_tower == null)
            {
                return;
            }

            NameText.text = _tower.Name;
            LevelText.text = $"Level: {_tower.Level} ({_tower.Experience}/{_tower.ExperienceRequired})";
            DamageText.text = $"Damage: {_tower.Damage} (+{_tower.DamageGain})";
            RangeText.text = $"Range: {_tower.Range} (+{_tower.RangeGain})";
            AttackSpeedText.text = $"Attack Speed: {_tower.AttackSpeed} (+{_tower.AttackSpeedGain})";
            ProjectileSpeedText.text = $"P. Speed: {_tower.ProjectileSpeed} (+{_tower.ProjectileSpeedGain})";
            SplashText.text = $"Splash: {_tower.Splash} (+{_tower.SplashGain})";
            ChainDamageText.text = $"Chain Damage: {_tower.ChainDamage} (+{_tower.ChainDamageGain})";
            ChainRangeText.text = $"Chain Range: {_tower.ChainRangeGain} (+{_tower.ChainRangeGain})";
            DamageTypeText.text = "Damage Type: " + _tower.DamageType;
        }

        public void UpdateTower(GameObject tower)
        {
            _tower = tower.GetComponentInChildren<TowerBase>();
        }
    }
}
