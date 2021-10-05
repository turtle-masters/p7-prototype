using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable))]
public class Task : Prompt
{
    [HideInInspector]
    public bool isGlowing = true;

    protected override void Start()
    {
        base.Start();
        // ...
    }

    protected override void Update()
    {
        base.Update();
        // ...
    }

    private new void TurnOn()
    {
        base.TurnOn();
        // ...
    }

    private new void TurnOff()
    {
        base.TurnOff();
        // ...
    }

    // ...
}
