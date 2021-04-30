using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts
{
    public class BuildMenu : MonoBehaviour
    {
        public GameObject Tower;

        public Color ValidColor;
        public Color InvalidColor;

        private GameObject _current;
        private SpriteRenderer[] _spriteRenderers;

        private void Update()
        {
            if (_current == null)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    _current = Instantiate(Tower, GetMousePosition(), Quaternion.identity);
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
                    if (IsValid())
                    {
                        Instantiate(Tower.GetComponent<Build>().Tower, _current.transform.position, Quaternion.identity);
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
            if (_current.transform.position.x > GameState.LevelSize.x ||
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
