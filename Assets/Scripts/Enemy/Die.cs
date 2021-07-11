using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    [UsedImplicitly]
    public sealed class Die : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.transform.parent.gameObject, stateInfo.length);
        }
    }
}
