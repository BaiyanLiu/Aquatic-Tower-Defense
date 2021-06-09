using System.Collections.Generic;
using Assets.Scripts.Effect;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Screens
{
    public abstract class DetailsScreen<T> : MonoBehaviour where T : MonoBehaviour
    {
        public RectTransform Screen;
        public Transform EffectsParent;
        public Text NameText;

        protected GameState GameState { get; private set; }
        protected float InitialHeight { get; private set; }
        protected GameObject Target { get; private set; }
        protected T Base { get; private set; }
        protected virtual EffectBase[] TargetEffects { get; } = {};

        private readonly List<GameObject> _effects = new List<GameObject>();
        private readonly List<EffectDisplay> _effectDisplays = new List<EffectDisplay>();
        private readonly List<RectTransform> _effectTransforms = new List<RectTransform>();

        [UsedImplicitly]
        private void Start()
        {
            GameState = GameState.GetGameState(gameObject);
            InitialHeight = Screen.rect.height;

            if (EffectsParent != null)
            {
                for (var i = 0; i < EffectsParent.childCount; i++)
                {
                    var effect = EffectsParent.GetChild(i).gameObject;
                    _effects.Add(effect);
                    _effectDisplays.Add(effect.GetComponent<EffectDisplay>());
                    _effectTransforms.Add(effect.GetComponent<RectTransform>());
                }
            }
        }

        [UsedImplicitly]
        private void Update()
        {
            if (Target == null)
            {
                return;
            }

            var height = 0f;
            for (var i = 0; i < _effects.Count; i++)
            {
                if (i < TargetEffects.Length)
                {
                    _effectTransforms[i].anchoredPosition = new Vector2(0f, -height);
                    height += _effectDisplays[i].UpdateEffect(TargetEffects[i]) + 10f;
                    _effects[i].SetActive(true);
                }
                else
                {
                    _effects[i].SetActive(false);
                }
            }

            height = OnUpdate(height);

            Screen.pivot = new Vector2(-0.1f, Target.transform.position.y >= 0f ? 1f : 0f);
            Screen.position = new Vector2(Target.transform.position.x, Target.transform.position.y);
            Screen.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, InitialHeight + height);
        }

        protected abstract float OnUpdate(float height);

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
