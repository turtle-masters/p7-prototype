using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class FriendlyThrowable : Throwable
{
    Task parentTask;
    Hand attachedHand;

    protected void Start()
    {
        Task parentTask = this.GetComponent<Task>();
        parentTask.OnResolve.AddListener(
            (Prompt p) => this.OnDetachedFromHand()
        );
    }

    protected override void HandHoverUpdate(Hand hand)
    {
        GrabTypes gt = hand.GetGrabStarting();

        if (gt != GrabTypes.None && this.parentTask.IsActive())
        {
            this.attachedHand = hand;
            this.parentTask.Grab(hand);
            if (this.parentTask.target != null)
                base.HandHoverUpdate(hand);
        }
    }

    protected void OnDetachedFromHand()
    {
        if (this.parentTask != null && this.parentTask.target != null)
        {
            this.attachedHand.DetachObject(this.gameObject, this.restoreOriginalParent);
            base.OnDetachedFromHand(this.attachedHand);
        }
        this.attachedHand = null;
    }

    protected override void HandAttachedUpdate(Hand hand)
    {
        //base.HandAttachedUpdate(hand);

        if (this.onHeldUpdate != null)
            this.onHeldUpdate.Invoke(hand);
    }
}
