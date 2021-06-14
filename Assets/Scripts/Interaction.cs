using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class Interaction : MonoBehaviour
    {
        public event EventHandler<GameObject> OnClick;
        public event EventHandler<Vector2> OnMove;
        public event EventHandler OnMoveEnd;

        private bool _isMoving;
        private Vector2 _initialPosition;

        [UsedImplicitly]
        private void Update()
        {
            if (_isMoving)
            {
                OnMove?.Invoke(this, (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition) - _initialPosition);
            }
        }

        [UsedImplicitly]
        private void OnMouseDown()
        {
            OnClick?.Invoke(this, gameObject);
        }

        [UsedImplicitly]
        private void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1))
            {
                _isMoving = true;
                _initialPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (!Input.GetMouseButton(1))
            {
                _isMoving = false;
                OnMoveEnd?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
