using System;
using System.Collections.Generic;
using Assets.Scripts.Upgrade;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Screens
{
    public sealed class Upgrades : MonoBehaviour
    {
        public Transform UpgradesParent;
        public UpgradeDetails UpgradeDetails;

        private readonly List<GameObject> _upgrades = new List<GameObject>();
        private Vector2 _scale;
        private float _positionOffset;
        private int _sortingOrder;

        private UpgradeBase _current;
        private SpriteRenderer _spriteRenderer;

        [UsedImplicitly]
        private void Start()
        {
            var parent = UpgradesParent.parent;
            _sortingOrder = parent.GetComponent<Canvas>().sortingOrder;

            while (Math.Abs(parent.localScale.x - 1) < 0.000001f)
            {
                parent = parent.parent;
            }
            _scale = new Vector2(1f / parent.localScale.x, 1f / parent.localScale.y);
            _positionOffset = _scale.x / 2f + 7.6f;

            gameObject.SetActive(false);
        }

        [UsedImplicitly]
        private void Update()
        {
            if (_current != null && _spriteRenderer != null)
            {
                _spriteRenderer.color = _current.CanLevelUp ? Colors.Instance.Green : Colors.Instance.Red;
            }
        }

        public void SetUpgrades(UpgradeBase[] upgrades)
        {
            _upgrades.ForEach(Destroy);
            _upgrades.Clear();
            UpgradeDetails.UpdateTarget(null);

            if (upgrades == null)
            {
                return;
            }

            foreach (var upgrade in upgrades)
            {
                var upgradeObject = Instantiate(upgrade.Icon, Vector2.zero, Quaternion.identity, UpgradesParent);
                upgradeObject.transform.localPosition = new Vector3(_upgrades.Count % 6 * _positionOffset, 0f, -100f);
                upgradeObject.transform.localScale = _scale;
                foreach (var spriteRenderer in upgradeObject.GetComponentsInChildren<SpriteRenderer>())
                {
                    spriteRenderer.sortingOrder = _sortingOrder;
                }

                var interaction = upgradeObject.GetComponentInChildren<Interaction>();
                interaction.OnClick += HandleUpgradeClick;
                interaction.OnEnter += (sender, o) =>
                {
                    _current = upgrade;
                    _spriteRenderer = o.GetComponentInChildren<SpriteRenderer>();
                    GameState.Instance.UpdateCost(upgrade.NextCost);
                    UpgradeDetails.UpdateTarget(upgradeObject, true, upgrade);
                };
                interaction.OnExit += HandleUpgradeMouseExit;

                _upgrades.Add(upgradeObject.gameObject);
            }
        }

        private void HandleUpgradeClick(object sender, GameObject o)
        {
            _current.LevelUp();
            GameState.Instance.UpdateCost(_current.NextCost);
        }

        private void HandleUpgradeMouseExit(object sender, EventArgs e)
        {
            _current = null;
            _spriteRenderer.color = Color.white;
            GameState.Instance.UpdateCost(null);
            UpgradeDetails.UpdateTarget(null);
        }
    }
}
