using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAnimator : MonoBehaviour
{
    public Animator animator;

    public void AnimatorDisable()
    {
        animator.enabled = false;
    }
}
