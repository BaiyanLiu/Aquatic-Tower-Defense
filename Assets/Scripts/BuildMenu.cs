using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts
{
    public class BuildMenu : MonoBehaviour
    {
        public GameObject Tower;

        private GameObject _current;

        private void Update()
        {
            if (_current == null)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    _current = Instantiate(Tower, GetMousePosition(), Quaternion.identity);
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
                    Instantiate(Tower.GetComponent<Build>().Tower, _current.transform.position, Quaternion.identity);
                    Destroy(_current);
                }
            }

            if (_current != null)
            {
                _current.transform.position = GetMousePosition();
            }
        }

        private Vector2 GetMousePosition()
        {
            return Vector2Int.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
