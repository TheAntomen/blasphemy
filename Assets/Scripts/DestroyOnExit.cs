using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroy object after animation
/// </summary>
public class DestroyOnExit : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.gameObject != null)
        {
            Destroy(animator.gameObject, stateInfo.length);
        }

    }
}