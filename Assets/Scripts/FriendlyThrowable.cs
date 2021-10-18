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
        this.parentTask = this.GetComponent<Task>();
        parentTask.OnResolve.AddListener(
            (Prompt p) => this.OnDetachedFromHand()
        );
    }

    protected override void HandHoverUpdate(Hand hand)
    {
        Debug.Log(this.gameObject.name + "->HandHoverUpdate->" + hand.name);

        GrabTypes gt = hand.GetGrabStarting();

        Debug.Log(this.parentTask);
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
        Debug.Log(this.gameObject.name + "->OnDetachedFromHand");

        if (this.parentTask != null && this.parentTask.target != null)
        {
            this.attachedHand.DetachObject(this.gameObject, this.restoreOriginalParent);
            base.OnDetachedFromHand(this.attachedHand);
        }
        this.attachedHand = null;
    }

    protected override void HandAttachedUpdate(Hand hand)
    {
        Debug.Log(this.gameObject.name + "->HandAttachedUpdate" + hand.name);

        //base.HandAttachedUpdate(hand);

        if (this.onHeldUpdate != null)
            this.onHeldUpdate.Invoke(hand);
    }
}
