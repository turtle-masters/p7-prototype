using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManagerScript : MonoBehaviour
{
    public static MinigameManagerScript instance;
    public static int maxLevelNumber = 3;
    public int currentLevel = 1;
    public GameObject gunObject = null;

    public static GameObject playerGameObject = null;
    public static float chemTextDisableDistance = 20f;

    public GameObject[] levelPrefabArray = new GameObject[maxLevelNumber];

    public static int[] goalCounter = new int[maxLevelNumber];
    public static int[] goalMax = {8,40,8};
    public static int[] levelGunMode = {1,2,4};
 
    private void Awake() {
        if (instance != null) {
            Destroy(gameObject);
        }else{
            instance = this;
        }
        playerGameObject=GameObject.FindGameObjectWithTag("Player");
        if(playerGameObject==null)
            Debug.Log("No Player Found");
    }

    private void Start() {
        LevelSetup(currentLevel);
    }

    public void LevelSetup(int _level) {
        for(int i=0;i<maxLevelNumber;i++) {
            if(i==currentLevel-1) {
                levelPrefabArray[i].SetActive(true);
            } else {
                levelPrefabArray[i].SetActive(false);
            }
        }
        //gunObject.GetComponent<ShootingScript>().gunMode=levelGunMode[currentLevel-1];
    }

    public void GoalUpdate(GameObject updateSourceObject) {
        Debug.LogWarning("GOAL UPDATED: " + currentLevel + ", " + updateSourceObject.GetComponent<ChemData>().Name);

        if(currentLevel==1 && updateSourceObject.GetComponent<ChemData>().Name=="ATP") {
            ChemData[] chemDataArray = FindObjectsOfType<ChemData>();
            goalCounter[currentLevel-1] = 0;
            foreach(var tempData in chemDataArray) {
                if(tempData.Name=="ATP") {
                    goalCounter[currentLevel-1]++;
                }
            }
        } else if(currentLevel==2 && updateSourceObject.GetComponent<ChemData>().Name=="Glucose") {
            goalCounter[currentLevel-1]++;
        } else if(currentLevel==3 && updateSourceObject.GetComponent<ChemData>().Name=="Acetaldehyde") {
            goalCounter[currentLevel-1]++;
        }

        if(goalCounter[currentLevel-1] == goalMax[currentLevel-1]) {
            GameObject.Find("TransitionManager").GetComponent<IntroOutro>().fadeOut();
            Invoke("completeGame", 2f);
        }
    }

    public string GetGoalString() {
        if(currentLevel==1) {
            return "ADP's charged: " + goalCounter[0] + "/"+goalMax[0];
        } else if(currentLevel==2) {
            return "Glucose hydrolised: " + goalCounter[1] + " /" + goalMax[1];
        } else if(currentLevel==3) {
            return "Ethanol created: " + goalCounter[2] + " /" + goalMax[2];
        }
        return "Error";
    }

    public int GetCurrentLevel() {
        return currentLevel;
    }

    public void SetCurrentLevel(int _currentLevel) {
        Debug.Log("Current Level was set to " + _currentLevel);
        currentLevel = _currentLevel;
        LevelSetup(currentLevel);
    }

    private void completeGame()
    {
        GameObject f = GameObject.Find("Gun"+currentLevel);
        Debug.Log(f.name);
        f.GetComponent<ShootingScript>().detach();
        Debug.Log("Minigame completed!");
        Scene activeScene = SceneManager.GetActiveScene();
        GameObject[] objectsInScene = activeScene.GetRootGameObjects();
        foreach (GameObject go in objectsInScene)
        {
            Level level = go.GetComponent<Level>();
            if (level != null && level.isActive)
                level.Complete();
        }
    }
}
