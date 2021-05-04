using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts
{
    public class Build : MonoBehaviour
    {
        public GameObject[] Temps;

        public Color ValidColor;
        public Color InvalidColor;

        private GameState _gameState;

        private Dictionary<KeyCode, GameObject> _temps;
        private GameObject _current;
        private SpriteRenderer[] _spriteRenderers;

        private void Start()
        {
            _gameState = GameState.GetGameState(gameObject);

            _temps = new Dictionary<KeyCode, GameObject>();
            for (var i = 0; i < Temps.Length; i++)
            {
                _temps[KeyCode.Alpha1 + i] = Temps[i];
            }
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
                break;
            }

            if (_current != null)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Destroy(_current);
                } 
                else if (Input.GetMouseButtonDown(0))
                {
                    if (IsValid() && _gameState.HasPath(_current.transform.position))
                    {
                        Instantiate(_current.GetComponent<Temp>().Tower, _current.transform.position, Quaternion.identity);
                        Destroy(_current);
                    }
                }

                _current.transform.position = GetMousePosition();
                foreach (var spriteRenderer in _spriteRenderers)
                {
                    spriteRenderer.color = IsValid() ? ValidColor : InvalidColor;
                }
            }
        }

        private Vector2 GetMousePosition()
        {
            return Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        private bool IsValid()
        {
            if (_gameState.IsWaveActive ||
                _current.transform.position.x > GameState.LevelSize.x ||
                _current.transform.position.x < -GameState.LevelSize.x ||
                _current.transform.position.y > GameState.LevelSize.y ||
                _current.transform.position.y < -GameState.LevelSize.y)
            {
                return false;
            }
            var hit = Physics2D.OverlapBox(_current.transform.position, _current.transform.localScale / 2f, 0f, 1 << 30);
            return hit == null;
        }
    }
}
