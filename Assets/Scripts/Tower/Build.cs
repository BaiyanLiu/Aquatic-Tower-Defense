using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Tower
{
    public class Build : MonoBehaviour
    {
        public GameObject[] Temps;
        public GameObject MenuParent;

        public Text CostText;
        public Text MenuNameText;

        public Color ValidColor;
        public Color ValidCostColor;
        public Color InvalidColor;

        private GameState _gameState;

        private readonly Dictionary<KeyCode, GameObject> _temps = new Dictionary<KeyCode, GameObject>();
        private GameObject _current;
        private string _currentName;
        private SpriteRenderer[] _spriteRenderers;
        private int _cost;

        private readonly List<GameObject> _menuTemps = new List<GameObject>();
        private readonly List<string> _menuNames = new List<string>();
        private readonly List<SpriteRenderer[]> _menuSpriteRenderers = new List<SpriteRenderer[]>();
        private Vector2 _menuScale;
        private Vector2 _menuPosition;
        private bool _updateMenu;

        private void Start()
        {
            _gameState = GameState.GetGameState(gameObject);

            for (var i = 0; i < Temps.Length; i++)
            {
                _temps[KeyCode.Alpha1 + i] = Temps[i];
            }

            var canvas = MenuParent.transform.parent;
            _menuScale = 0.75f * new Vector2(1f / canvas.localScale.x, 1f / canvas.localScale.y);
            MenuParent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (_menuScale.x + 10f) * Temps.Length - 10f);
            _menuPosition = new Vector2(MenuParent.GetComponent<RectTransform>().rect.xMin + _menuScale.x / 2f, 0f);

            foreach (var temp in Temps)
            {
                var menuTemp = Instantiate(temp, Vector2.zero, Quaternion.identity, MenuParent.transform);
                menuTemp.transform.localScale = _menuScale;
                _menuTemps.Add(menuTemp);
                _menuNames.Add(menuTemp.GetComponent<Temp>().Name);
                _menuSpriteRenderers.Add(menuTemp.GetComponentsInChildren<SpriteRenderer>());
            }

            UpdateCost(null);
            UpdateMenu();
        }

        private void Update()
        {
            if (_updateMenu)
            {
                _updateMenu = false;
                UpdateMenu();
            }

            foreach (var keyCode in _temps.Keys.Where(Input.GetKeyDown))
            {
                if (_current != null)
                {
                    Destroy(_current);
                }

                var currentPrefab = _temps[keyCode];
                if (_currentName != currentPrefab.GetComponent<Temp>().Name)
                {
                    _current = Instantiate(currentPrefab, GetMousePosition(), Quaternion.identity);
                    _currentName = _current.GetComponent<Temp>().Name;
                    _spriteRenderers = _current.GetComponentsInChildren<SpriteRenderer>();
                    UpdateCost(_current?.GetComponent<Temp>().Cost);
                }
                else
                {
                    _currentName = null;
                    UpdateCost(null);
                }

                MenuNameText.text = _currentName;
                _updateMenu = true;
                break;
            }

            if (_current != null)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (IsValid() && _gameState.HasPath(_current.transform.position))
                    {
                        Instantiate(_current.GetComponent<Temp>().Tower, _current.transform.position, Quaternion.identity);
                        Destroy(_current);
                        _currentName = null;

                        _gameState.UpdateGold(-_cost);
                        UpdateCost(null);

                        MenuNameText.text = null;
                        _updateMenu = true;
                    }
                }

                _current.transform.position = GetMousePosition();
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
                _current.transform.position.x > GameState.MapSize.x ||
                _current.transform.position.x < -GameState.MapSize.x ||
                _current.transform.position.y > GameState.MapSize.y ||
                _current.transform.position.y < -GameState.MapSize.y)
            {
                return false;
            }
            var hit = Physics2D.OverlapBox(_current.transform.position, _current.transform.localScale / 2f, 0f, 1 << 30);
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

        private void UpdateMenu()
        {
            var position = new Vector2(_menuPosition.x, _menuPosition.y);
            for (var i = 0; i < _menuTemps.Count; i++)
            {
                _menuTemps[i].transform.localPosition = position;
                position.x += _menuScale.x + 10f;

                var color = Color.white;
                if (_currentName == _menuNames[i])
                {
                    MenuNameText.rectTransform.anchoredPosition = new Vector2((_menuScale.x + 10f) * (i + 1), 0f);
                    position.x += MenuNameText.rectTransform.rect.width + 10f;
                    color = ValidColor;
                }

                foreach (var spriteRenderer in _menuSpriteRenderers[i])
                {
                    spriteRenderer.color = color;
                }
            }
        }
    }
}
