using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Screens;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Enemy
{
    public class WavePreview : MonoBehaviour
    {
        public Transform PreviewParent;
        public Text CountPrefab;
        public EnemyDetails EnemyDetails;

        private Wave _wave;
        private int _level;

        private Vector2 _scale;
        private Vector2 _initialPosition;

        [UsedImplicitly]
        private void Start()
        {
            _scale = 0.65f * new Vector2(1f / transform.parent.localScale.x, 1f / transform.parent.localScale.y);
            _initialPosition = new Vector2(_scale.x / 2f, 0f);
        }

        [UsedImplicitly]
        private void Update()
        {
            if (_wave != null)
            {
                if (PreviewParent.childCount > 0)
                {
                    return;
                }

                var groups = GroupEnemies();
                var position = _initialPosition;
                foreach (var group in groups)
                {
                    CreateEnemy(group.Key, position);
                    position.x += _scale.x / 2f + 10f;

                    var count = Instantiate(CountPrefab, Vector2.zero, Quaternion.identity, PreviewParent);
                    count.text = "X" + group.Value;
                    count.rectTransform.anchoredPosition = position;
                    position.x += count.rectTransform.rect.width + _scale.x / 2f + 10f;
                }
            }

            else if (PreviewParent.childCount > 0)
            {
                foreach (Transform child in PreviewParent)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        public void UpdateWave(Wave wave, int level)
        {
            _wave = wave;
            _level = level;
        }

        private Dictionary<GameObject, int> GroupEnemies()
        {
            var nameToGameObject = new Dictionary<string, GameObject>();
            var nameToCount = new Dictionary<string, int>();

            foreach (var enemy in _wave.Enemies)
            {
                var enemyName = enemy.GetComponent<EnemyBase>().Name;
                nameToGameObject[enemyName] = enemy;
                nameToCount.TryGetValue(enemyName, out var count);
                nameToCount[enemyName] = count + 1;
            }

            return nameToCount.Keys.ToDictionary(k => nameToGameObject[k], k => nameToCount[k]);
        }

        private void CreateEnemy(GameObject enemy, Vector2 position)
        {
            var enemyObject = Instantiate(enemy, Vector3.zero, Quaternion.identity, PreviewParent);
            enemyObject.name = "Preview";
            enemyObject.GetComponentInChildren<Animator>().enabled = false;
            enemyObject.GetComponent<Move>().enabled = false;
            enemyObject.GetComponent<CircleCollider2D>().radius = 0.5f;
            enemyObject.GetComponent<EnemyBase>().Level = _level;

            var enemyTransform = enemyObject.transform;
            enemyTransform.GetChild(1).gameObject.SetActive(false);
            enemyTransform.localScale = _scale;
            enemyTransform.GetChild(0).localScale = Vector2.one;
            enemyTransform.localPosition = position;

            var interaction = enemyObject.GetComponent<Interaction>();
            interaction.OnClick += HandleEnemyClick;
            interaction.OnEnter += HandleEnemyMouseEnter;
            interaction.OnExit += HandleEnemyMouseExit;
        }

        private void HandleEnemyClick(object sender, GameObject enemy)
        {
            EnemyDetails.UpdateTarget(enemy, false);
        }

        private void HandleEnemyMouseEnter(object sender, GameObject enemy)
        {
            EnemyDetails.UpdateTarget(enemy);
        }

        private void HandleEnemyMouseExit(object sender, EventArgs e)
        {
            EnemyDetails.UpdateTarget(null);
        }
    }
}
