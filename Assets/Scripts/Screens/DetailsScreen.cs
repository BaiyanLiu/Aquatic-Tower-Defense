using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public abstract class DetailsScreen<T> : MonoBehaviour where T : MonoBehaviour
    {
        public RectTransform Screen;
        public Text NameText;

        protected GameState GameState { get; private set; }
        protected float InitialHeight { get; private set; }
        protected GameObject Target { get; private set; }
        protected T Base { get; private set; }

        private void Start()
        {
            GameState = GameState.GetGameState(gameObject);
            InitialHeight = Screen.rect.height;
            OnStart();
        }

        protected virtual void OnStart() {}

        private void FixedUpdate()
        {
            if (Target == null)
            {
                return;
            }

            Screen.pivot = new Vector2(-0.1f, Target.transform.position.y >= 0f ? 1f : 0f);
            Screen.position = new Vector2(Target.transform.position.x, Target.transform.position.y);

            OnUpdate();
        }

        protected abstract void OnUpdate();

        public void UpdateTarget(GameObject target)
        {
            if (Target != null)
            {
                Screen.gameObject.SetActive(false);
                OnDeselected();
            }

            if (target != null && target != Target)
            {
                Target = target;
                Base = target.GetComponentInChildren<T>();
                Screen.gameObject.SetActive(true);
                OnSelected();
            }
            else
            {
                Target = null;
            }
        }

        protected virtual void OnDeselected() {}

        protected virtual void OnSelected() {}
    }
}
