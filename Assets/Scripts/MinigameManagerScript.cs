using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManagerScript : MonoBehaviour
{
    public static MinigameManagerScript instance;
    public static int maxLevelNumber = 3;
    public int currentLevel = 1;

    public static GameObject playerGameObject = null;
    public static float chemTextDisableDistance = 8f;

    public GameObject[] levelPrefabArray = new GameObject[maxLevelNumber];

    public static int[] goalCounter = new int[maxLevelNumber];
    public static int[] goalMax = {0,0,10};
 
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
        for(int i=0;i<maxLevelNumber;i++) {
            if(i==currentLevel-1) {
                levelPrefabArray[i].SetActive(true);
            } else {
                levelPrefabArray[i].SetActive(false);
            }
        }
    }

    public void GoalUpdate(GameObject updateSourceObject) {
        if(currentLevel==1 && updateSourceObject.GetComponent<ChemData>().Name=="ATP") {
            ChemData[] chemDataArray = FindObjectsOfType<ChemData>();
            goalCounter[currentLevel-1] = 0;
            foreach(var tempData in chemDataArray) {
                if(tempData.Name=="ATP") {
                    goalCounter[currentLevel-1]++;
                }
            }
        }
    }

    public string GetGoalString() {
        if(currentLevel==1) {
            return "ADP's charged: " + goalCounter[0] + "/"+goalMax[0];
        } else if(currentLevel==2) {
            return "Glucose hydrolised: " + goalCounter[1] + " /" + goalMax[1];
        }
        return "Error";
    }

    public int GetCurrentLevel() {
        return currentLevel;
    }

    public void SetCurrentLevel(int _currentLevel) {
        currentLevel = _currentLevel;
        for(int i=0;i<maxLevelNumber;i++) {
            if(i==currentLevel-1) {
                levelPrefabArray[i].SetActive(true);
            } else {
                levelPrefabArray[i].SetActive(false);
            }
        }
    }
}
