using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

//[RequireComponent(typeof(Interactable))]
public class Task : Prompt
{
    [Tooltip("Whether this GameObject glows when activated.")]
    public bool isGlowing = true;
    [HideInInspector]
    [Tooltip("Whether this GameObject can be moved by the player.")]
    public bool isMovable = false;

    private Interactable localInteractable;

    protected override void Awake()
    {
        base.Awake();

        // make some settings in Interactable right off the bat
        this.localInteractable = this.gameObject.AddComponent<Interactable>();
        this.localInteractable.highlightOnHover = false;  // we'll handle our own highlight (glowing)
    }

    protected override void Start()
    {
        base.Start();

        this.OnActive.AddListener(p => localInteractable.onAttachedToHand += Interact);
    }

    protected override void Update()
    {
        base.Update();

        // ...
    }

    public void EnterHover()
    {
        // ...
    }

    public void ExitHover()
    {
        // ...
    }

    /*
     * Call when the player grabs the associated GameObject
     * To be used by DebugPlayer
     */
    public void Interact()
    {
        // ...
    }

    /*
     * Call when the player grabs the associated GameObject
     * To be used by SteamVR
     */
    public void Interact(Hand hand)
    {
        this.Interact(hand.gameObject);
    }

    private void Interact(GameObject gameObject)
    {
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
}
