using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Level : Logger
{
    [Tooltip("The first Prompt to be activated when the level is loaded.")]
    public Prompt initialPrompt;

    private static void Log(GameObject playerObject, string details)
    {
        // TODO: unwrap and track the Player object and hand off to base.Log
        // ...
    }

    protected override void Awake()
    {
        base.Awake();

        // ...
    }

    protected override void Start()
    {
        base.Start();
        if (initialPrompt != null) initialPrompt.Activate();

        // ...
    }

    protected override void Update()
    {
        base.Update();

        // ...
    }

    // ...
}
