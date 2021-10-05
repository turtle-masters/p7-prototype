using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ActiveEvent : UnityEvent<Prompt> { }
[Serializable]
public class ResolveEvent : UnityEvent<Prompt> { }

/**
 * The base class for audio/visual Player signalling and communications
 */
[RequireComponent(typeof(AudioSource))]
public class Prompt : Logger
{
    [Tooltip("A (preferably) unqiue and descriptive name to help identify this Prompt.")]
    public string promptName = "An Awesome Prompt";
    [HideInInspector]
    public string promptText = "Hello, Player!";    // I suspect the prompt text will be
    [HideInInspector]
    public bool animateText = false;                // very hard to implement properly ...

    [Tooltip("The playback volume range (0 to 1) of the audio clip to be played.")]
    public float playbackVolume = 1f;
    [Tooltip("Whether the audio clip should be played in the player's ear (omnidirectional) or coming from the associated GameObject.")]
    public bool isOmnidirectional = false;
    [Tooltip("Whether the audio clip should start again after it has finished playing (provided the Prompt is still active at that point).")]
    public bool isLooping = true;
    [Tooltip("The amount of seconds (as a float) to wait after playback has ended before continuing.")]
    public float secondsWaitAfterPlay = 1.5f;

    [Tooltip("Subscribe a method here to have it invoked whenever this Prompt is activated. This could be if you want something to happen simultanuously with this Prompt!")]
    public ActiveEvent OnActive = new ActiveEvent();
    [Tooltip("Subscribe a method here to have it invoked whenever this Prompt is resolved. This could be if you want another Prompt to fire after this one is done!")]
    public ResolveEvent OnResolve = new ResolveEvent();

    [HideInInspector]
    public static List<Prompt> activePrompts = new List<Prompt>();  // enumerating the raw List is discouraged, as asynchronous modification can occur

    protected Level parentLevel;
    protected GameObject playerObject;
    private bool isActive = false;

    protected override void Awake()
    {
        base.Awake();

        // make some settings in AudioSource right off the bat
        AudioSource localSource = this.gameObject.GetComponent<AudioSource>();
        localSource.playOnAwake = false;
    }

    protected override void Start()
    {
        base.Start();

        // try to retrieve the parent level immidiately
        // other attempts will be made if unsuccessful
        this.parentLevel = this.GetParentLevel();

        // find the player object
        try
        {
            this.playerObject = GameObject.FindGameObjectsWithTag("Player")[0];  // there should only ever be one GameObject tagged with "Player"
        }
        catch (IndexOutOfRangeException e)
        {
            Debug.LogError(this + " was unable to locate \"Player\"-tagged object. Is the Player object tagged with \"Player\"?" + e, this);
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    public override string ToString()
    {
        return "<" + this.promptName + "> " + base.ToString();
    }

    /*
     * Resolve all currently active Prompts
     */
    public static void ResolveAll()
    {
        // copy activePrompts to protect from asynchronous modification during enumeration
        List<Prompt> activePromptsCopy = new List<Prompt>(activePrompts);
        foreach (Prompt p in activePromptsCopy) p.Resolve();
    }

    /*
     * Toggle active/non-active state
     * It's probably not the best idea to use this, better to do it explicitly using Activate and Resolve
     */
    public void ToggleActive()
    {
        this.SetActive(!this.IsActive());
    }

    /*
     * Activate the Prompt
     * This can be called as often as you like; it'll only fire if the Prompt is currently inactive
     */
    public void Activate()
    {
        this.SetActive(true);
    }

    /*
     * Activate the Prompt, considering the given Prompt as the parent
     * This is not actually used for anything internally, and is only here to make chaining Prompts easier
     */
    public void Activate(Prompt parentPrompt)
    {
        this.SetActive(true);
    }

    /*
     * Resolve this Prompt
     * This can be called as often as you like; it'll only fire if the Prompt is currently active
     */
    public void Resolve()
    {
        this.SetActive(false);
    }

    /*
     * Whether this Prompt is currently active or not
     */
    public bool IsActive()
    {
        return this.isActive;
    }

    /*
     * Retrieves the level containing this Prompt in the game world, otherwise null
     * It is important that every Prompt is placed inside a GameObject with an active Level component
     */
    public Level GetParentLevel()
    {
        if (this.parentLevel == null)
        {
            // find parent Level
            this.parentLevel = GetComponentInParent<Level>();
            if (this.parentLevel == null) Debug.LogError(this + " was unable to get parent level", this);
        }
        return this.parentLevel;
    }

    private void SetActive(bool newState)
    {
        // make sure to not invoke events unless the state has actually changed
        if (newState != this.IsActive())
        {
            if (newState) this.TurnOn();
            else this.TurnOff();
        }
    }

    protected static void Log(Prompt p, string details)
    {
        // TODO: unwrap Prompt object and hand off to base.Log
        // ...
    }

    protected void TurnOn()
    {
        this.isActive = true;
        activePrompts.Add(this);
        if (this.OnActive != null) this.OnActive.Invoke(this);

        PlaySound();
    }

    private void PlaySound()
    {
        if (this.GetAudioClip() != null)
        {
            this.GetAudioSource().PlayOneShot(this.GetAudioClip(), this.playbackVolume);
            IEnumerator DelayedCallback(float time)
            {
                yield return new WaitForSeconds(time);
                if (this.IsActive())
                {
                    if (this.isLooping) PlaySound();  // keep playing sound effect recursively if set
                    else this.TurnOff();
                }
            }
            StartCoroutine(DelayedCallback(this.GetAudioClip().length + this.secondsWaitAfterPlay));
        }
        else
        {
            Debug.LogWarning(this + " attempted to play audio but no clip was given", this);
            if (!this.isLooping) this.TurnOff();
        }
    }

    protected void TurnOff()
    {
        this.isActive = false;
        activePrompts.Remove(this);
        
        this.GetAudioSource().Stop();

        if (this.OnActive != null) this.OnResolve.Invoke(this);
        else Debug.LogWarning(this + " OnResolve was invoked with no subscribed listeners. Is the event chain supposed to end here?", this);
    }

    private AudioSource GetAudioSource()
    {
        if (this.isOmnidirectional && this.playerObject != null)
        {
            AudioSource source = playerObject.GetComponentInChildren<AudioSource>();
            if (source != null) return source;
            else Debug.LogError(this + " was unable to locate audio source on \"Player\"-tagged object in Prompt. Does the Player object have an AudioSource component?", this);
        }
        return this.gameObject.GetComponent<AudioSource>();
    }

    private AudioClip GetAudioClip()
    {
        return this.gameObject.GetComponent<AudioSource>().clip;
    }
}
