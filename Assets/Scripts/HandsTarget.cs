using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Prompt))]
[RequireComponent(typeof(InteractionTarget))]
public class HandsTarget : FriendlyInteractable
{
    Prompt localPrompt;

    private new void Start()
    {
        base.Start();

        this.localPrompt = this.GetComponent<Prompt>();
        this.GetComponent<Collider>().enabled = true;
    }

    protected override void OnHandHoverBegin(Hand hand)
    {
        if (this.localPrompt.IsActive())
            this.localPrompt.Resolve();
        else base.OnHandHoverBegin(hand);
    }
}
