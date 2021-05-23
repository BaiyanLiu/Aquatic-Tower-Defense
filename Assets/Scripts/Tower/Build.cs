using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Tower
{
    public class Build : MonoBehaviour
    {
        public GameObject[] Temps;

        public Text CostText;

        public Color ValidColor;
        public Color ValidCostColor;
        public Color InvalidColor;

        private GameState _gameState;

        private Dictionary<KeyCode, GameObject> _temps;
        private GameObject _current;
        private SpriteRenderer[] _spriteRenderers;
        private int _cost;

        private void Start()
        {
            _gameState = GameState.GetGameState(gameObject);

            _temps = new Dictionary<KeyCode, GameObject>();
            for (var i = 0; i < Temps.Length; i++)
            {
                _temps[KeyCode.Alpha1 + i] = Temps[i];
            }
            UpdateCost(null);
        }

        private void Update()
        {
            foreach (var keyCode in _temps.Keys.Where(Input.GetKeyDown))
            {
                if (_current != null)
                {
                    Destroy(_current);
                }
                _current = Instantiate(_temps[keyCode], GetMousePosition(), Quaternion.identity);
                _spriteRenderers = _current.GetComponentsInChildren<SpriteRenderer>();
                UpdateCost(_temps[keyCode].GetComponent<Temp>().Cost);
                break;
            }

            if (_current != null)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Destroy(_current);
                    UpdateCost(null);
                } 
                else if (Input.GetMouseButtonDown(0))
                {
                    if (IsValid() && _gameState.HasPath(_current.transform.position))
                    {
                        Instantiate(_current.GetComponent<Temp>().Tower, _current.transform.position, Quaternion.identity);
                        _gameState.UpdateGold(-_cost);
                        UpdateCost(null);
                        Destroy(_current);
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
    }
}
