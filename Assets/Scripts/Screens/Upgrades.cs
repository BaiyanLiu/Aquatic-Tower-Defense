using System;
using System.Collections.Generic;
using Assets.Scripts.Upgrade;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Screens
{
    public class Upgrades : MonoBehaviour
    {
        public Transform UpgradesParent;
        public UpgradeDetails UpgradeDetails;

        private readonly List<GameObject> _upgrades = new List<GameObject>();
        private Vector2 _scale;
        private float _positionOffset;
        private int _sortingOrder;

        [UsedImplicitly]
        private void Start()
        {
            var parent = UpgradesParent.parent;
            _sortingOrder = parent.GetComponent<Canvas>().sortingOrder;

            while (Math.Abs(parent.localScale.x - 1) < 0.00000001f)
            {
                parent = parent.parent;
            }
            _scale = new Vector2(1f / parent.localScale.x, 1f / parent.localScale.y);
            _positionOffset = _scale.x / 2f + 7.6f;

            gameObject.SetActive(false);
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
                var upgradeObject = Instantiate(upgrade.Prefab, Vector2.zero, Quaternion.identity, UpgradesParent);
                // ReSharper disable once PossibleLossOfFraction
                upgradeObject.transform.localPosition = new Vector3(_upgrades.Count % 6 * _positionOffset, 0f, -1f);
                upgradeObject.transform.localScale = _scale;
                upgradeObject.GetComponent<SpriteRenderer>().sortingOrder = _sortingOrder;

                var interaction = upgradeObject.GetComponent<Interaction>();
                interaction.OnEnter += (sender, o) =>
                {
                    UpgradeDetails.UpdateTarget(upgradeObject, true, upgrade);
                };
                interaction.OnExit += HandleUpgradeMouseExit;

                _upgrades.Add(upgradeObject.gameObject);
            }
        }

        private void HandleUpgradeMouseExit(object sender, EventArgs e)
        {
            UpgradeDetails.UpdateTarget(null);
        }
    }
}
