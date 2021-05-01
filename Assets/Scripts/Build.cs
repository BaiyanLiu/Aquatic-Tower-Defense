using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts
{
    public class Build : MonoBehaviour
    {
        public GameObject Temp;

        public Color ValidColor;
        public Color InvalidColor;

        private GameState _gameState;

        private GameObject _current;
        private SpriteRenderer[] _spriteRenderers;

        private void Start()
        {
            _gameState = GameState.GetGameState(gameObject);
        }

        private void Update()
        {
            if (_current == null)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    _current = Instantiate(Temp, GetMousePosition(), Quaternion.identity);
                    _spriteRenderers = _current.GetComponentsInChildren<SpriteRenderer>();
                }
            } 
            else if (_current != null)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Destroy(_current);
                } 
                else if (Input.GetMouseButtonDown(0))
                {
                    if (IsValid() && _gameState.HasPath(_current.transform.position))
                    {
                        Instantiate(Temp.GetComponent<Temp>().Tower, _current.transform.position, Quaternion.identity);
                        Destroy(_current);
                    }
                }
            }

            if (_current != null)
            {
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
            if (_gameState.IsStarted ||
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
