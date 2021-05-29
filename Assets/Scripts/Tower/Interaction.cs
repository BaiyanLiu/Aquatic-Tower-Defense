using System;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class Interaction : MonoBehaviour
    {
        public event EventHandler OnClick;

        private void OnMouseDown()
        {
            OnClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
