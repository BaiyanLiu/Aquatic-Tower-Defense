using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public sealed class LevelIndicator : MonoBehaviour
    {
        public TowerBase Tower;

        private SpriteRenderer _spriteRenderer;
        private Color _delta;

        [UsedImplicitly]
        private void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _delta = (Colors.Instance.Yellow - Color.white) / 9;

            Tower.OnLevelUp += HandleTowerLevelUp;
            UpdateColor();
        }

        private void HandleTowerLevelUp(object sender, EventArgs e)
        {
            UpdateColor();
        }

        private void UpdateColor()
        {
            _spriteRenderer.color = Color.white + _delta * (Tower.Level - 1);
        }
    }
}
