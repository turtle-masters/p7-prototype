using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Prompt))]
public class MicroveseLevelSelect : MonoBehaviour
{
    public int levelIndex;

    void Start()
    {
        this.GetComponent<Prompt>().OnActive.AddListener((Prompt p) =>
            MinigameManagerScript.instance.SetCurrentLevel(this.levelIndex));
    }
}
