using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Tower
{
    public class Build : MonoBehaviour
    {
        public GameObject[] Towers;
        public GameObject BuildMenu;

        public Text CostText;
        public Text CurrentNameText;

        public Color ValidColor;
        public Color ValidCostColor;
        public Color InvalidColor;

        private GameState _gameState;

        private readonly Dictionary<KeyCode, GameObject> _towers = new Dictionary<KeyCode, GameObject>();
        private GameObject _tower;
        private GameObject _placeholder;
        private string _name;
        private SpriteRenderer[] _spriteRenderers;
        private int _cost;

        private readonly List<GameObject> _buildMenuTowers = new List<GameObject>();
        private readonly List<string> _buildMenuNames = new List<string>();
        private readonly List<SpriteRenderer[]> _buildMenuSpriteRenderers = new List<SpriteRenderer[]>();
        private Vector2 _buildMenuScale;
        private Vector2 _buildMenuInitialPosition;
        private bool _updateBuildMenu;

        private void Start()
        {
            _gameState = GameState.GetGameState(gameObject);

            for (var i = 0; i < Towers.Length; i++)
            {
                _towers[KeyCode.Alpha1 + i] = Towers[i];
            }

            var buildMenuParent = BuildMenu.transform.parent;
            _buildMenuScale = 0.75f * new Vector2(1f / buildMenuParent.localScale.x, 1f / buildMenuParent.localScale.y);
            BuildMenu.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (_buildMenuScale.x + 10f) * Towers.Length - 10f);
            _buildMenuInitialPosition = new Vector2(BuildMenu.GetComponent<RectTransform>().rect.xMin + _buildMenuScale.x / 2f, 0f);

            foreach (var tower in Towers)
            {
                var buildMenuTower = Instantiate(tower, Vector2.zero, Quaternion.identity, BuildMenu.transform);
                buildMenuTower.transform.localScale = _buildMenuScale;
                _buildMenuTowers.Add(buildMenuTower);
                _buildMenuNames.Add(buildMenuTower.GetComponentInChildren<TowerBase>().Name);
                _buildMenuSpriteRenderers.Add(buildMenuTower.GetComponentsInChildren<SpriteRenderer>());
            }

            UpdateCost(null);
            UpdateBuildMenu();
        }

        private void Update()
        {
            if (_updateBuildMenu)
            {
                _updateBuildMenu = false;
                UpdateBuildMenu();
            }

            foreach (var keyCode in _towers.Keys.Where(Input.GetKeyDown))
            {
                if (_placeholder != null)
                {
                    Destroy(_placeholder);
                }

                var tower = _towers[keyCode];
                if (_name != tower.GetComponentInChildren<TowerBase>().Name)
                {
                    _tower = tower;
                    _placeholder = Instantiate(tower, GetMousePosition(), Quaternion.identity);
                    _placeholder.GetComponentInChildren<BoxCollider2D>().enabled = false;
                    _placeholder.GetComponentInChildren<Attack>().enabled = false;

                    _name = _placeholder.GetComponentInChildren<TowerBase>().Name;
                    _spriteRenderers = _placeholder.GetComponentsInChildren<SpriteRenderer>();
                    UpdateCost(_placeholder.GetComponentInChildren<TowerBase>().Cost);
                }
                else
                {
                    _name = null;
                    UpdateCost(null);
                }

                CurrentNameText.text = _name;
                _updateBuildMenu = true;
                break;
            }

            if (_placeholder != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (IsValid() && _gameState.HasPath(_placeholder.transform.position))
                    {
                        Instantiate(_tower, _placeholder.transform.position, Quaternion.identity);
                        Destroy(_placeholder);
                        _name = null;

                        _gameState.UpdateGold(-_cost);
                        UpdateCost(null);

                        CurrentNameText.text = null;
                        _updateBuildMenu = true;
                    }
                }

                _placeholder.transform.position = GetMousePosition();
                foreach (var spriteRenderer in _spriteRenderers)
                {
                    spriteRenderer.color = IsValid() ? ValidColor : InvalidColor;
                }
            }

            CostText.color = _cost > _gameState.Gold ? InvalidColor : ValidCostColor;
        }

        private Vector2 GetMousePosition()
        {
            return Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        private bool IsValid()
        {
            if (_gameState.IsGameOver ||
                _gameState.IsWaveActive ||
                _cost > _gameState.Gold ||
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

        private void UpdateCost(int? cost)
        {
            if (cost != null)
            {
                _cost = cost.Value;
                CostText.text = "-" + _cost;
            }
            else
            {
                CostText.text = "";
            }
        }

        private void UpdateBuildMenu()
        {
            var position = new Vector2(_buildMenuInitialPosition.x, _buildMenuInitialPosition.y);
            for (var i = 0; i < _buildMenuTowers.Count; i++)
            {
                _buildMenuTowers[i].transform.localPosition = position;
                position.x += _buildMenuScale.x + 10f;

                var color = Color.white;
                if (_name == _buildMenuNames[i])
                {
                    CurrentNameText.rectTransform.anchoredPosition = new Vector2((_buildMenuScale.x + 10f) * (i + 1), 0f);
                    position.x += CurrentNameText.rectTransform.rect.width + 10f;
                    color = ValidColor;
                }

                foreach (var spriteRenderer in _buildMenuSpriteRenderers[i])
                {
                    spriteRenderer.color = color;
                }
            }
        }
    }
}
