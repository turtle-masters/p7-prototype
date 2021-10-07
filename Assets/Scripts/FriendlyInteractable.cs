using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Task))]
public class FriendlyInteractable : Interactable
{
    public Material highlightMaterial;
    public Material hoverMaterial;
    public Material grabMaterial;

    private Task parentTask;

    protected override void Start()
    {
        base.Start();

        this.parentTask = this.gameObject.GetComponent<Task>();
        this.ChangeMaterial(highlightMaterial);
    }

    protected override void Update()
    {
        if (this.parentTask.IsActive())
        {
            base.Update();
        }
    }

    public void Activate()
    {
        this.OnHandHoverBegin(new Hand());  // fake hand don't sue me Valve
    }

    private void ChangeMaterial(Material m)
    {
        if (m == null) return;
        Interactable.highlightMat = m;
    }

    protected override void OnHandHoverBegin(Hand hand)
    {
        this.gameObject.GetComponent<Task>().EnterHover();
        this.ChangeMaterial(this.hoverMaterial);
        base.OnHandHoverBegin(hand);
    }

    protected override void OnHandHoverEnd(Hand hand)
    {
        this.gameObject.GetComponent<Task>().ExitHover();
        this.ChangeMaterial(this.highlightMaterial);
        base.OnHandHoverEnd(hand);
    }

    protected override void OnAttachedToHand(Hand hand)
    {
        this.gameObject.GetComponent<Task>().Grab(hand);
        this.ChangeMaterial(this.grabMaterial);
        base.OnAttachedToHand(hand);
    }

    protected override void OnDetachedFromHand(Hand hand)
    {
        this.gameObject.GetComponent<Task>().Drop(hand);
        this.ChangeMaterial(this.highlightMaterial);
        base.OnDetachedFromHand(hand);
    }
}
