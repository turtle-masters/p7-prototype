using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    [Tooltip("The first Prompt to be activated when the level is loaded.")]
    public Prompt initialPrompt;

    protected virtual void Awake()
    {
        // ...
    }

    protected virtual void Start()
    {
        if (initialPrompt != null) initialPrompt.Activate();

        // ...
    }

    /*protected virtual void Update()
    {
        // ...
    }*/

    // ...
}
