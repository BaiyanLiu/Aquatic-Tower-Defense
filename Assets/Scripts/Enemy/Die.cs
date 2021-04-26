using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class Die : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Destroy(animator.gameObject, stateInfo.length);
        }
    }
}
