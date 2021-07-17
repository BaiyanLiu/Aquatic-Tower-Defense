using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Screens;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Tower
{
    public sealed class Build : MonoBehaviour
    {
        public GameObject[] Towers;
        public GameObject BuildMenu;
        public TowerDetails TowerDetails;
        public Text CurrentNameText;

        private readonly Dictionary<KeyCode, GameObject> _towers = new Dictionary<KeyCode, GameObject>();
        private GameObject _tower;
        private GameObject _placeholder;
        private string _name;
        private SpriteRenderer _spriteRenderer;
        private int _cost;

        private readonly List<GameObject> _buildMenuTowers = new List<GameObject>();
        private readonly List<string> _buildMenuNames = new List<string>();
        private readonly List<SpriteRenderer> _buildMenuSpriteRenderers = new List<SpriteRenderer>();
        private Vector2 _buildMenuScale;
        private Vector2 _buildMenuInitialPosition;
        private string _prevName;
        private float _prevNameWidth = -1f;

        [UsedImplicitly]
        private void Start()
        {
            var buildMenuParent = BuildMenu.transform.parent;
            _buildMenuScale = 0.75f * new Vector2(1f / buildMenuParent.localScale.x, 1f / buildMenuParent.localScale.y);
            BuildMenu.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (_buildMenuScale.x + 10f) * Towers.Length - 10f);
            _buildMenuInitialPosition = new Vector2(BuildMenu.GetComponent<RectTransform>().rect.xMin + _buildMenuScale.x / 2f, 0f);

            for (var i = 0; i < Towers.Length; i++)
            {
                var keyCode = KeyCode.None;
                if (i < 9)
                {
                    keyCode = KeyCode.Alpha1 + i;
                }
                else if (i == 9)
                {
                    keyCode = KeyCode.Alpha0;
                }

                _towers[keyCode] = Towers[i];

                var buildMenuTower = CreatePlaceholder(Towers[i], Vector2.zero, BuildMenu.transform);
                buildMenuTower.transform.localScale = _buildMenuScale;
                buildMenuTower.GetComponentInChildren<Interaction>().OnClick += (sender, args) =>
                {
                    OnKeyDown(keyCode);
                };

                _buildMenuTowers.Add(buildMenuTower);
                var towerName = buildMenuTower.GetComponentInChildren<TowerBase>().Name;
                _buildMenuNames.Add(towerName);
                _buildMenuSpriteRenderers.Add(buildMenuTower.GetComponentInChildren<SpriteRenderer>());

                GameState.Instance.RegisterTowerForName(towerName, Towers[i]);
            }

            UpdateBuildMenu();
        }

        [UsedImplicitly]
        private void Update()
        {
            foreach (var keyCode in _towers.Keys.Where(Input.GetKeyDown))
            {
                OnKeyDown(keyCode);
            }

            if (_placeholder != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (IsValid() && GameState.Instance.HasPath(_placeholder.transform.position))
                    {
                        CurrentNameText.text = _name = null;
                        GameState.Instance.IsBuilding = false;
                        GameState.Instance.UpdateGold(-_cost);
                        GameState.Instance.UpdateCost(null);
                        TowerDetails.UpdateTarget(null, false);

                        var tower = Instantiate(_tower, _placeholder.transform.position, Quaternion.identity);
                        GameState.Instance.RegisterTower(tower);
                        Destroy(_placeholder);
                    }
                }

                _placeholder.transform.position = GetMousePosition();
                _spriteRenderer.color = IsValid() ? Colors.Instance.Green : Colors.Instance.Red;
            }

            UpdateBuildMenu();
        }

        private void OnKeyDown(KeyCode keyCode)
        {
            if (_placeholder != null)
            {
                Destroy(_placeholder);
            }

            var tower = _towers[keyCode];
            if (_name != tower.GetComponentInChildren<TowerBase>().Name)
            {
                _tower = tower;
                _placeholder = CreatePlaceholder(tower, GetMousePosition());
                _placeholder.GetComponentInChildren<BoxCollider2D>().enabled = false;
                _name = _placeholder.GetComponentInChildren<TowerBase>().Name;
                _spriteRenderer = _placeholder.GetComponentInChildren<SpriteRenderer>();

                GameState.Instance.IsBuilding = true;
                _cost = _placeholder.GetComponentInChildren<TowerBase>().Cost;
                GameState.Instance.UpdateCost(_cost);
                TowerDetails.UpdateTarget(_placeholder, false);
            }
            else
            {
                _name = null;
                GameState.Instance.IsBuilding = false;
                GameState.Instance.UpdateCost(null);
                TowerDetails.UpdateTarget(null, false);
            }

            CurrentNameText.text = _name ?? "";
        }

        private GameObject CreatePlaceholder(GameObject tower, Vector2 position, Transform parent = null)
        {
            var placeholder = Instantiate(tower, position, Quaternion.identity, parent);
            placeholder.GetComponentInChildren<BoxCollider2D>().isTrigger = true;
            foreach (var circleCollider in placeholder.GetComponentsInChildren<CircleCollider2D>())
            {
                circleCollider.enabled = false;
            }
            placeholder.GetComponentInChildren<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            placeholder.GetComponentInChildren<Attack>().enabled = false;
            return placeholder;
        }

        private Vector2 GetMousePosition()
        {
            return Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        private bool IsValid()
        {
            if (GameState.Instance.IsGameOver ||
                GameState.Instance.IsWaveActive ||
                _cost > GameState.Instance.Gold ||
                _placeholder.transform.position.x > GameState.MapSize.x ||
                _placeholder.transform.position.x < -GameState.MapSize.x ||
                _placeholder.transform.position.y > GameState.MapSize.y ||
                _placeholder.transform.position.y < -GameState.MapSize.y)
            {
                return false;
            }
            var hit = Physics2D.OverlapBox(_placeholder.transform.position, _placeholder.transform.localScale / 2f, 0f, 1 << 30);
            return hit == null;
        }

        private void UpdateBuildMenu()
        {
            if (_prevName == _name && Math.Abs(_prevNameWidth - CurrentNameText.rectTransform.rect.width) < 0.000001f)
            {
                return;
            }

            _prevName = _name;
            _prevNameWidth = CurrentNameText.rectTransform.rect.width;

            var position = new Vector2(_buildMenuInitialPosition.x, _buildMenuInitialPosition.y);
            for (var i = 0; i < _buildMenuTowers.Count; i++)
            {
                _buildMenuTowers[i].transform.localPosition = position;
                position.x += _buildMenuScale.x + 10f;

                var color = Color.white;
                if (_name == _buildMenuNames[i])
                {
                    CurrentNameText.rectTransform.anchoredPosition = new Vector2((_buildMenuScale.x + 10f) * (i + 1), 0f);
                    position.x += _prevNameWidth + 10f;
                    color = Colors.Instance.Green;
                }

                _buildMenuSpriteRenderers[i].color = color;
            }
        }
    }
}
