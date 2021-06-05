using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class Interaction : MonoBehaviour
    {
        public event EventHandler OnClick;

        [UsedImplicitly]
        private void OnMouseDown()
        {
            OnClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
