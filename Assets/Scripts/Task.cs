using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(FriendlyInteractable))]
public class Task : Prompt
{
    [Tooltip("Whether this GameObject glows when activated.")]
    public bool isGlowing = true;
    [HideInInspector]
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

    public void EnterHover()
    {
        // ...
    }

    public void ExitHover()
    {
        // ...
    }

    /*
     * Called when the player grabs the associated GameObject
     */
    public void Grab(Hand hand)
    {
        // the player has grabbed this GameObject at this point...
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
