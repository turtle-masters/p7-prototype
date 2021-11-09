using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroOutro : MonoBehaviour
{
    public Animator animator;
    public string anim;
    GameObject transition;
    public Text text;
    public bool gamefinish = false;

    private void Awake()
    {
        transition = GameObject.Find("Transition");
        text = transition.GetComponentInChildren<Text>();
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
        text.text = "Loading";
        anim = "FOut";
        animator.SetTrigger(anim);
        if (gamefinish)
        {
            text.text = "Game Over";
            Application.Quit();
        }
    }

    public void fadeOut()
    {
        text.text = "Day " + (GameObject.Find("MinigameManager").GetComponent<MinigameManagerScript>().currentLevel + 1);
        anim = "FOut";
        animator.SetTrigger(anim);

    }

    public void finishGame()
    {
        gamefinish = true;
    }

    public void finishGame(Prompt p)
    {
        this.finishGame();
    }
}