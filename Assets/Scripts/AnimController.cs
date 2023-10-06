using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField]
    private float animStep;

    [SerializeField]
    private float maxModifier;

    private Animator animator;

    [SerializeField]
    private float animStepModifier;

    [SerializeField]
    private float loopPercent;

    private bool leftLegPushin;
    private bool gameStarted;
    private Dictionary<bool, string> animStates = new Dictionary<bool, string>()
    {
        { true, "Base.LeftLegPush" },
        { false, "Base.RightLegPush" }
    };

    private void Start()
    {
        leftLegPushin = true;
        animator = GetComponent<Animator>();
        animator.speed = 0;

        loopPercent = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    void FixedUpdate()
    {
        PlayAnimStep();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (gameStarted)
            {
                animStepModifier++;
                if (animStepModifier > maxModifier)
                {
                    animStepModifier = maxModifier;
                }
            }
            else
            {
                gameStarted = true;
                animator.Play(animStates[leftLegPushin], 0, 0.5f);
                StartCoroutine(DownModifier());
            }
        }
    }

    private void PlayAnimStep()
    {
        animator.Play(
            animStates[leftLegPushin],
            0,
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime + animStep * animStepModifier
        );

        loopPercent = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        if (loopPercent > 1)
        {
            leftLegPushin = !leftLegPushin;
            animator.Play(animStates[leftLegPushin], 0, 0);
        }
        if (loopPercent < 0)
        {
            StopAllCoroutines();
            animator.Play(animStates[leftLegPushin], 0, 0);
            animStepModifier = 0;
            Debug.Log("LOSE");
        }
    }

    private IEnumerator DownModifier()
    {
        while (true)
        {
            animStepModifier -= Time.fixedDeltaTime;
            if (animStepModifier < -maxModifier)
            {
                animStepModifier = -maxModifier;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
