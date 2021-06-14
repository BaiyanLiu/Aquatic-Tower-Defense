using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class Interaction : MonoBehaviour
    {
        public event EventHandler<GameObject> OnClick;
        public event EventHandler<Vector2> OnMove;
        public event EventHandler OnMoveStart; 
        public event EventHandler OnMoveEnd;
        public event EventHandler<GameObject> OnEnter;
        public event EventHandler OnExit;

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
                OnMoveStart?.Invoke(this, EventArgs.Empty);
            }
            else if (_isMoving && !Input.GetMouseButton(1))
            {
                _isMoving = false;
                OnMoveEnd?.Invoke(this, EventArgs.Empty);
            }
        }

        [UsedImplicitly]
        private void OnMouseEnter()
        {
            if (!_isMoving)
            {
                OnEnter?.Invoke(this, gameObject);
            }
        }

        [UsedImplicitly]
        private void OnMouseExit()
        {
            if (!_isMoving)
            {
                OnExit?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
