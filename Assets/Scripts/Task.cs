using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(FriendlyInteractable))]
public class Task : Prompt
{
    [Tooltip("Whether this GameObject glows when activated.")]
    public bool isGlowing = true;
    [Tooltip("Whether this GameObject can be moved by the player.")]
    public bool isMovable = false;

    /*protected override void Awake()
    {
        base.Awake();
    }*/

    protected override void Start()
    {
        base.Start();

        //this.OnActive.AddListener(p => this.gameObject.GetComponent<FriendlyInteractable>().onAttachedToHand += this.Grab);
        //this.OnActive.AddListener(p => this.gameObject.GetComponent<FriendlyInteractable>().onDetachedFromHand += this.Drop);
    }

    /*protected virtual void Update()
    {
        // ...
    }*/

    protected override void OnPlaybackEnd()
    {
        if (this.GetAudioClip() != null && this.isLooping)
            this.PlaySound();  // keep playing sound effect recursively if there is one to be played
        //else this.TurnOff();
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
    public void Grab(Hand hand)
    {
        // the player has grabbed this GameObject at this point...
        this.Resolve();  // TODO: add more logic to this...
    }

    /*
     * Called when the player lets go of the associated GameObject
     */
    public void Drop(Hand hand)
    {
        // the player has released this GameObject at this point...
    }


    protected override void TurnOn()
    {
        base.TurnOn();
        this.gameObject.GetComponent<FriendlyInteractable>().Activate();
    }

    protected override void TurnOff()
    {
        base.TurnOff();

        // ...
    }
}
