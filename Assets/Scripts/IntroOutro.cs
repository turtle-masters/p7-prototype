using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroOutro : MonoBehaviour
{
    public Animator animator;
    public string anim;
    GameObject transition;

    private void Awake()
    {
        transition = GameObject.Find("Transition");
        animator = transition.GetComponent<Animator>();
        fadeIn();
    }

    public void fadeIn()
    {
        Debug.Log("called");
        anim = "FIn";
        animator.SetTrigger(anim);
    }

    public void fadeOut(Prompt p)
    {
        anim = "FOut";
        animator.SetTrigger(anim);

    }

    public void fadeOut()
    {
        anim = "FOut";
        animator.SetTrigger(anim);

    }


}