using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

[Serializable]
public class GrabEvent : UnityEvent<Prompt> { }

[RequireComponent(typeof(FriendlyInteractable))]
public class Task : Prompt
{
    [Tooltip("Make GameObject glow when activated.")]
    public bool isGlowing = true;
    [Tooltip("If set, the Task will only resolve when the associated GameObject is moved to the same location as the target transform. Setting this also makes the Task movable by the Player.")]
    public InteractionTarget target;
    [Tooltip("The amount of diviation on all axis (in Unity units) allowed from the target transform before it is considered hit.")]
    public float targetPrecision = 0.2f;
    [Tooltip("When enabled, the Task object must match both the position AND the rotation of the target.")]
    public bool matchRotation = false;
    [Tooltip("Disable the mesh and interactability of the Task object after the Task is completed.")]
    public bool hideAfterCompletion = true;

    [Tooltip("Invoked whenever this Task is grabbed. Use this to activate narrative Prompts!")]
    public ActiveEvent OnGrab = new ActiveEvent();

    protected override void Start()
    {
        if (this.target != null) this.gameObject.AddComponent<Rigidbody>();
        base.Start();
    }

    protected virtual void Update()
    {
        // TODO: check for position in relation to target if defined and grabbed...
        if (this.target != null && this.IsActive() /*&& this.gameObject.GetComponent<FriendlyInteractable>().attachedToHand != null*/)
        {
            Transform selfTransform = this.transform;
            Transform targetTransform = this.target.transform;

            // check position
            Vector3 positionDifference = selfTransform.position - targetTransform.position;
            bool validPosition = true;
            for (int i = 0; i < 3; i++)
                if (Mathf.Abs(positionDifference[i]) > this.targetPrecision) validPosition = false;

            bool validRotation = true;  // placeholder
            // TODO: check for rotation aswell...
            if (this.matchRotation && !compareRotation(selfTransform.rotation,targetTransform.rotation,0.15f))
            {
                validRotation = false;
            }
            // report the product of both checks
            if (validPosition && validRotation) this.Resolve();
        }
    }
    bool compareRotation(Quaternion r1, Quaternion r2, float precision)
    {
        return Mathf.Abs(Quaternion.Dot(r1, r2)) >= 1 - precision;

    }
    protected override void OnPlaybackEnd()
    {
        if (this.GetAudioClip() != null && this.isLooping)
            this.PlaySound();  // keep playing sound effect recursively if there is one to be played
        //else this.TurnOff();
    }

    public bool IsMovable()
    {
        return this.target != null;
    }

    public void EnterHover(Hand hand)
    {

    }

    public void ExitHover(Hand hand)
    {

    }

    /*
     * Called when the player grabs the associated GameObject
     */
    public void Grab(Hand hand, bool debugAutoComplete = false)
    {
        // the player has grabbed this GameObject...
        if (!this.target || debugAutoComplete)
            this.Resolve();
        else
        {
            this.target.Activate();
            this.OnGrab.Invoke(this);
        }
    }

    /*
     * Called when the player lets go of the associated GameObject
     */
    public void Drop(Hand hand, bool debugAutoComplete = false)
    {
        // the player has released this GameObject...
        //if (this.target) this.target.Resolve();
    }

    public new void Resolve()
    {
        if (this.target)
        {
            this.target.Resolve();
            this.GetComponent<Rigidbody>().isKinematic = false;
            if (hideAfterCompletion)
            {
                this.GetComponent<FriendlyInteractable>().enabled = false;
                this.GetComponent<Renderer>().enabled = false;
            }
        }
        base.Resolve();
    }

    protected override void TurnOn()
    {
        base.TurnOn();
        this.gameObject.GetComponent<FriendlyInteractable>().Activate();
    }

    protected override void TurnOff()
    {
        base.TurnOff();
        this.gameObject.GetComponent<FriendlyInteractable>().DebugExitHover();
    }
}
