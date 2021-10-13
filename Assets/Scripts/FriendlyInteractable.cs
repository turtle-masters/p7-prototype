using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(MeshRenderer))]
//[RequireComponent(typeof(Task))]
public class FriendlyInteractable : Interactable
{
    public Material highlightMaterial;
    public Material hoverMaterial;
    public Material grabMaterial;
    [HideInInspector]
    public bool isActuallyHovering;

    private Task parentTask;

    protected override void Start()
    {
        if (this.highlightMaterial == null)
            this.highlightMaterial =    Resources.Load<Material>("YellowHue");
        if (this.hoverMaterial == null)
            this.hoverMaterial =        Resources.Load<Material>("GreenHue");

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
        this.ChangeMaterial(this.highlightMaterial);
        if (!this.gameObject.GetComponent<Task>().isGlowing) return;
        base.OnHandHoverBegin(new Hand());  // fake hand don't sue me Valve
    }

    public void DebugEnterHover()
    {
        Debug.Log("DebugEnterHover");
        this.OnHandHoverBegin(new Hand());
    }

    public void DebugExitHover()
    {
        Debug.Log("DebugExitHover");
        this.OnHandHoverEnd(new Hand());
    }

    public void DebugGrab()
    {
        Debug.Log("DebugGrab");
        if (this.parentTask.IsActive()) 
            this.parentTask.Grab(new Hand(), true);
    }

    public void DebugDrop()
    {
        Debug.Log("DebugDrop");
        this.parentTask.Drop(new Hand(), true);
    }

    private void ChangeMaterial(Material m)
    {
        if (m == null) return;
        Interactable.highlightMat = m;
    }

    protected override void OnHandHoverBegin(Hand hand)
    {
        if (!this.parentTask.IsActive()) return;

        //Debug.Log("OnHandHoverBegin was called!");

        this.isActuallyHovering = true;
        this.parentTask.EnterHover(hand);

        if (!this.parentTask.isGlowing) return;
        base.OnHandHoverEnd(hand);
        this.ChangeMaterial(this.hoverMaterial);
        base.OnHandHoverBegin(hand);
    }

    protected override void OnHandHoverEnd(Hand hand)
    {
        if (!this.parentTask.IsActive()) return;

        //Debug.Log("OnHandHoverEnd was called!");

        this.isActuallyHovering = false;
        this.parentTask.ExitHover(hand);

        if (!this.parentTask.isGlowing) return;
        base.OnHandHoverEnd(hand);
        this.ChangeMaterial(this.highlightMaterial);
        base.OnHandHoverBegin(hand);
    }

    protected override void OnAttachedToHand(Hand hand)
    {
        if (!this.parentTask.IsActive()) return;

        this.parentTask.Grab(hand);
        this.ChangeMaterial(this.grabMaterial);

        if (this.parentTask.IsMovable()) base.OnAttachedToHand(hand);
        //this.parentTask.Resolve();
    }

    protected override void OnDetachedFromHand(Hand hand)
    {
        if (!this.parentTask.IsActive()) return;

        this.parentTask.Drop(hand);
        this.ChangeMaterial(this.highlightMaterial);

        if (this.parentTask.IsMovable()) base.OnDetachedFromHand(hand);
    }
}
