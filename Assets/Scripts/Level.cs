using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Linq;

public class Level : MonoBehaviour
{
    [Tooltip("The first Prompt to be activated when the level is loaded.")]
    public Prompt entryPrompt;
    
    [HideInInspector]
    public static GameObject globalPlayerObject;
    [HideInInspector]
    public bool isActive;
    [HideInInspector]
    public static Level activeLevel;
    [HideInInspector]
    public static int totalSceneChanges = 0;

    private static bool playerInitiated = false;
    private static List<int> culledScenes = new List<int>();

    private Level[] levelsInScene;

    static Level()
    {
        //SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;

        /*GameObject[] objects = SceneManager.GetSceneByBuildIndex(0).GetRootGameObjects();
        foreach (GameObject gameObject in objects)
        {
            Debug.Log(gameObject.name);
            if (gameObject.tag == "Player")
                globalPlayerObject = gameObject;
        }
        Debug.Log(globalPlayerObject);*/
    }

    private void OnEnable()
    {
        this.SetVisibilityOfAllChildren(false);
    }

    private void Start()
    {
        // find all Level components in the Scene
        levelsInScene = new Level[4];
        IEnumerator ie = SceneManager.GetActiveScene().GetRootGameObjects().GetEnumerator();
        ie.Reset();
        while (ie.MoveNext())
        {
            GameObject currentObject = (GameObject)ie.Current;
            if (currentObject != null && currentObject.GetComponent<Level>())
                //levelsInScene.Add(currentObject.GetComponent<Level>());
                switch (currentObject.name)
                {
                    case "Level1":
                        levelsInScene[0] = currentObject.GetComponent<Level>();
                        if (currentObject.activeSelf) Level.activeLevel = levelsInScene[0];
                        break;
                    case "Level2":
                        levelsInScene[1] = currentObject.GetComponent<Level>();
                        if (currentObject.activeSelf) Level.activeLevel = levelsInScene[1];
                        break;
                    case "Level3":
                        levelsInScene[2] = currentObject.GetComponent<Level>();
                        if (currentObject.activeSelf) Level.activeLevel = levelsInScene[2];
                        break;
                    case "Level4":
                        levelsInScene[3] = currentObject.GetComponent<Level>();
                        if (currentObject.activeSelf) Level.activeLevel = levelsInScene[3];
                        break;
                }
        }
        Debug.Log(Level.activeLevel.gameObject.name);
    }

    private void Update()
    {
        UpdateActiveScene();
    }

    private static void CullPlayerObjects()
    {
        //Debug.Log(SceneManager.sceneCount);
        //Debug.Log(!playerInitiated && SceneManager.GetActiveScene().buildIndex != 0);
        if (!playerInitiated && SceneManager.GetActiveScene().buildIndex == 0)
            return;

        bool hasEncounteredPlayer = false;
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            //Debug.Log("Looking at Scene " + scene.name);

            /*if (Level.culledScenes.Contains(scene.buildIndex))
                continue;*/

            IEnumerator rootEnumerator = scene.GetRootGameObjects().GetEnumerator();
            rootEnumerator.Reset();
            //GameObject playerGameObject = Level.globalPlayerObject;

            while (rootEnumerator.MoveNext())
            {
                GameObject current = (GameObject)rootEnumerator.Current;
                if (current.tag == "Player")
                {
                    if (scene.name == "Microverse" && !hasEncounteredPlayer)
                        hasEncounteredPlayer = true;
                    else
                    {
                        Debug.Log("Found Player in Scene " + scene.name);
                        if (!DebugPlayer.isActive)
                        {
                            current.SetActive(false);
                            //culledScenes.Add(scene.buildIndex);
                        }

                        break;
                    }
                }
            }
            //Debug.Log("Number of culled Scenes: " + Level.culledScenes.Count);
        }

        playerInitiated = true;
    }

    private void UpdateActiveScene()
    {   
        if (Input.GetKeyDown("1") && !Level.activeLevel.Equals(this.levelsInScene[0])) {
            this.SetVisibilityOfAllChildren(false);
            Level.activeLevel = this.levelsInScene[0];
            Level.activeLevel.SetVisibilityOfAllChildren(true);
        }
        else if (Input.GetKeyDown("2") && !Level.activeLevel.Equals(this.levelsInScene[1])) {
            this.SetVisibilityOfAllChildren(false);
            Level.activeLevel = this.levelsInScene[1];
            Level.activeLevel.SetVisibilityOfAllChildren(true);
        }
        else if (Input.GetKeyDown("3") && !Level.activeLevel.Equals(this.levelsInScene[2])) {
            this.SetVisibilityOfAllChildren(false);
            Level.activeLevel = this.levelsInScene[2];
            Level.activeLevel.SetVisibilityOfAllChildren(true);
        }
        else if (Input.GetKeyDown("4") && !Level.activeLevel.Equals(this.levelsInScene[3]))
        {
            this.SetVisibilityOfAllChildren(false);
            Level.activeLevel = this.levelsInScene[3];
            Level.activeLevel.SetVisibilityOfAllChildren(true);
        }
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ...
    }

    private static void OnSceneUnloaded(Scene scene)
    {
        // ...
    }

    private static void Continue()
    {
        try
        {
            Scene nextScene = SceneManager.GetSceneByName(Level.GetNextSceneName());
            if (nextScene != null && nextScene.isLoaded)  // these two conditions are probably the same thing...
                SceneManager.SetActiveScene(nextScene);
            else
                // load next level (and scene) if async load didn't work for some reason
                SceneManager.LoadScene(Level.GetNextSceneName());
        }
        catch (System.InvalidOperationException e)
        {
            // we just pretend the level changed here
            Level.totalSceneChanges++;
            Debug.LogWarning("Level tried to continue to next level but failed: " + e);
        }
    }

    private static IEnumerator LoadNextLevel(string sceneName)
    {
        // use totalSceneLoads to figure out what scene to load
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;
        
        // wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
            yield return null;
    }

    private static string GetNextSceneName()
    {
        switch (Level.totalSceneChanges)
        {
            // first scene (tutorial) will be loaded automatically
            case 1:
            case 3:
            case 5:
            case 7:
                return "Village";
            case 2:
            case 4:
            case 6:
                return "Microverse";
            default:  // 8
                return "Room";
        }
    }

    private void SetVisibilityRecursively(GameObject node, bool isVisible, bool keepDisabled = false)
    {
        if (node.GetComponent<InteractionTarget>() != null) 
            keepDisabled = true;
        else if (node.GetComponent<Task>() != null && node.GetComponent<Task>().hideUntilActive)
            keepDisabled = true;

        /*if (isVisible) node.layer = 0;
        else node.layer = 1;*/

        if (!keepDisabled || node.GetComponent<InteractionTarget>() == null || !isVisible)
        {
            if (node.GetComponent<Renderer>() != null)
                node.GetComponent<Renderer>().enabled = isVisible;
            if (node.GetComponent<Collider>() != null)
                node.GetComponent<Collider>().enabled = isVisible;
        }

        for (int i = 0; i < node.transform.childCount; i++)
            this.SetVisibilityRecursively(node.transform.GetChild(i).gameObject, isVisible, keepDisabled);

        if (node.GetComponent<Renderer>() != null && keepDisabled) 
            node.GetComponent<Renderer>().enabled = false;

        if (node.GetComponent<Canvas>() != null)
            node.GetComponent<Canvas>().enabled = isVisible;
    }

    /*
     * Toggles Renderer- and Collider components in all children of this Level
     */
    public void SetVisibilityOfAllChildren(bool isVisible)
    {
        //Debug.Log(this.name + "->SetVisibilityOfAllChildren->" + isVisible);

        this.isActive = isVisible;

        this.SetVisibilityRecursively(this.gameObject, isVisible);
    }

    /*
     * Activates this Level
     */
    public void Activate()
    {
        //Debug.Log(this.name + "->Activate");

        this.SetVisibilityOfAllChildren(true);
        if (entryPrompt != null) entryPrompt.Activate();
        else Debug.LogError(this + " was activated but no initial Prompt was given. Did you foget to reference the entry Prompt?");

        Logger.Log(Classifier.Level.Started, this);
    }

    /*
     * Call when the Level is considered complete and the player is ready to move on to the next
     */
    public void Complete()
    {
        // resolve all unresolved prompts before continuing
        foreach (Prompt ap in new List<Prompt>(Prompt.activePrompts))
            ap.Resolve();

        this.SetVisibilityOfAllChildren(false);
        Logger.Log(Classifier.Level.Completed, this);

        // run preloaded scene
        Level.Continue();
    }

    public void Complete(Prompt p)
    {
        this.Complete();
    }
}
